using Scripts.ROULETTE.ScriptsGame.Scripts;
using Scripts.ROULETTE.ScriptsGame.ViewModel.audio;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.ROULETTE.ScriptsGame.Components.ui.game
{
    public class GameMusicImageDis : MonoBehaviour
    {
        public GameSound gameSound;
        public Image imageDisplay;
        public Sprite[] imageOnOff;
        public GameObject gameSound1;

        private void Start()
        {
            gameSound.isMusicOn
                .Subscribe(OnMusic)
                .AddTo(this);
            if (gameSound1 != null)
            {
                gameSound.isMusicOn
                    .Subscribe(OnSound)
                    .AddTo(this);
            }

        }

        private static void OnSound(bool isOn)
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
