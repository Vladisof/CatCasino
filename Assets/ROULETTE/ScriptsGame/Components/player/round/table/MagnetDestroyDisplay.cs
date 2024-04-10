using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.round.table
{
    public class MagnetDestroyDisplay : MonoBehaviour
    {
        public CharacterTable characterTable;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        public GameObject magnetDestroyer;
        public float magnetTime;

        private void Start()
        {
            characterTable.OnRound
                .Subscribe(MagnetDestroyer)
                .AddTo(this);
        }

        public void MagnetDestroyer(bool isRound)
        {
            if(isRound)
                return;

            GameFactory.ResetTableTurn(this, characterTable, 10).Execute();  
        }
    }
}
