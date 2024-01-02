using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TTManagerPatcher : CManagerPatchAssetBundleBase
{
    public static new TTManagerPatcher Instance { get { return CManagerPatchAssetBundleBase.Instance as TTManagerPatcher; } }
    //------------------------------------------------------------------------
    [SerializeField]
    private string MainLabelName = "Main";

    private List<string> m_listAssetBundleLabels = new List<string>();

    //-------------------------------------------------------------------------

    public CPatcherBase.SPatchEvent DoPatcherInitialize(bool bResetEventHandler, string strDownloadURL)
    {
        return ProtPatchAssetBundleInitialize(bResetEventHandler, strDownloadURL);
    }

    public void DoPatcherStart()
    {
        ProtPatchAddressableStart(m_listAssetBundleLabels);
    }

    public void DoPatcherTotalDownloadSize(UnityAction<long> delFinish)
    {
        ProtPatchAddressableTotalDowloadSize(m_listAssetBundleLabels, delFinish);
    }
    //-------------------------------------------------------------------------

    protected override void OnPatcherInitComplete()
    {

    }

    protected override void OnPatcherDownloadSize(string strLabelhName, long iSize)
    {

    }

    protected override void OnPatcherProgress(string strLabelhName, long iDownloadedByte, long iTotalByte, float fProgressPercent, uint iLoadCurrent, uint iLoadMax)
    {

    }

    protected override void OnPatcherEnd()
    {

    }

    protected override void OnPatcherStartLabel(string strLabelName)
    {

    }

    protected override void OnPatcherError(CPatcherBase.EPatchError eErrorType, string strMessage)
    {

    }

}