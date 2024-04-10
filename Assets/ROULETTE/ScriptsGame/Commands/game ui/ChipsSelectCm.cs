using Commands;
using Scripts.ROULETTE.Common.controllers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;

namespace Scripts.ROULETTE.ScriptsGame.Commands.game_ui
{    
    public class ChipsSelectCm : ICommand
    {
        private readonly CharacterTable _characterTable;
        private readonly Chip _chipData;

        public ChipsSelectCm(CharacterTable characterTable, Chip chipData)
        {
            this._characterTable = characterTable;
            this._chipData = chipData;
        }

        public void Execute()
        {
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[5]);
            _characterTable.currentChipSelected = _chipData;
        }
    }
}
