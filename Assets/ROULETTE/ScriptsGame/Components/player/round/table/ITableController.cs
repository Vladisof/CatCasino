using Scripts.ROULETTE.ScriptsGame.Components.chip;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.round.table
{
    public interface ITableController
    {
        void DestroyChipFromTable(ChipGame fiche);
        void LastChipDestroy(bool value);
        void RestoreTable(Table table);
        void ResetTable(bool destroyChips);
    }
}
