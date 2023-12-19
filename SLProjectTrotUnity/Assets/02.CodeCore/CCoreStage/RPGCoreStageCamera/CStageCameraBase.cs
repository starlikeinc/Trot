using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

abstract public class CStageCameraBase : CMonoBase
{
    protected Camera m_pCamera = null;	
	private UniversalAdditionalCameraData m_pURPCameraData = null;
    private bool m_bInitialize = false;
	private int m_iCameraCullingMask = 0;
	//------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
        InterStageCameraInitialize();
        PrivStageCameraRegist();

	}

    protected override void OnUnityStart()
    {
        base.OnUnityStart();		
	}

    private void Update()
	{
		OnUnityUpdate(Time.deltaTime);
	}

	private void LateUpdate()
	{
		OnUnityLateUpdate();
	}

    private void FixedUpdate()
    {
        OnUnityFixedUpdate(Time.fixedDeltaTime);
    }

    protected override void OnUnityDestroy()
	{
		base.OnUnityDestroy();
		PrivStageCameraUnRegist();
	}

	//--------------------------------------------------------------------
    internal void InterStageCameraInitialize()
    {
        if (m_bInitialize) return;
        m_bInitialize = true;
        m_pCamera = GetComponent<Camera>();
        m_iCameraCullingMask = m_pCamera.cullingMask;
        m_pURPCameraData = GetComponent<UniversalAdditionalCameraData>();
        OnStageCameraInitialize();
    }

	internal void InterStageCameraHide()
	{
		SetMonoActive(false);
		OnStageCameraShow();
	}

	internal void InterStageCameraShow()
	{
		SetMonoActive(true);
		OnStageCameraHide();
	}

    internal void InterStageCameraFocusOff()
    {
        SetMonoActive(false);
        OnStageCameraFocusOff();
    }

	internal void InterStageCameraDisable()  // 카메라는 존재하나 랜더를 하지 않는다 (오버레이만 랜더하고 싶을때)
	{
		m_iCameraCullingMask = m_pCamera.cullingMask;
		m_pCamera.cullingMask = 0;
		OnStageCameraDisable();
	}

	internal void InterStageCameraEnable()
	{
		m_pCamera.cullingMask = m_iCameraCullingMask;
		OnStageCameraEnable();
	}

    internal void InterStageCameraRemove()
    {
        OnStageCameraRemove();
    }

	//----------------------------------------------------------------
	internal void InterStageCameraOverlayStack(Camera pOverlayCamera, bool bStack)
	{
        if (pOverlayCamera == null) return;

		if (bStack)
		{
            if (m_pURPCameraData.cameraStack.Contains(pOverlayCamera) == true) return;

			List<Camera> StackCameraList = new List<Camera>();
			StackCameraList.Add(pOverlayCamera);

			for (int i = 0; i < m_pURPCameraData.cameraStack.Count; i++)
			{
				StackCameraList.Add(m_pURPCameraData.cameraStack[i]);
			}

			m_pURPCameraData.cameraStack.Clear();

			for (int i = 0; i < StackCameraList.Count; i++)
			{
				m_pURPCameraData.cameraStack.Add(StackCameraList[i]);
			}
		}
		else
		{
			m_pURPCameraData.cameraStack.Remove(pOverlayCamera);
		}
		
		OnStageCameraOverlayStack(pOverlayCamera, bStack);
    }
    //------------------------------------------------------------------
    public Camera GetCamera()
    {
        if (m_pCamera == null)
        {
            m_pCamera = GetComponent<Camera>();
        }
        return m_pCamera; 
    }

    public int GetCameraID() 
    {
        return GetCamera().GetInstanceID(); 
    }

    //--------------------------------------------------------------------
    protected void ProtStageCameraActivate()
    {
        CManagerStageCameraBase.Instance.InterStageCameraActivate(this);
    }

	//---------------------------------------------------------------------
	private void PrivStageCameraRegist()
	{      
		CManagerStageCameraBase.Instance?.InterStageCameraRegist(this, true);
		OnStageCameraRegist();
	}

	private void PrivStageCameraUnRegist()
	{
		CManagerStageCameraBase.Instance?.InterStageCameraRegist(this, false);
		OnStageCameraUnRegist();
	}
    //-----------------------------------------------------------------------
	protected virtual void OnUnityUpdate(float fDeltaTime) { }
	protected virtual void OnUnityLateUpdate() { }
    protected virtual void OnUnityFixedUpdate(float fFixedDeltaTime) { }
    protected virtual void OnStageCameraFocusOff() { }
	protected virtual void OnStageCameraHide() { }
	protected virtual void OnStageCameraShow() { }
	protected virtual void OnStageCameraOverlayStack(Camera pOverlayCamera, bool bStack) { }
	protected virtual void OnStageCameraRegist() { }
	protected virtual void OnStageCameraUnRegist() { }
	protected virtual void OnStageCameraEnable() { }
	protected virtual void OnStageCameraDisable() { }
    protected virtual void OnStageCameraRemove() { }
    protected virtual void OnStageCameraInitialize() { }
}
