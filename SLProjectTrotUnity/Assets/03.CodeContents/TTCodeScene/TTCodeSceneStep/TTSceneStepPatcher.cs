using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSceneStepPatcher : MonoBehaviour
{
    [Header("DownloadStart")]
    [SerializeField] private CImage DownloadStartBG;
    [SerializeField] private CText DownloadSize;

    [Header("DownloadProgress")]
    [SerializeField] private CImage ProgressBG;
    [SerializeField] private CImage ProgressBar;
    [SerializeField] private CText ProgressPercent;

    [Header("DownloadError")]
    [SerializeField] private CImage ErrorBG;
    [SerializeField] private CText ErrorDescrip;

    [Header("DownloadWarning")]
    [SerializeField] private CImage WarningBG;
    [SerializeField] private CText WarningDescrip;


    private CPatcherBase.SPatchEvent m_pPatchEvent;

    private string m_strDownloadSizeMB;
    //------------------------------------------------------------------------
    private void Start()
    {
        m_pPatchEvent = TTManagerPatcher.Instance.DoPatcherInitialize(false, TTManagerPatcher.DownloadURL);

        m_pPatchEvent.EventPatchInitComplete += OnPatcherInitComplete;
        m_pPatchEvent.EventPatchProgress += OnPatcherProgress;
        m_pPatchEvent.EventPatchFinish += OnPatcherEnd;
        m_pPatchEvent.EventPatchError += OnPatcherError;
    }

    //------------------------------------------------------------------------
    private void OnPatcherInitComplete()
    {
        if (PrivPatcherCheckInternetConnection() == false)
            return;

        TTManagerPatcher.Instance.DoPatcherTotalDownloadSize(PrivPatcherShowDownloadSize);
    }
    private void OnPatcherProgress(string Name, long _downloadedByte, long _totalByte, float Progress, uint _loadCurrent, uint _loadMax)
    {
        ProgressBar.fillAmount = Progress;
        ProgressPercent.text = $"( {PrivPatcherByteToMB(_downloadedByte).ToString().PadLeft(4)} / {m_strDownloadSizeMB.PadLeft(4)} )MB  {Progress * 100}%";
    }
    private void OnPatcherEnd()
    {
        ProgressBar.fillAmount = 1;
        ProgressPercent.text = $"( {m_strDownloadSizeMB.PadLeft(4)} / {m_strDownloadSizeMB.PadLeft(4)} )MB  100%";

        TTManagerSceneLoader.Instance.DoMgrSceneLoaderGoToMaster(null);
    }
    private void OnPatcherError(CPatcherBase.EPatchError errorType, string message)
    {
        ErrorDescrip.text = PrivPatcherGetErrorMessage(errorType, message);
        ErrorBG.gameObject.SetActive(true);
    }

    //------------------------------------------------------------------------
    private long PrivPatcherByteToMB(long size)
    {
        return (size / 1048576);
        // 1MB = 1048576 Byte
    }

    private void PrivPatcherShowDownloadSize(long size)
    {
        PrivShowFakeDownloadSize();
        //PrivPatcherShowRealDownloadSize(size);
    }
    private void PrivPatcherDownloadStart()
    {
        WarningBG.gameObject.SetActive(false);
        DownloadStartBG.gameObject.SetActive(false);
        ProgressBG.gameObject.SetActive(true);

        PrivStartFakeDownload();
        //PrivPatcherStartRealDownload();
    }
    private void PrivPatcherDownloadCancel()
    {
        Debug.Log("CancelDownload");
    }
    private void PrivPatcherErrorConfirm()
    {
        Debug.Log("ConfirmError");
    }

    private void PrivPatcherShowRealDownloadSize(long size)
    {
        m_strDownloadSizeMB = PrivPatcherByteToMB(size).ToString();
        DownloadSize.text = $"( {m_strDownloadSizeMB}MB )";
        DownloadStartBG.gameObject.SetActive(true);
    }
    private void PrivPatcherStartRealDownload()
    {
        TTManagerPatcher.Instance.DoPatcherStart();
    }

    private string PrivPatcherGetErrorMessage(CPatcherBase.EPatchError errorType, string message)
    {
        string errorMessage = $"{errorType}\n";

        switch (errorType)
        {
            case CPatcherBase.EPatchError.NotInitialized:
                errorMessage += "Patcher 초기화 실패 오류";
                break;
            case CPatcherBase.EPatchError.AlreadyPatchProcess:
                errorMessage += "패치가 이미 진행중입니다";
                break;
            case CPatcherBase.EPatchError.NotEnoughDiskSpace:
                errorMessage += "저장공간이 부족합니다";
                break;
            case CPatcherBase.EPatchError.NetworkDisable:
                errorMessage += "인터넷이 연결되어 있지 않습니다";
                break;
            case CPatcherBase.EPatchError.CatalogUpdateFail:
                errorMessage += "카탈로그 업데이트 실패";
                break;
            case CPatcherBase.EPatchError.PatchFail:
                errorMessage += "패치가 실패하였습니다";
                break;
            case CPatcherBase.EPatchError.HTTPError:
                errorMessage += "프로토콜 에러";
                break;
            case CPatcherBase.EPatchError.WebRequestError:
                errorMessage += "WebRequest 에러";
                break;
        }

        if (message != null) errorMessage += $"\n{message}";

        return errorMessage;
    }
    private void PrivPatcherCheckNetworkType()
    {
        PrivPatcherCheckInternetConnection();

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            WarningDescrip.text = $"앱의 다운로드 크기가 {m_strDownloadSizeMB} MB 입니다. 셀룰러 네트워크를 통해 데이터를 사용하면 추가 요금이 부과될 수 있습니다.";
            WarningBG.gameObject.SetActive(true);
            DownloadStartBG.gameObject.SetActive(false);
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            PrivPatcherDownloadStart();
        }
    }
    private bool PrivPatcherCheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            DownloadStartBG.gameObject.SetActive(false);
            OnPatcherError(CPatcherBase.EPatchError.NetworkDisable, null);
            return false;
        }

        return true;
    }

    #region ===== FakeDownload =====

    private int FakeDownloadSize = 39;
    private float FakeDownloadTime = 1;
    private string FakeDownloadSizeStr;

    private void PrivShowFakeDownloadSize()
    {
        DownloadSize.text = $"( {FakeDownloadSize}MB )";
        DownloadStartBG.gameObject.SetActive(true);
    }

    private void PrivStartFakeDownload()
    {
        StartCoroutine(PrivCOFakeLoad());
    }

    private IEnumerator PrivCOFakeLoad()
    {
        float progressTime = 0;

        FakeDownloadSizeStr = FakeDownloadSize.ToString();

        while (progressTime < FakeDownloadTime)
        {
            float progress = progressTime / FakeDownloadTime;
            int downloadedMB = (int)(progress * FakeDownloadSize);

            ProgressBar.fillAmount = progress;
            ProgressPercent.text = $"( {downloadedMB.ToString().PadLeft(4)} / {FakeDownloadSizeStr.PadLeft(4)} )MB  {(progress * 100).ToString("0.0")}%";

            progressTime += Time.deltaTime;
            yield return null;
        }

        ProgressBar.fillAmount = 1;
        ProgressPercent.text = $"( {FakeDownloadSizeStr.PadLeft(4)} / {FakeDownloadSizeStr.PadLeft(4)} )MB  100%";

        TTManagerSceneLoader.Instance.DoMgrSceneLoaderGoToMaster(null);
    }

    #endregion

    //------------------------------------------------------------------------
    public void HandlePatcherWarningConfirm()
    {
        PrivPatcherDownloadStart();
    }
    public void HandlePatcherWarningCancel()
    {
        PrivPatcherDownloadCancel();
    }
    public void HandlePatcherDownloadYes()
    {
        PrivPatcherCheckNetworkType();
    }
    public void HandlePatcherDownloadNo()
    {
        PrivPatcherDownloadCancel();
    }
    public void HandlePatcherErrorConfirm()
    {
        PrivPatcherErrorConfirm();
    }
}