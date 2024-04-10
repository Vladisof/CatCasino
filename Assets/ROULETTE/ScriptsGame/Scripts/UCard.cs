using UnityEngine;
using UnityEngine.UI;

namespace Cat
{

  public enum CardType
  {
    Clover = 0,
    Heart = 1,
    Spade = 2,
    Diamond = 3,
  };

  public class UCard : MonoBehaviour
  {

    private Transform _tr;
    public Image Front;
    public Image IconCenter;
    public Image IconTopLeft;
    public Text NumberTopLeft;
    public Image IconBottomRight;
    public Text NumberBottomRight;
    public Image Back;

    public Sprite [] Sprites;
    public CardType Symbol;
    public int Number;

    public bool FrontSide = true;
    private bool _inFlipping = false;
    private float _flippingAge = 0.0f;
    private float _flippingPeriod = 0.6f;

    private void Awake()
    {
      _tr = transform;
      SetSide(true);
    }

    private void Update()
    {

      if (_inFlipping)
      {

        if ((_flippingAge < _flippingPeriod * 0.5f) && (_flippingAge + Time.deltaTime >= _flippingPeriod * 0.5f))
          SetSide(!FrontSide);

        _flippingAge += Time.deltaTime;
        float value = 2.0f / _flippingPeriod;
        float rotY = 90.0f * Mathf.PingPong(_flippingAge * value, 1.0f);

        if (_flippingAge > _flippingPeriod)
        {
          rotY = 0.0f;
          _inFlipping = false;
        }

        _tr.localRotation = Quaternion.Euler(0, rotY, 0);
      }

    }

    public void SetSide (bool bFront)
    {
      FrontSide = bFront;
      Front.gameObject.SetActive(FrontSide);
      Back.gameObject.SetActive(!FrontSide);
    }

    public int GetIndexof52()
    {
      return (int)Symbol * 13 + Number - 1;
    }

    public void SetSymbolNumber (int indexof52)
    {
      CardType type = (CardType)(indexof52 / 13);
      int number = indexof52 - (int)type * 13 + 1;
      SetSymbolNumber(type, number);
    }

    public void SetSymbolNumber (CardType type, int number)
    {
      Symbol = type;
      Number = number;

      string strNumber;

      if (number == 11)
        strNumber = "J";
      else if (number == 12)
        strNumber = "Q";
      else if (number == 13)
        strNumber = "K";
      else if (number == 1)
        strNumber = "A";
      else
        strNumber = number.ToString();

      Sprite sprSymbol = Sprites[(int)type];

      IconCenter.sprite = sprSymbol;
      IconTopLeft.sprite = sprSymbol;
      NumberTopLeft.text = strNumber;
      IconBottomRight.sprite = sprSymbol;
      NumberBottomRight.text = strNumber;

      Color color = IsRedColor(type) ? Color.red : Color.black;
      IconCenter.color = color;
      IconTopLeft.color = color;
      NumberTopLeft.color = color;
      IconBottomRight.color = color;
      NumberBottomRight.color = color;
    }

    public static bool IsRedColor (CardType type)
    {
      bool isRed = ((type == CardType.Heart) || (type == CardType.Diamond)) ? true : false;
      return isRed;
    }

    public void Flip()
    {
      if (_inFlipping)
        return;

      _inFlipping = true;
      _flippingAge = 0.0f;
    }
  }

}