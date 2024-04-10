using Commands;
using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.buttons
{
    public class ButtonTableInter : MonoBehaviour, IInterButton
    {
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;

        public void InstantiateChip(CharacterTable characterTable, ButtonTbl buttonData)
        {
            GameObject chipInstance = Instantiate(characterTable.chipPrefab);
            chipInstance.SetActive(false);
            GameFactory.ButtonTableTurn(characterTable, chipInstance, buttonData).Execute();
        }
    }
}
