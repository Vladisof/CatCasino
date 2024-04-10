using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

namespace Cat
{

  public enum MsgType
  {
    None = -1,
    Ok = 0,
    OkCancel = 1,
    Max = 2,
  };

  public class UIMessage : MonoBehaviour
  {

    private static UIMessage instance;
    public Image Dialog;
    public Text Title;
    public Text Info;
    public Button buttonOk;
    public Button buttonCancel;
    public Button buttonExit;

    private MsgType Type;
    private IntEvent ButtonCallback = null;


    void Start()
    {
      instance = this;
      gameObject.SetActive(false);
      ButtonCallback = new IntEvent();
    }

    void Update()
    {

      if (Input.GetKeyDown(KeyCode.Escape))
      {
        Hide();
      }
    }

    public void OnButtonOk()
    {
      BenAudioManager.SoundPlay(0);
      Callback(0);
    }

    public void OnButtonCancel()
    {
      BenAudioManager.SoundPlay(0);
      Callback(1);
    }

    public void OnButtonClose()
    {
      BenAudioManager.SoundPlay(0);
      Callback(-1);
    }

    public void Callback (int value)
    {
      if (ButtonCallback != null)
      {
        ButtonCallback.Invoke(value);
      }

      Hide();
    }

    public void Hide()
    {
      StartCoroutine(BenUtil.ImageScale(Dialog, Dialog.color, 1.0f, 1.1f, 1.0f, 0.1f, 0.0f));
      StartCoroutine(HideProcess(0.1f));
    }

    public IEnumerator HideProcess (float fDelay)
    {

      if (fDelay > 0.01f)
        yield return new WaitForSeconds(fDelay);

      gameObject.SetActive(false);
      SceneSlotGame.uiState = 0;
    }


    void _Show (string title, string info, MsgType type, UnityAction<int> _Callback)
    {
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.SetActive(true);
      SceneSlotGame.uiState = 1;

      Title.text = title;
      Info.text = info;
      Type = type;
      ButtonCallback.RemoveAllListeners();

      if (_Callback != null)
        ButtonCallback.AddListener(_Callback);

      if (Type == MsgType.None)
      {
        buttonOk.gameObject.SetActive(false);
        buttonCancel.gameObject.SetActive(false);
      } else if (Type == MsgType.Ok)
      {
        buttonOk.gameObject.SetActive(true);
        buttonCancel.gameObject.SetActive(false);

        buttonOk.transform.localPosition = new Vector3(0, -120, 0);
      } else if (Type == MsgType.OkCancel)
      {
        buttonOk.gameObject.SetActive(true);
        buttonCancel.gameObject.SetActive(true);

        buttonOk.transform.localPosition = new Vector3(-100, -120, 0);
        buttonCancel.transform.localPosition = new Vector3(100, -120, 0);
      } else
      {}

      StartCoroutine(BenUtil.ImageScale(Dialog, Dialog.color, 1.0f, 1.1f, 1.0f, 0.1f, 0.0f));
    }

    public static void Show (string title, string info, MsgType type, UnityAction<int> _Callback)
    {
      instance._Show(title, info, type, _Callback);
    }
  }

}