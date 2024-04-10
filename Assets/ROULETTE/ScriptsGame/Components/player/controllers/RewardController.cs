using System;
using System.Threading.Tasks;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.controllers
{
    public class RewardController : MonoBehaviour
    {
        [FormerlySerializedAs("rewardFortuneData")]
        public RewardFort rewardFortData;

        private float _secondsToWait;
        private IRewardTimer _rewardTimer;

        void Awake()
        {
            _rewardTimer = GetComponent<RewardHandler>();
        }

        void Start()
        {
            RewardStart();
        }

        async void RewardStart()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            _secondsToWait = PlayerPrefs.GetFloat("SecondsToWaitReward");
            var lastChestOpen = ulong.Parse(PlayerPrefs.GetString("LastRewardOpen"));

            rewardFortData.isPlay = false;

            _rewardTimer.IsRewardReady(rewardFortData, _secondsToWait);
        }

        void Update()
        {
            _rewardTimer.IsRewardReady(rewardFortData, _secondsToWait);

            if (!rewardFortData.isRewardPossible.Value)
            {
                if (_rewardTimer.IsRewardReady(rewardFortData, _secondsToWait))
                {
                    return;
                }
                rewardFortData.rewardTimer.Value = _rewardTimer.CalculateTimer(_secondsToWait);
            }
        }
    }
}
