using Cat;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Scripts

{
  public class BenSetting : MonoBehaviour
  {

    public static BenSetting instance;

    public static int MusicVolume = 100;
    public static int SoundVolume = 100;
    public static bool bFirstRun = false;
    private static int StageRunCount = 0;
    private static int ShowInstruction = 100;
    public static BenNumber Gold;

    void Awake()
    {
      instance = this;
      Gold = new BenNumber(BenNumber.IncType.VALUE, "#,##0", 0, 10000000000, 5500);
      Load();
    }

    void Update()
    {
      Gold.Update();
    }

    void OnDestroy()
    {
      Save();
    }

    public static void Save()
    {
      PlayerPrefs.SetInt("MusicVolume", MusicVolume);
      PlayerPrefs.SetInt("SoundVolume", SoundVolume);
      PlayerPrefs.SetInt("StageRunCount", StageRunCount);
      PlayerPrefs.SetInt("ShowInstruction", ShowInstruction);
      PlayerPrefs.SetFloat("Credit", (float)Gold.Target());
    }

    private static void Load()
    {
      if (PlayerPrefs.HasKey("MusicVolume"))
      {
        MusicVolume = PlayerPrefs.GetInt("MusicVolume");
        SoundVolume = PlayerPrefs.GetInt("SoundVolume");
        StageRunCount = PlayerPrefs.GetInt("StageRunCount");
        ShowInstruction = PlayerPrefs.GetInt("ShowInstruction");
        float fGold = PlayerPrefs.GetFloat("Credit");
        Gold.ChangeTo(fGold);
      } else
      {
        Save();
        bFirstRun = true;
      }
    }

    public bool MakeUIInsideScreen (Transform tr)
    {
      Vector3 [] objectCorners = new Vector3[4];
      tr.gameObject.GetComponent<RectTransform>().GetWorldCorners(objectCorners);

      bool IsOurSide = false;
      Vector3 vOffset = Vector3.zero;

      foreach (Vector3 corner in objectCorners)
      {

        if ((corner.x < 0.0f) && (vOffset.x < -corner.x))
        {
          vOffset.x = -corner.x;
          IsOurSide = true;
        }

        if ((corner.x > Screen.width) && (vOffset.x > Screen.width - corner.x))
        {
          vOffset.x = Screen.width - corner.x;
          IsOurSide = true;
        }

        if ((corner.y < 0.0f) && (vOffset.y < -corner.y))
        {
          vOffset.y = -corner.y;
          IsOurSide = true;
        }

        if ((corner.y > Screen.height) && (vOffset.y > Screen.height - corner.y))
        {
          vOffset.y = Screen.height - corner.y;
          IsOurSide = true;
        }

        if (IsOurSide)
        {
          Vector3 pos = tr.position;
          tr.position = pos + vOffset;
          return true;
        }
      }

      return false;
    }
  }


}