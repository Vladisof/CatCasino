using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.Components.player.round.table;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.ui.game
{
    public class GameDeleteIn : MonoBehaviour
    {
        public CharacterTable characterTable;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        [FormerlySerializedAs("magnetDestroyerDisplay")]
        public MagnetDestroyDisplay magnetDestroyDisplay;

        public void OnClick()
        {
            GameFactory.ResetTableTurn(magnetDestroyDisplay, characterTable).Execute();
        }
    }
}
