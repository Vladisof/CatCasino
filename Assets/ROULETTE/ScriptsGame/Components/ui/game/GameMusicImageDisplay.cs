using UnityEngine;
using ViewModel;
using UniRx;
using SlotPerfectKit.Scripts;
using UnityEngine.UI;

namespace Components
{
    public class GameMusicImageDisplay : MonoBehaviour
    {
        public GameSound gameSound;
        public Image imageDisplay;
        public Sprite[] imageOnOff;
        
        void Start()
        {
            gameSound.isMusicOn
                .Subscribe(OnMusic)
                .AddTo(this);
            gameSound.isMusicOn
                .Subscribe(OnSound)
                .AddTo(this);
        }

        private void OnSound(bool isOn)
        {
            bool i = isOn;
            BenSetting.SoundVolume = i ? 100 : 0;
            BenSetting.Save();
        }
        private void OnMusic(bool isOn)
        {
            Sprite i = isOn == true ? imageOnOff[0] : imageOnOff[1];
            imageDisplay.sprite = i;
        }
    }
}
