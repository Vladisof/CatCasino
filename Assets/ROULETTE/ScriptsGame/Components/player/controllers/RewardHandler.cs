using System;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.controllers
{
    public class RewardHandler : MonoBehaviour, IRewardTimer
    {
        public ulong LastChestOpen()
        {
            return 0;
        }
        public bool IsRewardReady(RewardFort rewardFort, float SecondsToWait)
        {
            var lastChestOpen = LastChestOpen();

            ulong diff = ((ulong)DateTime.Now.Ticks - lastChestOpen);
            ulong m = diff / TimeSpan.TicksPerSecond;

            float secondsLeft = (float)(SecondsToWait - m);

            if (secondsLeft < 0)
            {
                rewardFort.rewardTimer.Value = rewardFort.rewardLabel;
                rewardFort.isRewardPossible.Value = true;
                return true;
            } else
            {
                rewardFort.isRewardPossible.Value = false;
                return false;
            }
        }
        public string CalculateTimer(float secondsToWait)
        {
            ulong diff = ((ulong)DateTime.Now.Ticks - LastChestOpen());
            ulong m = diff / TimeSpan.TicksPerSecond;
            float secondsLeft = (float)(secondsToWait - m);
            string t = "";
            
            t += ((int)secondsLeft / 3600).ToString() + "h:";
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;
            t += ((int)secondsLeft / 60).ToString("00") + "m:";
            t += (secondsLeft % 60).ToString("00") + "s";

            return t;
        }
    }
}
