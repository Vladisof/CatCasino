using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;

namespace Scripts.ROULETTE.ScriptsGame.Components.buttons
{
    public interface ILongPress
    {
        void SetPointerDown(bool value);
        void LongPressCheck(CharacterTable characterTable, ButtonTbl buttonData);
        void ResetPointer();
        void LongPress(CharacterTable characterTable, ButtonTbl buttonData, bool currentStatus);
    }
}
