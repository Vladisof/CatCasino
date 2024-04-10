using System;
using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.fortune
{
    public class FortuneInput : MonoBehaviour
    {
        [FormerlySerializedAs("characterCmdFactory")]
        public CharacterFactory characterFactory;
        [FormerlySerializedAs("rewardFortune")]
        public RewardFort rewardFort;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        public CharacterTable characterTable;
        public char separatorAnchor;

        private bool _isExecute = false;
        private int _count = 1;

        private void OnTriggerStay(Collider collider)
        {
            if(!_isExecute && !rewardFort.isPlay && rewardFort.isPayment)
            {
                if(collider.CompareTag("AnchorSelectUI"))
                {
                    _isExecute = true;
                    int pos = Convert.ToInt32(collider.name.Split(separatorAnchor)[1]);
                    FortuneWin(pos);
                }
            }
        }

        private void FortuneWin(int pos)
        {
            GameFactory.FortuneTurn(characterFactory, characterTable, rewardFort, pos).Execute();
        }
    }
}
