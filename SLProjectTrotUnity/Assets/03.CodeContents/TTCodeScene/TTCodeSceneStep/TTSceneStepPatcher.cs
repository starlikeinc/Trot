using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TTSceneStepPatcher : MonoBehaviour
{
    [Header("DownloadStart")]
    [SerializeField] private CImage ImgBGDownload;
    [SerializeField] private CText TxtDownloadSize;

    [Header("DownloadProgress")]
    [SerializeField] private CImage ImgBGProgress;  
    [SerializeField] private CImage ImgProgressBar; 
    [SerializeField] private CText TxtDownloadProgress;

    [Header("DownloadError")]
    [SerializeField] private CImage ImgBGError;
    [SerializeField] private CText TxtErrorDescrip;


    private CPatcherBase.SPatchEvent mPatchEvent;


   

    private long mDownloadSize;
    //------------------------------------------------------------------------
    private void Start()
    {
        mPatchEvent = TTManagerPatcher.Instance.DoPatcherInitialize(false, TTManagerPatcher.DownloadURL);

        mPatchEvent.EventPatchInitComplete += OnPatcherInitComplete;
        mPatchEvent.EventPatchProgress += OnPatcherProgress;
        mPatchEvent.EventPatchFinish += OnPatcherEnd;
		mPatchEvent.EventPatchError += OnPatcherError;
	}

	//------------------------------------------------------------------------
	private void OnPatcherInitComplete()
    {
        TTManagerPatcher.Instance.DoPatcherTotalDownloadSize(PrivShowDownloadSize);
    }
    private void OnPatcherProgress(string Name, long _downloadedByte, long _totalByte, float Progress, uint _loadCurrent, uint _loadMax)
    {
        ImgProgressBar.fillAmount = Progress;
        TxtDownloadProgress.text = $"( {_downloadedByte} / {mDownloadSize} ) MB {Progress * 100}%";
    }
    private void OnPatcherEnd()
    {
        ImgProgressBar.fillAmount = 1;
        TxtDownloadProgress.text = "100 %";

        TTManagerSceneLoader.Instance.DoMgrSceneLoaderGoToMaster(null);
    }
    private void OnPatcherError(CPatcherBase.EPatchError errorType, string message)
    {
        
        TxtErrorDescrip.text = $"{errorType}\n{message}";
        ImgBGError.gameObject.SetActive(true);
    }
    //------------------------------------------------------------------------

    private void PrivShowDownloadSize(long size)
    {
        PrivShowFakeDownloadSize();
        //PrivShowRealDownloadSize(size);
    }
    private void PrivStartDownload()
    {
        ImgBGDownload.gameObject.SetActive(false);
        ImgBGProgress.gameObject.SetActive(true);

        PrivStartFakeDownload();
        //PrivStartRealDownload();
    }
    private void PrivCancelDownload()
    {
        Debug.Log("CancelDownload");
    }
    private void PrivConfirmError()
    {
        Debug.Log("ConfirmError");
    }

    private void PrivShowRealDownloadSize(long size)
    {
        mDownloadSize = size;
        TxtDownloadSize.text = $"( {size.ToString("0")}MB )";
    }
    private void PrivStartRealDownload()
    {
        TTManagerPatcher.Instance.DoPatcherStart();
    }

    #region ===== FakeDownload =====

    private int FakeDownloadSize = 39;
    private float FakeDownloadTime = 2;

    private void PrivShowFakeDownloadSize()
    {
        TxtDownloadSize.text = $"( {FakeDownloadSize}MB )";
    }

    private void PrivStartFakeDownload()
    {
        StartCoroutine(PrivCOFakeLoad());
    }

    private IEnumerator PrivCOFakeLoad()
    {
        float progressTime = 0;

        while(progressTime < FakeDownloadTime)
        {
            float progress = progressTime / FakeDownloadTime;
            int downloadedMB = (int)(progress * FakeDownloadSize);

            ImgProgressBar.fillAmount = progress;
            TxtDownloadProgress.text = $"( {downloadedMB} / {FakeDownloadSize} ) MB {(progress * 100).ToString("0.0")}%";

            progressTime += Time.deltaTime;
            yield return null;
        }

        ImgProgressBar.fillAmount = 1;
        TxtDownloadProgress.text = $"( {FakeDownloadSize} / {FakeDownloadSize} ) MB 100%";

        TTManagerSceneLoader.Instance.DoMgrSceneLoaderGoToMaster(null);
    }

    #endregion

    //------------------------------------------------------------------------

    public void HandleOnClickButtonDownloadYes()
    {
        PrivStartDownload();
    }
    public void HandleOnClickButtonDownloadNo()
    {
        PrivCancelDownload();
    }
    public void HandleOnClickButtonErrorConfirm()
    {
        PrivConfirmError();
    }
}
