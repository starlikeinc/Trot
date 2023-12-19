using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

abstract public class CManagerStageCameraBase : CManagerTemplateBase<CManagerStageCameraBase>
{
	private Dictionary<int, CStageCameraBase> m_mapCameraInstance = new Dictionary<int, CStageCameraBase>();
	private CStageCameraBase m_pMainCamera = null;             
    private Camera m_pUICamera = null;                          public Camera GetMgrUICamera() { return m_pUICamera; }
	//---------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
        PrivStageCameraCollectInstance();

    }
	//----------------------------------------------------------------
	internal void InterStageCameraRegist(CStageCameraBase pCamera, bool bRegist)
	{
		if (bRegist)
		{
            PrivStageCameraRegist(pCamera);
		}
		else
		{
            PrivStageCameraUnRegist(pCamera);
		} 
	}

    internal void InterStageCameraActivate(CStageCameraBase pStageCamera)
    {
        PrivStageCameraRegist(pStageCamera);
        PrivStageCameraActivate(pStageCamera);
    }

	//------------------------------------------------------------------
	public void DoCameraUIOverlayStack(Camera pUICamera, bool bStack)
	{
        m_pUICamera = pUICamera;
		if (m_pMainCamera)
        {
            m_pMainCamera.InterStageCameraOverlayStack(pUICamera, bStack);
        }
	}

	public CStageCameraBase GetStageCameraMain()
	{
		return m_pMainCamera;
	}

	public void DoStageCameraRenderEnableAll(bool bEnable)
	{
        Dictionary<int, CStageCameraBase>.Enumerator it = m_mapCameraInstance.GetEnumerator();
        while(it.MoveNext())
        {
            if (bEnable)
            {
                it.Current.Value.InterStageCameraEnable();
            }
            else
            {
                it.Current.Value.InterStageCameraDisable();
            }
        }
    }

	public CStageCameraBase GetMgrStageCamera() 
    {
        if (m_pMainCamera == null)
        {
            PrivStageCameraCollectInstance();
		}
        return m_pMainCamera; 
    }

	//-----------------------------------------------------------------
	private void PrivStageCameraRegist(CStageCameraBase pStageCamera)
	{
		int CameraID = pStageCamera.GetCameraID();

        if (m_mapCameraInstance.ContainsKey(CameraID) == false)
        {
			pStageCamera.InterStageCameraInitialize();
			m_mapCameraInstance[CameraID] = pStageCamera;

            if (m_pUICamera != null)
            {
                pStageCamera.InterStageCameraOverlayStack(m_pUICamera, true);
            }
        }
    }

	private void PrivStageCameraUnRegist(CStageCameraBase pCamera)
	{
        int CameraID = pCamera.GetCameraID();
        if (m_mapCameraInstance.ContainsKey(CameraID))
        {
            pCamera.InterStageCameraRemove();
            if (m_pMainCamera == pCamera)
            {
                m_pMainCamera = null;
            }

            m_mapCameraInstance.Remove(CameraID);
        }
    }

    private void PrivStageCameraCollectInstance()
    {
        CStageCameraBase [] aStageCamera = FindObjectsByType<CStageCameraBase>(FindObjectsSortMode.None);
        for (int i = 0; i < aStageCamera.Length; i++)
        {           
            PrivStageCameraRegist(aStageCamera[i]);            
            if (i == 0)
            {
                PrivStageCameraActivate(aStageCamera[i]);
			}
        }
    }

    private void PrivStageCameraActivate(CStageCameraBase pStageCamera)
    {
        if (m_pMainCamera == pStageCamera) return;
        if (m_pMainCamera != null)
        {
            m_pMainCamera.InterStageCameraFocusOff();
            m_pMainCamera.InterStageCameraOverlayStack(m_pUICamera, false);
        }

        m_pMainCamera = pStageCamera;
        m_pMainCamera.InterStageCameraShow();
        m_pMainCamera.InterStageCameraOverlayStack(m_pUICamera, true);

        Dictionary<int, CStageCameraBase>.Enumerator it = m_mapCameraInstance.GetEnumerator();
        while(it.MoveNext())
        {
            if (it.Current.Value != m_pMainCamera)
            {
                it.Current.Value.InterStageCameraHide();
            }            
        }
    }
}
