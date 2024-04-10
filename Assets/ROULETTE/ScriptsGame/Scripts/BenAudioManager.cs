using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts.ROULETTE.ScriptsGame.Scripts;

namespace Cat
{

  public class BenAudioManager : MonoBehaviour
  {

    private static BenAudioManager instance;

    public AudioClip [] audioClip;
    private List<AudioSource> AudioSourcePool;
    public AudioSource AudioSourceBGM;

    void Awake()
    {
      instance = this;

      AudioSourcePool = new List<AudioSource>();
      AudioSourceAlloc();
    }

    void Start()
    {

      if (BenSetting.MusicVolume != 0)
        BenAudioManager.MusicPlay();
    }

    public void AudioSourceAlloc()
    {
      AudioSourcePool.Clear();

      for (int i = 0; i < 100; ++i)
      {
        AudioSource aS = (AudioSource)gameObject.AddComponent<AudioSource>();
        aS.loop = false;
        AudioSourcePool.Add(aS);
      }
    }

    private AudioSource AudioSourcePop()
    {
      if (AudioSourcePool.Count <= 0)
        return null;

      AudioSource aS = AudioSourcePool[0];
      AudioSourcePool.RemoveAt(0);
      AudioSourcePool.Add(aS);

      return aS;
    }

    public static void SoundPlay (int iType)
    {
      if (BenSetting.SoundVolume == 0)
        return;

      AudioSource aS = instance.AudioSourcePop();
      aS.PlayOneShot(instance.audioClip[iType]);
    }

    public static void SoundPlay (AudioClip clip)
    {
      if (BenSetting.SoundVolume == 0)
        return;

      if (clip == null)
        return;

      AudioSource aS = instance.AudioSourcePop();
      aS.PlayOneShot(clip);
    }

    public void SoundPlayCoroutine (int iType, float fDelay)
    {
      StartCoroutine(SoundPlayIn(iType, fDelay));
    }

    public IEnumerator SoundPlayIn (int iType, float fDelay)
    {
      if (fDelay > 0.0001f)
        yield return new WaitForSeconds(fDelay);

      SoundPlay(iType);
    }

    public static void MusicPlay()
    {
      if (BenSetting.MusicVolume == 0)
        return;

      instance.AudioSourceBGM.Play();
    }

    public static void ToggleMusic()
    {
      if (MusicIsPlaying())
      {
        MusicStop();
        Debug.Log("Music is stopped");
      } else
      {
        MusicStop();
      }
    }


    public static bool MusicIsPlaying()
    {
      return instance.AudioSourceBGM.isPlaying;
    }

    public static void MusicStop()
    {
      instance.AudioSourceBGM.Stop();
    }
  }
}