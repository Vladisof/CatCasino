using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Cat
{

  public class UDoubleGame : MonoBehaviour
  {

    private static UDoubleGame _instance;

    public Image Dialog;
    public Text Title;
    public UCard CardDeck;
    public UCard CardCenter;
    public UCard [] CardsRight;
    public Button [] Buttons;
    public Image DoubleBackground;
    public Image QuadBackground;
    public Text DoubleText;
    public Text QuadText;
    public Animator animator;

    private int _selectCountMax = 5;
    private int _selectCount = 0;

    private float _coinStart;
    private float _coinWins;
    private List<int> _deck = new List<int>();
    private int _cardIndexInDeck = 0;

    private bool _inShowResult = false;
    private bool _inputEnabled = true;

    private void Awake()
    {
      _instance = this;
      gameObject.SetActive(false);

    }

    private void Update()
    {

      if (_inputEnabled)
      {
        float fRatio = Mathf.PingPong(Time.time * 2.0f, 1.0f) * 0.25f + 0.5f;
        Color color = new Color(fRatio, fRatio, fRatio, 1.0f);
        DoubleBackground.color = color;
        QuadBackground.color = color;
      }
    }

    private void Shuffle()
    {
      _deck.Clear();

      for (int i = 0; i < 52; ++i)
        _deck.Add(i);

      _deck.Sort((x, y) => Random.value < 0.5f ? -1 : 1);
      _cardIndexInDeck = 0;
    }

    private void InputEnable (bool bEnable)
    {

      _inputEnabled = bEnable;

      for (int i = 0; i < Buttons.Length; ++i)
      {
        Buttons[i].interactable = bEnable;
      }
    }

    private void UpdateText()
    {
      Title.text = "Win <color=yellow>" + _coinWins.ToString("#,##0.0") + "</color> Credits";
      DoubleText.text = "Double to \n<color=yellow>" + (_coinWins * 2.0f).ToString("#,##0.0") + "</color> credits";
      QuadText.text = "Quadruple to \n<color=yellow>" + (_coinWins * 2.0f).ToString("#,##0.0") + "</color> credits";

      if (_selectCount == _selectCountMax)
      {
        DoubleText.text = "";
        QuadText.text = "";
      }
    }

    private void ShowResult (bool bSuccess, float fMultiply)
    {
      if (_inShowResult)
        return;

      if (bSuccess)
        BenAudioManager.SoundPlay(3);

      _inShowResult = true;
      InputEnable(false);
      _selectCount++;

      CardCenter.Flip();

      if (bSuccess && (_selectCount < _selectCountMax))
      {
        _coinWins *= fMultiply;
        UpdateText();
        animator.Play("Move");
      } else
      {
        if (bSuccess)
        {
          _coinWins *= fMultiply;
          SceneSlotGame.instance.OnDoubleGameEnd(_coinWins - _coinStart);
        } else
        {
          _coinWins *= 0.0f;
          SceneSlotGame.instance.OnDoubleGameEnd(-_coinStart);
        }

        UpdateText();
        StartCoroutine(HideDelay(1.5f));
      }
    }

    private IEnumerator HideDelay (float fDelay)
    {

      if (fDelay > 0.01f)
        yield return new WaitForSeconds(fDelay);

      animator.Play("Hide");
    }


    public void CardMoveAnimationEnd()
    {
      animator.Play("Normal");

      CardsRight[0].SetSymbolNumber(CardsRight[1].GetIndexof52());
      CardsRight[1].SetSymbolNumber(CardsRight[2].GetIndexof52());
      CardsRight[2].SetSymbolNumber(CardsRight[3].GetIndexof52());
      CardsRight[3].SetSymbolNumber(CardsRight[4].GetIndexof52());
      CardsRight[4].SetSymbolNumber(CardCenter.GetIndexof52());
      CardCenter.SetSymbolNumber(CardDeck.GetIndexof52());
      CardDeck.SetSymbolNumber(_deck[_cardIndexInDeck++]);
      CardDeck.SetSide(false);
      CardCenter.SetSide(false);

      InputEnable(true);
      _inShowResult = false;
    }

    public void OnButtonDouble (int value)
    {
      BenAudioManager.SoundPlay(0);
      bool bSuccess = false;

      if (value == 0)
        bSuccess = UCard.IsRedColor(CardCenter.Symbol);
      else
        bSuccess = !UCard.IsRedColor(CardCenter.Symbol);

      ShowResult(bSuccess, 2.0f);
    }

    public void OnButtonQuad (int value)
    {
      BenAudioManager.SoundPlay(0);
      bool bSuccess = ((CardType)value == CardCenter.Symbol) ? true : false;

      ShowResult(bSuccess, 4.0f);
    }

    public void OnButtonTake()
    {
      BenAudioManager.SoundPlay(0);
      SceneSlotGame.instance.OnDoubleGameEnd(_coinWins - _coinStart);
      animator.Play("Hide");
    }

    public void Hide()
    {
      gameObject.SetActive(false);
      SceneSlotGame.uiState = 0;
    }

    private void _Show (float coin)
    {
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.SetActive(true);
      SceneSlotGame.uiState = 1;

      _coinStart = coin;
      _coinWins = coin;
      _selectCount = 0;
      _inShowResult = false;

      Shuffle();

      foreach (UCard t in CardsRight)
      {
        t.SetSymbolNumber(_deck[_cardIndexInDeck++]);
      }

      CardCenter.SetSymbolNumber(_deck[_cardIndexInDeck++]);
      CardDeck.SetSymbolNumber(_deck[_cardIndexInDeck++]);
      CardDeck.SetSide(false);
      CardCenter.SetSide(false);

      UpdateText();
      animator.Play("Show");

      InputEnable(true);
    }

    public static void Show (float Coin) { _instance._Show(Coin); }
  }

}