using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

//  오브젝트 풀링 기반으로 에셋번들 메모리 사이즈에 따른 자율 해제 리소스 핸들을 구현 

public abstract class CManagerPrefabAutoReleaseBase : CManagerPrefabPoolBase
{       
    public enum ELoadingQuality
    {
        Low,        // 퍼포먼스 비용이 가장 적으나 전체 로딩시간은 길다.  
        Medium,     // 일반   디바이스에서 사용할수 있다.
        High,       // 고성능 디바이스에서 사용할 수 있다.
    }

    protected enum EAutoReleaseFilterType
    {
        None,                      // 필터링 하지 않는다
        ReferenceCountAndTime,     // 레퍼런스가 가장 작고 오래된 리소스를 1개 해재
        ReferenceCountOnly,        // 레퍼런스가 가장 작은 객체를 모두 해재 
        TimeOnly,                  // 오래된 리소스를 1개 해제
    }

    public class SLoadingQuality
    {
        public uint AutoReleaseMagaByteCap;
        public int  UploadSliceTime;
        public int  UploadBufferSize;
        public SLoadingQuality(uint _poolSize, int _sliceTime, int _bufferSize) { AutoReleaseMagaByteCap = _poolSize; UploadSliceTime = _sliceTime; UploadBufferSize = _bufferSize; }
    }
     
    private class SAssetBundleInfo
    {
        public string       BundleName;
        public string       FilePath;
        public long         DiskSize = 0;
        public int          RefCounter = 0;
        public AssetBundle  Bundle = null;
    }

    private class SAddressableInfo
    {
        public string                 AddressableName;
        public List<SAssetBundleInfo> DependencyAssetBundle = new List<SAssetBundleInfo>();
        public uint                   RefCount = 0;
        public float                  LastRefTime = 0;
    }

    private SLoadingQuality mQualityLow = new SLoadingQuality(512, 1, 4);
    private SLoadingQuality mQualityMedium = new SLoadingQuality(1024, 2, 16);
    private SLoadingQuality mQualityHigh = new SLoadingQuality(1536, 3, 32);

    [SerializeField]
    private ELoadingQuality        LoadingQuality = ELoadingQuality.Low;
   
    private bool m_bActivate = true;
    //----------------------------------------------------------------------

    private Dictionary<string, SAddressableInfo> m_dicAddressableInfo = new Dictionary<string, SAddressableInfo>();
    private Dictionary<string, SAssetBundleInfo> m_dicAssetBundleInfo = new Dictionary<string, SAssetBundleInfo>();
    private uint   m_iAutoReleaseCap = 0;
    private uint   m_iUsePoolSize = 0;
    protected long m_lAssetBundleUseSize = 0;
    //------------------------------------------------------------------------
    protected override void OnUnityAwake()
    {
        base.OnUnityAwake();
     //   InitializeLoadingQuality();
    }

	protected override void OnUnityUpdate()
	{
		base.OnUnityUpdate();

        if (m_bActivate) 
		{

		}
	}
	protected override void OnPrefabOriginLoaded(string _addressableName, AsyncOperationHandle pLoadedHandle)
    {
        if (m_dicAddressableInfo.ContainsKey(_addressableName)) return;

        SAddressableInfo AddInfo = new SAddressableInfo();
        AddInfo.AddressableName = _addressableName;
        m_dicAddressableInfo[_addressableName] = AddInfo;

        Addressables.LoadResourceLocationsAsync(_addressableName).Completed += (AsyncOperationHandle<IList<IResourceLocation>> ResourceLocation) =>
        {
            if (ResourceLocation.Result.Count > 0)
            {
                if (ResourceLocation.Result[0].Dependencies != null)
                {
                    for (int i = 0; i < ResourceLocation.Result[0].Dependencies.Count; i++)
                    {
                        IResourceLocation Location = ResourceLocation.Result[0].Dependencies[i];
                        AssetBundleRequestOptions RequestOptions = Location.Data as AssetBundleRequestOptions;
                       
                        SAssetBundleInfo BundleInfo = AddAssetBundleInfo(RequestOptions.BundleName, Location.InternalId, RequestOptions.BundleSize);
                        AddInfo.DependencyAssetBundle.Add(BundleInfo);
                    }
                }
            }
        };
    }
    protected override void OnPrefabCloneInstance(string _addressableName, GameObject _cloneInstance) 
    {
        if (m_dicAddressableInfo.ContainsKey(_addressableName))
		{
            SAddressableInfo AddInfo = m_dicAddressableInfo[_addressableName];
            AddInfo.RefCount++;
            AddInfo.LastRefTime = Time.time;
		}
    }
    
