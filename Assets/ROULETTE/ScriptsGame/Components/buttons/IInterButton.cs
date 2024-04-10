using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;

namespace Scripts.ROULETTE.ScriptsGame.Components.buttons
{
    public interface IInterButton
    {
        void InstantiateChip(CharacterTable characterTable, ButtonTbl buttonData);
    }
}
