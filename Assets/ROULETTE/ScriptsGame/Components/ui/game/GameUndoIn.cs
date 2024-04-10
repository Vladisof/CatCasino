using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.ui.game
{
    public class GameUndoIn : MonoBehaviour
    {
        public CharacterTable characterTable;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        public void OnClick() 
        {
            GameFactory.UndoTableTurn(characterTable).Execute();
        }
    }
}
