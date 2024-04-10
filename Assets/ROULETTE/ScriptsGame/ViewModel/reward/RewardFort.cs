using System;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.ViewModel.reward
{
    [CreateAssetMenu(fileName = "New Reward Fortune", menuName = "Scriptable/Reward Fortune")]
    public class RewardFort : ScriptableObject
    {
        public float secondsToWaitReward;
        public string rewardLabel;
        public float totalAngle;
        public int sectionCount;
        public int[] payment;
        public bool isPlay;
        public bool isPayment;

        public StringReactiveProperty rewardTimer;
        public BoolReactiveProperty isRewardPossible;

        public readonly ISubject<bool> OnFortune = new Subject<bool>();

        public void OpenReward()
        {
            PlayerPrefs.SetString("LastRewardOpen", DateTime.Now.Ticks.ToString());
        }
    }
}