    protected override void OnPrefabCloneRemove(string _addressableName, GameObject _removeClone) 
    {
        if (m_dicAddressableInfo.ContainsKey(_addressableName))
        {
            SAddressableInfo AddInfo = m_dicAddressableInfo[_addressableName];
            AddInfo.RefCount--;
        }
    }

	protected override void OnPrefabOriginRemove(string _addressableName, AsyncOperationHandle pRemovedHandle)
	{
        if (m_dicAddressableInfo.ContainsKey(_addressableName))
        {
            SAddressableInfo AddInfo = m_dicAddressableInfo[_addressableName];

            for (int i = 0; i < AddInfo.DependencyAssetBundle.Count; i++)
			{
                AddInfo.DependencyAssetBundle[i].RefCounter--;
			}
        }       
    }

	protected override void OnAddressableLoadGameObject(string strAddressableName, AsyncOperationHandle pLoadedHandle, GameObject pLoadedGameObject)
	{
		// 로드된 메모리를 체크하자
	}

    //-------------------------------------------------------------------------
    private void InitializeLoadingQuality()
    {
        SLoadingQuality QualityInfo = mQualityLow;
        switch (LoadingQuality)
        {
            case ELoadingQuality.Low:
                QualityInfo = mQualityLow;
                break;
            case ELoadingQuality.Medium:
                QualityInfo = mQualityMedium;
                break;
            case ELoadingQuality.High:
                QualityInfo = mQualityHigh;
                break;
        }
        m_iAutoReleaseCap = QualityInfo.AutoReleaseMagaByteCap * 1048576;  // 메가바이트 => 바이트 변환 상수 
		QualitySettings.asyncUploadTimeSlice = QualityInfo.UploadSliceTime;
		QualitySettings.asyncUploadBufferSize = QualityInfo.UploadBufferSize;
		Application.backgroundLoadingPriority = ThreadPriority.Low;

		Application.backgroundLoadingPriority = ThreadPriority.Low;
	}

    private SAssetBundleInfo AddAssetBundleInfo(string _assetBundleName, string _assetBundleFileName, long _fileSize)
    {
        SAssetBundleInfo BundleInfo = null;
        if (m_dicAssetBundleInfo.ContainsKey(_assetBundleName))
		{
            BundleInfo = m_dicAssetBundleInfo[_assetBundleName];
		}
        else
		{
            BundleInfo = new SAssetBundleInfo();
            BundleInfo.BundleName = _assetBundleName;
            BundleInfo.FilePath = _assetBundleFileName;
            BundleInfo.DiskSize = _fileSize;

            m_dicAssetBundleInfo[_assetBundleName] = BundleInfo;
            m_lAssetBundleUseSize += _fileSize;

            List<AssetBundle> BundleList = AssetBundle.GetAllLoadedAssetBundles().ToList();
            for (int i = 0; i < BundleList.Count; i++)
            {
                if (BundleList[i].name == _assetBundleName)
				{
                    BundleInfo.Bundle = BundleList[i];
                    break;
				}
            }
		}

        BundleInfo.RefCounter++;
        return BundleInfo;
	}
}
