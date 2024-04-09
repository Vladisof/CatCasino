using System.Collections;
using BE;
using SlotPerfectKit.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BE
{

	public class SceneSlotGame : MonoBehaviour
	{

		public	static SceneSlotGame instance;

		public	static int 		uiState = 0; 	// popup window shows or not
		public  static BeNumber	Win;			// total win number

		public 	SlotGame	Slot;			// slot game class

		public	TextMeshProUGUI		textLine;		// user selected line info text
		public	TextMeshProUGUI		textBet;		// current betting info text
		public	TextMeshProUGUI		textTotalBet;	// total betting info
		public	TextMeshProUGUI		textTotalWin;	// total win info
		public	TextMeshProUGUI		textGold;		// user gold info
		public	TextMeshProUGUI		textInfo;		// other info text
		
		public 	Button		btnMenu;		// call setting dialog
		public 	Button		btnLines;		// change line selected
		public Button btnLinesDown; // decrease line selected
		public 	Button		btnBet;			// change bet 
		public Button btnBetDown; // decrease bet
		public 	Button		btnSpin;		// start spin
		public 	Toggle		toggleAutoSpin;	// auto spin toggle button

		public 	GameObject	FreeSpinBackground;	// background og game scene

		void Awake () {
			instance=this;
		}

		void Start () {

			// set range of numbers and type
			BenSetting.Gold.AddUIText(textGold);
			Win = new BeNumber(BeNumber.IncType.VALUE, "#,##0.00", 0, 10000000000, 0);			
			Win.AddUIText(textTotalWin);

			//set saved user gold count to slotgame
			Slot.Gold = (float)BenSetting.Gold.Target();
			//set win value to zero
			Win.ChangeTo(0);

			UpdateUI ();

			//double button show only user win
			textInfo.text = "";
		}
		void Update () {
		
			// if user press 'escape' key, show quit message window
			if ((uiState==0) && Input.GetKeyDown(KeyCode.Escape)) { 
				UISGMessage.Show("Quit", "Do you want to quit this program ?", MsgType.OkCancel, MessageQuitResult);
			}
			
			Win.Update();

			// if auto spin is on or user has free spin to run, then start spin
			if((toggleAutoSpin.isOn || Slot.gameResult.InFreeSpin()) && Slot.SpinEnable()) {
				//Debug.Log ("Update Spin");
				OnButtonSpin();
			}
		}

		// when user pressed 'ok' button on quit message.
		public void MessageQuitResult(int value) {
			//Debug.Log ("MessageQuitResult value:"+value.ToString ());
			if(value == 0) {
				Application.Quit ();
			}
		}

		//user clicked shop button, then show shop
		public void OnButtonShop() {
			BeAudioManager.SoundPlay(0);
			UISGShop.Show();
		}

		// user clicked option button, then show option
		public void OnButtonExit() {
			BeAudioManager.SoundPlay(0);
			SceneManager.LoadScene("Menu");
		}

		// user clicked paytable button, then show paytable
		public void OnButtonPayTable() {
			BeAudioManager.SoundPlay(0);
			UISGPayTable.Show(Slot);
		}

		// user clicked Max line button, then ser slot's line count to max
		public void OnButtonMaxLine() {
			BeAudioManager.SoundPlay(0);
			Slot.LineSet(Slot.Lines.Count);
			UpdateUI();
		}

		//user clicked line button, increase line count 
		public void OnButtonLines() {
			BeAudioManager.SoundPlay(0);
			Slot.LineSet(Slot.Line+1);
			UpdateUI();
		}
		
		public void OnButtonLinesDown() {
			BeAudioManager.SoundPlay(0);
			Slot.LineSet(Slot.Line-1);
			UpdateUI();
		}

		// user clicked bet button, then increase bet number
		public void OnButtonBet() {
			BeAudioManager.SoundPlay(0);
			Slot.BetSet(Slot.Bet+1);
			UpdateUI();
		}
		
		public void OnButtonBetDown() {
			BeAudioManager.SoundPlay(0);
			Slot.BetSet(Slot.Bet-1);
			UpdateUI();
		}

		//user clicked double button, then start double game
		public void OnButtonDouble() {
			BeAudioManager.SoundPlay(0);
			UIDoubleGame.Show(Slot.gameResult.GameWin);
		}

		// user clicked spin button
		public void OnButtonSpin() {
			//Debug.Log ("OnButtonSpin");

			BeAudioManager.SoundPlay(0);

			// start spin
			SlotReturnCode code = Slot.Spin();
			// if spin succeed
			if(SlotReturnCode.Success == code) {
				// disabled inputs
				ButtonEnable(false);
				UpdateUI();
				// apply decreased user gold
				BenSetting.Gold.ChangeTo(Slot.Gold);
				BenSetting.Save();

				// set info text
				if(Slot.gameResult.InFreeSpin()) 	textInfo.text = "Free Spin "+Slot.gameResult.FreeSpinCount.ToString ()+" of "+Slot.gameResult.FreeSpinTotalCount.ToString ();
				else 								textInfo.text = "";
			}
			else {
				// if spin fails
				// show Error Message
				if(SlotReturnCode.InSpin == code) 		{ UISGMessage.Show("Error", "Slot is in spin now.", MsgType.Ok, null); }
				else if(SlotReturnCode.NoGold == code) 	{ UISGMessage.Show("Error", "Not enough gold.", MsgType.Ok, null); }
				else {}
			}
		}

		// if user clicked auto spin
		public void AutoSpinToggled(bool value) {
			BeAudioManager.SoundPlay(0);
		}

		// update ui text & win number
		public void UpdateUI() {
			textLine.text = Slot.Line.ToString ();
			textBet.text = Slot.RealBet.ToString ("#0.00");
			textTotalBet.text = Slot.TotalBet.ToString ("#0.00");
			Win.ChangeTo(Slot.gameResult.GameWin);
		}

		// enable or disable button inputs
		public void ButtonEnable(bool bEnable) {
			btnMenu.interactable = bEnable;
			btnLines.interactable = bEnable;
			btnLinesDown.interactable = bEnable;
			btnBet.interactable = bEnable;
			btnBetDown.interactable = bEnable;
			btnSpin.interactable = bEnable;
		}

		//------------------------------------------
		//callback functions
		// when double game ends
		public void OnDoubleGameEnd(float delta) {
			//Debug.Log("OnDoubleGameEnd:"+delta.ToString ());

			//change user gold by delta (change gold value)
			Slot.Gold += delta;
			BenSetting.Gold.ChangeTo(Slot.Gold);
			BenSetting.Save();
			Slot.gameResult.GameWin += delta;
			Win.ChangeTo(Slot.gameResult.GameWin);
		}

		// when reel stoped
		public void OnReelStop(int value) {
			//Debug.Log ("OnReelStop:"+value.ToString());
			BeAudioManager.SoundPlay(2);
		}

		// when spin completed
		public void OnSpinEnd() {
			//Debug.Log("OnSpinEnd");

			// if user has win
			if(Slot.gameResult.Wins.Count != 0)
				textInfo.text = "Win "+Slot.gameResult.Wins.Count.ToString ()+" Lines ";

			UpdateUI();
			// increase user gold
			BenSetting.Gold.ChangeTo(Slot.Gold);
			BenSetting.Save();
		}

		//when splash window shows
		public void OnSplashShow(int value) {
			//Debug.Log ("OnSplashShow:"+value.ToString());
			BeAudioManager.SoundPlay(3);
			UISGSplash.Show (value);

			// change background image if free spin
			if(value == (int)SplashType.FreeSpin) {
				FreeSpinBackground.SetActive(true);
			}
			else if(value == (int)SplashType.FreeSpinEnd) {
				FreeSpinBackground.SetActive(false);
			}
			else {}
		}

		// when splash hide
		public void OnSplashHide(int value) {
			//Debug.Log ("OnSplashHide:"+value.ToString());
			StartCoroutine(SlotSplashHide(value, 0.5f));
		}

		// when all splash works end
		public void OnSplashEnd() {
			//Debug.Log ("OnSplashEnd");
			if(Slot.gameResult.InFreeSpin()) {
				textInfo.text = "Free Spin "+Slot.gameResult.FreeSpinCount.ToString ()+" of "+Slot.gameResult.FreeSpinTotalCount.ToString ();
			}
			else {
				textInfo.text = "";
				ButtonEnable(true);
			}
		}

		// splash idx change
		public IEnumerator SlotSplashHide(int value, float fDelay) {
			
			if(fDelay > 0.01f)
				yield return new WaitForSeconds(fDelay);
			
			Slot.SplashCount[value] = 0;
			Slot.SplashActive++;
			Slot.InSplashShow = false;
		}

	}

}
