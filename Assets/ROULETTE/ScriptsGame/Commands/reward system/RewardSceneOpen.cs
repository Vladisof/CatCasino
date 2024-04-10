using Commands;
using Managers;
using Scripts.ROULETTE.Common.controllers;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.reward_system
{
    public class RewardSceneOpen : ICommand
    {
        private readonly string _rewardScene;

        public RewardSceneOpen(string rewardScene)
        {
            this._rewardScene = rewardScene;
        }

        public void Execute()
        {
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[5]);
            GameManager.Instance.ToggleRewardSystem();
            GameManager.Instance.LoadScene(_rewardScene);
        }
    }
}
