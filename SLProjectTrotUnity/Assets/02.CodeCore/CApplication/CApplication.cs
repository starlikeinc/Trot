using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CApplication : CManagerTemplateBase<CApplication>
{
   
	public enum EAppLanguageType
	{
		English,
		Korean,
		China,
		Japan,		
	}
	[SerializeField]
	private int TargetFrameRate = 60; 
	[SerializeField]
	private bool ShowFPS = true;
	
	[SerializeField]
	private Vector2Int ScreenSize = new Vector2Int(1920, 1080);  public Vector2 GetAppBaseResolution() { return ScreenSize; }

	[SerializeField]
	private EAppLanguageType BuildLanguage = EAppLanguageType.English; public EAppLanguageType GetAppLanguage() { return BuildLanguage; }

	private float m_fDeltaTime = 0f;
	private GUIStyle m_pGUIStyle = new GUIStyle();
    private bool m_bDelevopBuild = false;    public bool IsDevelopBuild { get { return m_bDelevopBuild; } }
	//------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
#if UNITY_EDITOR
        m_bDelevopBuild = true;
#endif

        Application.targetFrameRate = TargetFrameRate;
		Application.runInBackground = true;
		Input.simulateMouseWithTouches = true; 
		Input.multiTouchEnabled = true;

		PrivApplicationGUIFont();
	} 

    protected override void OnUnityStart()
	{
		PrivApplicationPrintDebug();
	}

	private void Update()
	{
		if (ShowFPS)
		{
			m_fDeltaTime += (Time.deltaTime - m_fDeltaTime) * 0.1f;
		}
	}

	private void OnGUI()
	{
		if (ShowFPS)
		{
			float msec = m_fDeltaTime * 1000.0f;
			float fps = 1.0f / m_fDeltaTime;
			string text = string.Format("{0:0.0}ms, FPS: {1:0.}", msec, fps);

			GUI.Label(new Rect(0, 0, 200, 100), text, m_pGUIStyle);
			GUI.Label(new Rect(0, 0, 200, 100), text, m_pGUIStyle);
		}
	}

	private void PrivApplicationGUIFont()
	{
		m_pGUIStyle.fontSize = 20;
		m_pGUIStyle.normal.textColor = Color.green;
	}

	private void PrivApplicationPrintDebug()
	{
		Debug.LogFormat("[Screen Info] Screen With {0} / Screen Height {1}", Screen.width, Screen.height);
	}
}




//public static string GetData()
//{
//	string result = "";
//	if (Application.platform == RuntimePlatform.Android)
//	{
//		var osBuild = new AndroidJavaClass("android.os.Build");
//		string brand = osBuild.GetStatic<string>("BRAND");
//		string fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
//		string model = osBuild.GetStatic<string>("MODEL");
//		string menufacturer = osBuild.GetStatic<string>("MANUFACTURER");
//		string device = osBuild.GetStatic<string>("DEVICE");
//		string product = osBuild.GetStatic<string>("PRODUCT");
//		result += "Model : " + model;
//		result += "/";
//		result += "Menufacturer : " + menufacturer;
//		result += "/";
//		result += "Device : " + device;
//		result += "/";
//		result += "Fingerprint : " + fingerPrint;
//		result += "/";
//		result += "Product : " + product;
//	}
//}