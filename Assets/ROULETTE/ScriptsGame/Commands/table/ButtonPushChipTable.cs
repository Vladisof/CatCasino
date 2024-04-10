using Commands;
using Scripts.ROULETTE.Common.controllers;
using Scripts.ROULETTE.ScriptsGame.Components.chip;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.table
{
    public class ButtonPushChipTable : ICommand
    {
        private readonly TableManage _tableController;
        private readonly GameObject _chipInstance;
        private readonly ButtonTbl _buttonData;
        private readonly Chip _chipData;

        public ButtonPushChipTable(TableManage tableController, GameObject chipInstance, ButtonTbl buttonData)
        {
            this._tableController = tableController;
            this._chipInstance = chipInstance;
            this._buttonData = buttonData;
            this._chipData = tableController.tables.characterTable.currentChipSelected;
        }

        public void Execute()
        {
            ChipGame chipGame = _chipInstance.GetComponent<ChipGame>();
            
            if(_chipData.chipKay == KeyFiche.ChipAll)
            {
                _chipData.chipValue = _tableController.tables.characterTable.characterMoney.characterMoney.Value;
            }
            
            if(_tableController.tables.characterTable.characterMoney.CheckBetValue(_chipData.chipValue))
            {
                PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[3]);

                bool _hasFichasOnTop = _buttonData.currentChipsOnTop > 0;
                _tableController.tableInteract.PushChipInButton(_tableController.tables.characterTable, _buttonData, chipGame, _chipData, _chipInstance, _buttonData.currentSpritePivot, _buttonData.GetOffset(), _hasFichasOnTop);
                
                _buttonData.currentChipsOnTop++;
            }
            else
            {
                chipGame.DestroyChip();
            }
        }    

    }
}
