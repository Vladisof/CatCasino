using Commands;
using Scripts.ROULETTE.Common.controllers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;

namespace Scripts.ROULETTE.ScriptsGame.Commands.round_controller
{    
    public class UndoTur : ICommand
    {
        private readonly CharacterTable _characterTable;

        public UndoTur(CharacterTable characterTable)
        {
            this._characterTable = characterTable;
        }

        public void Execute()
        {
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[4]);
            _characterTable.OnDestroyLastChip.OnNext(true);
        }
    }
}
