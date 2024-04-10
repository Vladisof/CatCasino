using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.round;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.ui.game
{
    public class GamePlayIn : MonoBehaviour
    {
        [FormerlySerializedAs("chartacterTable")]
        public CharacterTable characterTable;
        [FormerlySerializedAs("gameRoulette")]
        public GameRoullete gameRoullete;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        
        public void OnClick()
        {
            GameFactory.PlayTurn(characterTable, gameRoullete).Execute();
        }
    }
}
