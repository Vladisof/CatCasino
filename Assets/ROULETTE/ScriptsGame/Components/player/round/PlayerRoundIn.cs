using System;
using System.Threading.Tasks;
using Scripts.ROULETTE.ScriptsGame.Components.player.round.table;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.round
{
    public class PlayerRoundIn : MonoBehaviour
    {
        public CharacterTable characterTable;
        private ITableController _tableController;

        private void Awake()
        {
            _tableController = GetComponent<ITableController>();
        }

        private void Start()
        {
            characterTable.OnDestroyChip
                .Subscribe(_tableController.DestroyChipFromTable)
                .AddTo(this);

            characterTable.OnDestroyLastChip
                .Subscribe(_tableController.LastChipDestroy)
                .AddTo(this);
            
            characterTable.OnRound
                .Subscribe(OnRoundFinish)
                .AddTo(this);
            
            characterTable.OnResetGame
                .Subscribe(_tableController.ResetTable)
                .AddTo(this);

            characterTable.OnRestoreTable
                .Subscribe(_tableController.RestoreTable)
                .AddTo(this);
        }

        public async void OnRoundFinish(bool isRound)
        {
            if(isRound)
                return;

            foreach(var item in characterTable.currentTableInGame)
            {
                characterTable.lastTable.Add(item);   
            }

            characterTable.currentNumbers.Add(characterTable.lastNumber);

            await Task.Delay(TimeSpan.FromSeconds(2));
            _tableController.ResetTable(false);
        }
    }
}
