using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TTSceneStepLogo : MonoBehaviour
{   
    void Start()
    {
        Invoke("HandleStepLogoFinish", 1f);
        
    }
    //-----------------------------------------------------
    private void HandleStepLogoFinish()
	{
        SceneManager.LoadScene("TTScenePatcher");
	}
}
