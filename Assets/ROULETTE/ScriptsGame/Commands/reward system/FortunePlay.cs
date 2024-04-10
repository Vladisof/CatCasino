using Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;

namespace Scripts.ROULETTE.ScriptsGame.Commands.reward_system
{
    public class FortunePlay : ICommand
    {
        private readonly RewardFort _rewardFort;

        public FortunePlay(RewardFort rewardFort)
        {
            this._rewardFort = rewardFort;
        }

        public void Execute()
        {
            _rewardFort.OnFortune.OnNext(true);
            _rewardFort.OpenReward();
        }
    }
}
