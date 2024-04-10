using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.controllers
{
    public interface IRewardTimer
    {
        public ulong LastChestOpen();
        public bool IsRewardReady(RewardFort rewardFort, float SecondsToWait);
        public string CalculateTimer(float secondsToWait);
    }
}
