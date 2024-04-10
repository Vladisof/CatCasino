using System.Collections;
using Commands;
using Scripts.ROULETTE.Common.controllers;
using Scripts.ROULETTE.ScriptsGame.Components.player.round.table;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.round_controller
{    
    public class ResetTur : ICommand
    {
        private readonly MagnetDestroyDisplay _magnetDestroyDisplay;
        private float _magnetTime;
        private readonly CharacterTable _characterTable;
        private readonly int _delayTime;

        public ResetTur(MagnetDestroyDisplay magnetDestroyDisplay, CharacterTable characterTable, int delayTime)
        {
            this._magnetDestroyDisplay = magnetDestroyDisplay;
            this._characterTable = characterTable;
            this._delayTime = delayTime;
        }

        public void Execute()
        {
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[5]);
            if(_characterTable.currentTableCount <= 0)
                return;
                
            Debug.Log(@"Destroying chips of the table!");
            _magnetDestroyDisplay.StartCoroutine(ResetRoundProcess(_magnetDestroyDisplay.magnetTime));
        }
         
        IEnumerator ResetRoundProcess(float seg)
        {
            _characterTable.currentTableActive.Value = false;
            yield return new WaitForSeconds(_delayTime);
            _magnetDestroyDisplay.magnetDestroyer.SetActive(true);
            yield return new WaitForSeconds(seg);
            _magnetDestroyDisplay.magnetDestroyer.SetActive(false);
            _characterTable.currentTableActive.Value = true;
        }
    }
}
