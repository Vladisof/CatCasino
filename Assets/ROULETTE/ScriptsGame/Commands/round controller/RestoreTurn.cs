using Commands;
using Scripts.ROULETTE.Common.controllers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;

namespace Scripts.ROULETTE.ScriptsGame.Commands.round_controller
{    
    public class RestoreTurn : ICommand
    {
        private readonly CharacterTable _characterTable;

        public RestoreTurn(CharacterTable characterTable)
        {
            this._characterTable = characterTable;
        }

        public void Execute()
        {
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[5]);

            if (_characterTable.currentTableCount > 0)
                return;

            RestorePreviousRound();
        }

        private void RestorePreviousRound()
        {
            Table table = new Table()
            {
                TableChips = _characterTable.lastTable
            };

            _characterTable.OnRestoreTable.OnNext(table);
        }
    }
}
