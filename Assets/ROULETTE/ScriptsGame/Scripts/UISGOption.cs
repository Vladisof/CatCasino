using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SlotPerfectKit.Scripts;

namespace BE {
	
	public class UISGOption : MonoBehaviour {
		
		private static UISGOption instance;
		
		public 	Toggle 		uiMusicToggle;
		public 	Toggle 		uiSoundToggle;
		public	Image 		Dialog;

		void Awake () {
			instance=this;
			gameObject.SetActive(false);
		}
		
		void Start () {
		}
		
		void Update () {
			if (Input.GetKeyDown(KeyCode.Escape)) { 
				Hide();
			}
		}
		
		void OnEnable(){
			uiMusicToggle.isOn = (BenSetting.MusicVolume != 0) ? false : true;
			uiSoundToggle.isOn = (BenSetting.SoundVolume != 0) ? false : true;
		}
		
		public void MusicToggled(bool value) {
			//Debug.Log ("MusicToggled "+value);
			BeAudioManager.SoundPlay(0);
			BenSetting.MusicVolume = value ? 0 : 100;
			BenSetting.Save();
			
			if(value) 	{
				BeAudioManager.MusicStop();
			}
			else {
				if(!BeAudioManager.MusicIsPlaying()) 
					BeAudioManager.MusicPlay();
			}
		}
		
		public void SoundToggled(bool value) {
			BeAudioManager.SoundPlay(0);
			BenSetting.SoundVolume = value ? 0 : 100;
			BenSetting.Save();
		}
		
		public void OnButtonMenu() {
			BeAudioManager.SoundPlay(0);
			Application.LoadLevel ("Game_Lobby");
		}
		
		public void OnButtonContinue() {
			//Debug.Log("UISGOption::OnButtonContinue");
			BeAudioManager.SoundPlay(0);
			//Hide();
		}
		
		public void Hide() {
			//Debug.Log("UISGOption::Hide");
			gameObject.SetActive(false);
			SceneSlotGame.uiState = 0;
		}

		void _Show () {
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.SetActive(true);
			SceneSlotGame.uiState = 1;
			StartCoroutine(BEUtil.instance.ImageScale(Dialog, Dialog.color, 1.0f, 1.1f, 1.0f, 0.1f, 0.0f));
		}
		
		public static void Show() { instance._Show(); }
		
	}
	
}