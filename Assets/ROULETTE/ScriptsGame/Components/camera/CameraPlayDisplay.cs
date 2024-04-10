using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.camera
{
    public class CameraPlayDisplay : MonoBehaviour
    {
        public CharacterTable characterTable;
        public Animator mainCameraAnimator;

        void Start()
        {
            characterTable.OnRound
                .Subscribe(AnimateMainCamera)
                .AddTo(this);
        }

        public void AnimateMainCamera(bool isRound)
        {
            mainCameraAnimator.SetBool("Play", isRound);
        }
    }
}
