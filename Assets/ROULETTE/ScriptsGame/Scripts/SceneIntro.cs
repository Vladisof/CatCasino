using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Cat
{

  public class SceneIntro : MonoBehaviour
  {

    public CanvasGroup Logo;
    private float fAge = 0.0f;

    private void Awake()
    {
      Logo.alpha = 0.0f;
    }

    private void Update()
    {

      if (fAge < 3.0f)
      {
        fAge += Time.deltaTime;

        if (fAge > 3.0f)
        {
          Application.LoadLevel("Lobby");
        } else if (fAge > 1.0f)
        {
          Logo.alpha = fAge - 1.0f;
        } else
        {}
      }
    }
  }
}