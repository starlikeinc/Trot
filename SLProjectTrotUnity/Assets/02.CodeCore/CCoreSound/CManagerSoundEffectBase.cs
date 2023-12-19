using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BGM 페이드 인아웃 
// 각종 효과음 출력 

public abstract class CManagerSoundEffectBase : CManagerTemplateBase<CManagerSoundEffectBase>
{
	public enum ESoundPlayType
	{	
		Exclusive,		// 재생중이면 재생하지 않는다
		Reset,			// 재생을 중지하고 다시 재생한다.
		PlayOneShot,	// 중복 재생을 한다.
	}



	private class SSoundChannelInfo
	{
		public int				SoundChannelID;
		public ESoundPlayType	PlayType;					//    true일 경우 재생 중 일때는 다시 재생되지 않는다.
		public AudioSource		AudioOutput;
	}

	private class SSoundBGMInfo
	{
		public int ChannelID;
		public AnimationCurve FadeCurveIn = null;
		public AnimationCurve FadeCurveOut = null;
		public AudioSource    AudioOutput;
	}


	private Dictionary<int, SSoundChannelInfo>	m_mapSoundChannel = new Dictionary<int, SSoundChannelInfo>();
	private Dictionary<int, SSoundBGMInfo>		m_mapBGM = new Dictionary<int, SSoundBGMInfo>();
	//-----------------------------------------------------------------------------------------
	protected void ProtMgrSoundEffectChannelAdd(int SoundChannelID, ESoundPlayType ePlayType, AudioSource pAudioSource)
	{
		if (m_mapSoundChannel.ContainsKey(SoundChannelID) == false)
		{
			SSoundChannelInfo pSoundEffectGroup = new SSoundChannelInfo();
			pSoundEffectGroup.SoundChannelID = SoundChannelID;
			pSoundEffectGroup.PlayType = ePlayType;
			pSoundEffectGroup.AudioOutput = pAudioSource;

			m_mapSoundChannel.Add(SoundChannelID, pSoundEffectGroup);
		}
	}

	//----------------------------------------------------------------------------------------
	protected void ProtMgrSoundEffecPlay(int SoundChannlID, AudioClip pPlayClip)
	{
		if (m_mapSoundChannel.ContainsKey(SoundChannlID))
		{
			SSoundChannelInfo pSoundChannel = m_mapSoundChannel[SoundChannlID];
			PrivMgrSoundEffectPlay(pSoundChannel.PlayType, pSoundChannel.AudioOutput, pPlayClip);	
		}
	}
	//---------------------------------------------------------------------------------------
	private void PrivMgrSoundEffectPlay(ESoundPlayType ePlayType, AudioSource pAudioSource, AudioClip pPlayClip)
	{
		if (ePlayType == ESoundPlayType.Exclusive)
		{
			if (pAudioSource.isPlaying == false)
			{
				pAudioSource.clip = pPlayClip;
				pAudioSource.Play();
			}
		}
		else if (ePlayType == ESoundPlayType.PlayOneShot)
		{
			pAudioSource.PlayOneShot(pPlayClip);
		}
		else if (ePlayType == ESoundPlayType.Reset)
		{
			pAudioSource.Stop();
			pAudioSource.clip = pPlayClip;
			pAudioSource.Play();
		}
	}
}
