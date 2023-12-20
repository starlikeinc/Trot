using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TTSceneStepPatcher : MonoBehaviour
{
    void Start()
    {
		Invoke("HandleStepPatcherFinish", 1f);
	}


	//-------------------------------------------------------------
	private void HandleStepPatcherFinish()
	{
		
	}
}
