using System;
using System.Threading.Tasks;
using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.fortune
{
    public class FortunePlayInput : MonoBehaviour
    {
        [FormerlySerializedAs("rewardFortune")]
        public RewardFort rewardFort;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;

        void Start()
        {
            OnClick();
        }
        
        public async void OnClick()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            GameFactory.FortuneRewardTurn(rewardFort).Execute();
        }
    }
}
