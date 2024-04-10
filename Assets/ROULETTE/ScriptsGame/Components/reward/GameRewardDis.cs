using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scripts.ROULETTE.ScriptsGame.Components.reward
{
    public class GameRewardDis : MonoBehaviour
    {
        [FormerlySerializedAs("rewardFortune")]
        public RewardFort rewardFort;
        public Text timerLabel;
        public Button rewardButton;
        
        void Start()
        {
            rewardFort.isRewardPossible
                .Subscribe(OnReward)
                .AddTo(this);

            rewardFort.rewardTimer
                .Subscribe(OnTimer)
                .AddTo(this);
        }

        private void OnTimer(string time)
        {
            timerLabel.text = time;
        }

        private void OnReward(bool isReward)
        {
            rewardButton.interactable = isReward;
        }
    }
}
