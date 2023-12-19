using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
// [개요] Texture와 같은 일반 리소스 객체를 로드하기 위한 레이어
// [주의] 로드 즉시 해당 에셋번들을 해재하므로 같은 에셋을 여러번 호출하면 각자 메모리가 할당된다 (참조로 로딩하고 싶으면 Pool을 사용할것)
public abstract class CManagerResourceBase : CManagerAddressableBase<CManagerResourceBase, CAddressableProviderObject>
{   public static new CManagerResourceBase Instance { get { return CManagerAddressableBase<CManagerResourceBase, CAddressableProviderObject>.Instance as CManagerResourceBase; } }
    //-----------------------------------------------------------------------------------
    protected override void OnAddressableError(string _addressableName, string _error)
    {
        Debug.LogError(string.Format("[Addressable] {0} Error : {1}", _addressableName, _error));
    }
    //----------------------------------------------------------------------

    public void LoadResourceAudioClip(string strAssetName, UnityAction<AudioClip> delFinish)
    {

    }
    
    public void LoadResourceTexture(string strAsssetName)
    {

    }

    public void LoadResourceRawImage()
    {

    }
     
    public void LoadResourceTextAsset(string strAssetName, UnityAction<TextAsset> delFinish)
    {
        RequestLoad(strAssetName, (string strLoadedName, AsyncOperationHandle pLoadedHandle) => { 
            if (pLoadedHandle.Result != null)
            {
                if (delFinish == null)
                {
                    Addressables.Release(pLoadedHandle);
                }
                else
                {

                    delFinish.Invoke(pLoadedHandle.Result as TextAsset);
                }
            }
        });

	}

    public void ReleaseAsset(object pAsset)
    {
        Addressables.Release(pAsset);
    }
    //----------------------------------------------------------------------------------------


}
