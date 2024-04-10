using System.Collections;
using Cat;
using Scripts.ROULETTE.ScriptsGame.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cat
{

	public class SceneSlotGame : MonoBehaviour
	{

		public	static SceneSlotGame instance;

		public	static int 		uiState = 0;
		private static BenNumber	Win;

		public 	SlotGame	Slot;

		public	TextMeshProUGUI		textLine;
		public	TextMeshProUGUI		textBet;
		public	TextMeshProUGUI		textTotalBet;
		public	TextMeshProUGUI		textTotalWin;
		public	TextMeshProUGUI		textGold;
		public	TextMeshProUGUI		textInfo;
		
		public 	Button		btnMenu;
		public 	Button		btnLines;
		public Button btnLinesDown;
		public 	Button		btnBet;
		public Button btnBetDown;
		public 	Button		btnSpin;
		public 	Toggle		toggleAutoSpin;

		public 	GameObject	FreeSpinBackground;

		private void Awake ()
		{
			instance=this;
		}

		private void Start ()
		{

			BenSetting.Gold.AddUIText(textGold);
			Win = new BenNumber(BenNumber.IncType.VALUE, "#,##0.00", 0, 10000000000, 0);			
			Win.AddUIText(textTotalWin);
			
			Slot.Gold = (float)BenSetting.Gold.Target();

			Win.ChangeTo(0);

			UpdateUI ();
			
			textInfo.text = "";
		}

		private void Update ()
		{
			
			if ((uiState==0) && Input.GetKeyDown(KeyCode.Escape))
			{ 
				UIMessage.Show("Quit", "Do you want to quit this program ?", MsgType.OkCancel, MessageQuitResult);
			}
			
			Win.Update();
			
			if((toggleAutoSpin.isOn || Slot.gameResult.InFreeSpin()) && Slot.SpinEnable()) {
				OnButtonSpin();
			}
		}
		
		public void MessageQuitResult(int value)
		{
			if(value == 0) {
				Application.Quit ();
			}
		}
		
		public void OnButtonShop()
		{
			BenAudioManager.SoundPlay(0);
			UIShop.Show();
		}
		
		public void OnButtonExit()
		{
			BenAudioManager.SoundPlay(0);
			SceneManager.LoadScene("Menu");
		}
		
		public void OnButtonPayTable()
		{
			BenAudioManager.SoundPlay(0);
			UPayTable.Show(Slot);
		}

		public void OnButtonMaxLine() 
		{
			BenAudioManager.SoundPlay(0);
			Slot.LineSet(Slot.Lines.Count);
			UpdateUI();
		}
		
		public void OnButtonLines()
		{
			BenAudioManager.SoundPlay(0);
			Slot.LineSet(Slot.Line+1);
			UpdateUI();
		}
		
		public void OnButtonLinesDown()
		{
			BenAudioManager.SoundPlay(0);
			Slot.LineSet(Slot.Line-1);
			UpdateUI();
		}

		public void OnButtonBet()
		{
			BenAudioManager.SoundPlay(0);
			Slot.BetSet(Slot.Bet+1);
			UpdateUI();
		}
		
		public void OnButtonBetDown() 
		{
			BenAudioManager.SoundPlay(0);
			Slot.BetSet(Slot.Bet-1);
			UpdateUI();
		}
		
		public void OnButtonDouble()
		{
			BenAudioManager.SoundPlay(0);
			UDoubleGame.Show(Slot.gameResult.GameWin);
		}
		
		public void OnButtonSpin() 
		{
			BenAudioManager.SoundPlay(0);

			SlotReturnCode code = Slot.Spin();

			if(SlotReturnCode.Success == code) {
				ButtonEnable(false);
				UpdateUI();
				BenSetting.Gold.ChangeTo(Slot.Gold);
				BenSetting.Save();

				if(Slot.gameResult.InFreeSpin()) 	textInfo.text = "Free Spin "+Slot.gameResult.FreeSpinCount.ToString ()+" of "+Slot.gameResult.FreeSpinTotalCount.ToString ();
				else
					textInfo.text = "";
			}
			else
			{

				if(SlotReturnCode.InSpin == code) 		{ UIMessage.Show("Error", "Slot is in spin now.", MsgType.Ok, null); }
				else if(SlotReturnCode.NoGold == code) 	{ UIMessage.Show("Error", "Not enough gold.", MsgType.Ok, null); }
			}
		}
		
		public void AutoSpinToggled(bool value) {
			BenAudioManager.SoundPlay(0);
		}
		
		public void UpdateUI() {
			textLine.text = Slot.Line.ToString ();
			textBet.text = Slot.RealBet.ToString ("#0.00");
			textTotalBet.text = Slot.TotalBet.ToString ("#0.00");
			Win.ChangeTo(Slot.gameResult.GameWin);
		}

		public void ButtonEnable(bool bEnable) {
			btnMenu.interactable = bEnable;
			btnLines.interactable = bEnable;
			btnLinesDown.interactable = bEnable;
			btnBet.interactable = bEnable;
			btnBetDown.interactable = bEnable;
			btnSpin.interactable = bEnable;
		}
		
		public void OnDoubleGameEnd(float delta)
		{
			Slot.Gold += delta;
			BenSetting.Gold.ChangeTo(Slot.Gold);
			BenSetting.Save();
			Slot.gameResult.GameWin += delta;
			Win.ChangeTo(Slot.gameResult.GameWin);
		}

		public void OnReelStop(int value) 
		{
			BenAudioManager.SoundPlay(2);
		}

		public void OnSpinEnd()
		{

			if(Slot.gameResult.Wins.Count != 0)
				textInfo.text = "Win "+Slot.gameResult.Wins.Count.ToString ()+" Lines ";

			UpdateUI();
			BenSetting.Gold.ChangeTo(Slot.Gold);
			BenSetting.Save();
		}

		public void OnSplashShow(int value) {
			BenAudioManager.SoundPlay(3);
			UISplash.Show (value);
			
			if(value == (int)SplashType.FreeSpin) {
				FreeSpinBackground.SetActive(true);
			}
			else if(value == (int)SplashType.FreeSpinEnd) {
				FreeSpinBackground.SetActive(false);
			}
		}

		public void OnSplashHide(int value) {
			StartCoroutine(SlotSplashHide(value, 0.5f));
		}
		
		public void OnSplashEnd() {
			if(Slot.gameResult.InFreeSpin()) {
				textInfo.text = "Free Spin "+Slot.gameResult.FreeSpinCount.ToString ()+" of "+Slot.gameResult.FreeSpinTotalCount.ToString ();
			}
			else {
				textInfo.text = "";
				ButtonEnable(true);
			}
		}

		private IEnumerator SlotSplashHide(int value, float fDelay) {
			
			if(fDelay > 0.01f)
				yield return new WaitForSeconds(fDelay);
			
			Slot.SplashCount[value] = 0;
			Slot.SplashActive++;
			Slot.InSplashShow = false;
		}

	}

}
