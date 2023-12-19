using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public abstract class CManagerSceneLoaderBase : CManagerAddressableBase<CManagerSceneLoaderBase, CAddressableProviderScene>
{
    protected class SLoadedSceneInfo
	{
		public string			   AddressableName;
		public SceneInstance	   LoadedSceneInstance;
		public UnityAction<string> EventFinishLoad = null;
		public UnityAction<string> EventFinishUnload = null;
	}

    [SerializeField]
    private float MinLoadingTime = 1f;

    private string m_strMainSceneName;		public string p_MainSceneName { get { return m_strMainSceneName; } }
	private SceneInstance m_rMainScene =	new SceneInstance();
	private float m_fMainSceneStartTime = 0;
    private bool m_bLoading = false;
	private UnityAction m_delMainSceneLoadFinish = null;
	private UnityAction<string, float> m_delMainSceneLoadProgress = null;
	
	private Dictionary<string, SLoadedSceneInfo>	m_dicSceneInstance = new Dictionary<string, SLoadedSceneInfo>();
	private Queue<SLoadedSceneInfo>					m_queUnloadScene   = new Queue<SLoadedSceneInfo>();
	//------------------------------------------------------------------------
	protected override void OnUnityUpdate()
	{
		base.OnUnityUpdate();
		CText[] aTest = FindObjectsByType<CText>(FindObjectsSortMode.None);
	}

	protected sealed override void OnAddressableLoadScene(string strAddressableName, AsyncOperationHandle pLoadedHandle, SceneInstance _SceneInstance)
	{
		if (m_dicSceneInstance.ContainsKey(strAddressableName))
		{
			SLoadedSceneInfo LoadScene = m_dicSceneInstance[strAddressableName];
			LoadScene.LoadedSceneInstance = _SceneInstance;
			LoadScene.EventFinishLoad?.Invoke(strAddressableName);
			OnSceneAdditiveLoad(strAddressableName, _SceneInstance.Scene);
		}
	}
	//------------------------------------------------------------------------
	protected void ProtSceneLoaderSingle(string strAddressableName, UnityAction delFinish, UnityAction<string, float> delProgress = null, bool bForceDelete = false) // DontDestroy까지 모두 삭제해버린다. 완전 클리어 조심. 주로 런타임 업데이트시 어플리케이션 다시 시작할때 
	{
		if (SceneManager.GetActiveScene().name == strAddressableName)
		{
			delFinish?.Invoke();
			return;
		}

		if (m_bLoading)
		{
			Debug.LogErrorFormat("[Scene Loader] Dont call before Scene Loading is finish");
			return;
		}

		m_bLoading = true;

		m_strMainSceneName = strAddressableName;
		m_fMainSceneStartTime = Time.time;
		m_delMainSceneLoadFinish = delFinish;
		m_delMainSceneLoadProgress = delProgress;
		PrivSceneLoaderClearAll(bForceDelete);
		PrivLoadMainScene(strAddressableName);
	}

	protected void ProtSceneLoaderAdditive(string strAddressableName, UnityAction<string> delFinish, UnityAction<string, float> delProgress)
	{
		if (m_dicSceneInstance.ContainsKey(strAddressableName))
		{
			delFinish?.Invoke(strAddressableName);
			return;
		}

		SLoadedSceneInfo LoadedScene = new SLoadedSceneInfo();
		LoadedScene.AddressableName = strAddressableName;
		LoadedScene.EventFinishLoad = delFinish;

		m_dicSceneInstance[strAddressableName] = LoadedScene;
		RequestLoad(strAddressableName, null, delProgress);
	}

	protected void ProtSceneLoaderAdditiveUnload(string strAddressableName, UnityAction<string> delFinish)
	{
		if (m_dicSceneInstance.ContainsKey(strAddressableName))
		{
			m_bLoading = true;
			SLoadedSceneInfo SceneInfo = m_dicSceneInstance[strAddressableName];
			SceneInfo.EventFinishUnload = delFinish;
			m_queUnloadScene.Enqueue(SceneInfo);
			m_dicSceneInstance.Remove(strAddressableName);
		}
	}


	//-----------------------------------------------------------------------
	private IEnumerator CoroutineUnloadScene()
	{
		while(true)
		{
			if (m_queUnloadScene.Count > 0)
			{
				SLoadedSceneInfo pLoadedScene = m_queUnloadScene.Dequeue();
				AsyncOperationHandle<SceneInstance> AsyncHandle = Addressables.UnloadSceneAsync(pLoadedScene.LoadedSceneInstance);
				AsyncHandle.Completed += (AsyncOperationHandle<SceneInstance> _Result) =>
				{			
					OnSceneAdditiveUnload(pLoadedScene.AddressableName, _Result.Result.Scene);
					pLoadedScene.EventFinishUnload?.Invoke(pLoadedScene.AddressableName);
					if (m_queUnloadScene.Count == 0)
					{
						m_bLoading = false;
					}

				};
				yield return AsyncHandle;
			}
			else
			{
				yield break;
			}
		}
	}

    private void PrivLoadMainScene(string strAddressableName)
    {
		StartCoroutine(CoroutineLoadMainScene(strAddressableName));		
	}    

    private IEnumerator CoroutineLoadMainScene(string strAddressableName)
    {        
        AsyncOperationHandle<SceneInstance> AsyncHandle = Addressables.LoadSceneAsync(strAddressableName, LoadSceneMode.Single, false);
	
		while (true)
        {
            if (AsyncHandle.IsValid() == false)
            {
                OnAddressableError(strAddressableName, AsyncHandle.OperationException.ToString());
                yield break;
            }
			else if (AsyncHandle.IsDone)
			{
				float fConsumetime = m_fMainSceneStartTime - Time.time;
				if (fConsumetime < MinLoadingTime)
				{
					m_delMainSceneLoadProgress?.Invoke(m_strMainSceneName, 1f);
					yield return new WaitForSeconds(MinLoadingTime - fConsumetime);
				}

				yield return AsyncHandle.Result.ActivateAsync();
				m_rMainScene = AsyncHandle.Result;				

				m_bLoading = false;
				m_delMainSceneLoadFinish?.Invoke();
				OnSceneFinishMain(m_strMainSceneName, m_rMainScene.Scene);
				GlobalManagerSceneReset(); // 신 로드 직후 모든 메니저에게 통보
				yield break;

			}
            else
            {
                m_delMainSceneLoadProgress?.Invoke(strAddressableName, AsyncHandle.PercentComplete);
                yield return null;
            }
        }
    }

	private void PrivSceneLoaderClearAll(bool bForceDelete)
	{      
		m_dicSceneInstance.Clear();
		m_queUnloadScene.Clear();

		if (bForceDelete)
		{
			GameObject [] aGameObject = m_rMainScene.Scene.GetRootGameObjects();
			for (int i = 0; i < aGameObject.Length; i++)
			{
				Destroy(aGameObject[i]);
			}
		}
	}
	//-----------------------------------------------------------------------
	protected virtual void OnSceneFinishMain(string _addressableName, Scene _mainScene) { }
	protected virtual void OnSceneAdditiveLoad(string _addressableName, Scene _mainScene) { }
	protected virtual void OnSceneAdditiveUnload(string _addressableName, Scene _mainScene) { }
	
}
