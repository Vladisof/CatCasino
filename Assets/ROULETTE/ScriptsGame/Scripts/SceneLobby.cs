using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts.ROULETTE.ScriptsGame.Scripts;
using TMPro;

namespace Cat
{

  public class SceneLobby : MonoBehaviour
  {

    public GameObject prefabSceneItem;
    public RectTransform rtScrollContent;
    public TextMeshProUGUI textGold;

    private void Start()
    {
      Time.timeScale = 1;
      print("PlayerPrefs.GetInt = " + PlayerPrefs.GetInt("mk_slot_coins"));
      print("PlayerPrefs.GetInt = " + PlayerPrefs.GetInt("mk_slot_coins"));
      BenSetting.Gold.ChangeDelta(PlayerPrefs.GetInt("mk_slot_coins"));

      BenSetting.Gold.AddUIText(textGold);
    }

    public void lllll()
    {}

    void Update()
    {

      if (Input.GetKeyDown(KeyCode.Escape))
      {
        UIMessage.Show("Quit", "Do you want to quit this program ?", MsgType.OkCancel, MessageQuitResult);
      }

    }

    public void MessageQuitResult (int value)
    {
      if (value == 0)
      {
        Application.Quit();
      }
    }

    public void OnButtonUser()
    {
      BenAudioManager.SoundPlay(0);
    }

    public void OnButtonShop()
    {
      BenAudioManager.SoundPlay(0);
      BenSetting.Gold.ChangeDelta(100);
      BenSetting.Save();
    }

    public void OnButtonOption()
    {
      BenAudioManager.SoundPlay(0);
      UIOption.Show();
    }

    public void OnButtonPlay()
    {
      BenAudioManager.SoundPlay(0);
    }

    public void OnButtonGet()
    {
      BenAudioManager.SoundPlay(0);
    }

    public void OnButtonSelected (int value)
    {

      BenAudioManager.SoundPlay(0);

      if (value == 3)
        Application.LoadLevel("scene");

      else
        Application.LoadLevel("SlotGame");
    }
  }
}