using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public abstract class TTStagePlayMovieBase : TTStagePlayBase
{
	[SerializeField]
	private VideoPlayer MoviePlayer;
	[SerializeField]
	private MeshRenderer MeshDrawer;

	private bool m_bMoviePlayerPlaying = false;
	//-------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
		MeshDrawer.sortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(gameObject.layer));
		MeshDrawer.sortingOrder = 100;
		
	}


	protected override void OnStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
	{
		PrivStageMovieReset();
		PrivStageMovieLoadAsset(hLoadID, ()=> {
			UIManager.Instance.UIHide<UIFrameLoadingScreen>();
			delFinish.Invoke();
		});
	}

	protected override void OnStageReLoaded(uint hLoadID)
	{
		base.OnStageReLoaded(hLoadID);
		PrivStageMovieReset();
	}

	protected override void OnStageStart()
	{
		if (m_bMoviePlayerPlaying && MoviePlayer.clip == null)
		{
			//Error!
			return;
		}
		m_bMoviePlayerPlaying = true;

		MoviePlayer.Prepare();
		MoviePlayer.prepareCompleted += (VideoPlayer pVideoPlayer) =>
		{
			pVideoPlayer.Play();
			pVideoPlayer.started += (VideoPlayer pVideoPlayer) =>
			{
				OnStageMovieStart(pVideoPlayer.length);
			};
		};
	}

	//-------------------------------------------------------------------
	private void PrivStageMovieLoadAsset(uint hLoadID, UnityAction delFinish)
	{
		TTManagerResourceLoader.Instance.DoMgrResourceVideoClip("TTMovieBasic_JJiniya_yeongtak", (VideoClip pVideoClip) => {
			MoviePlayer.clip = pVideoClip;
			delFinish.Invoke();
		});
	}

	private void PrivStageMovieReset()
	{
		m_bMoviePlayerPlaying = false;
	}


	//------------------------------------------------------------------------
	protected virtual void OnStageMovieStart(double fLength) { }
	protected virtual void OnStageMovieStop() { }
}
