using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts.ROULETTE.ScriptsGame.Scripts;

namespace Cat
{

  public class UIOption : MonoBehaviour
  {

    private static UIOption _instance;

    public Toggle uiMusicToggle;
    public Toggle uiSoundToggle;
    public Image Dialog;

    void Awake()
    {
      _instance = this;
      gameObject.SetActive(false);
    }

    void Start() {}

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        Hide();
      }
    }

    void OnEnable()
    {
      uiMusicToggle.isOn = (BenSetting.MusicVolume != 0) ? false : true;
      uiSoundToggle.isOn = (BenSetting.SoundVolume != 0) ? false : true;
    }

    public void MusicToggled (bool value)
    {
      BenAudioManager.SoundPlay(0);
      BenSetting.MusicVolume = value ? 0 : 100;
      BenSetting.Save();

      if (value)
      {
        BenAudioManager.MusicStop();
      } else
      {
        if (!BenAudioManager.MusicIsPlaying())
          BenAudioManager.MusicPlay();
      }
    }

    public void SoundToggled (bool value)
    {
      BenAudioManager.SoundPlay(0);
      BenSetting.SoundVolume = value ? 0 : 100;
      BenSetting.Save();
    }

    public void OnButtonMenu()
    {
      BenAudioManager.SoundPlay(0);
      Application.LoadLevel("Game_Lobby");
    }

    public void OnButtonContinue()
    {
      //Debug.Log("UISGOption::OnButtonContinue");
      BenAudioManager.SoundPlay(0);
      //Hide();
    }

    public void Hide()
    {
      //Debug.Log("UISGOption::Hide");
      gameObject.SetActive(false);
      SceneSlotGame.uiState = 0;
    }

    void _Show()
    {
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.SetActive(true);
      SceneSlotGame.uiState = 1;
      StartCoroutine(BenUtil.ImageScale(Dialog, Dialog.color, 1.0f, 1.1f, 1.0f, 0.1f, 0.0f));
    }

    public static void Show() { _instance._Show(); }

  }

}