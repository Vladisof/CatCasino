using Commands;
using Infrastructure;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.save_system
{    
    public class LoadRound : ICommand
    {
        private readonly CharacterTable _characterTable;
        private readonly ISaveRound _loadRoundGateway;

        public LoadRound(CharacterTable characterTable, ISaveRound loadRoundGateway)
        {
            this._characterTable = characterTable;
            this._loadRoundGateway = loadRoundGateway;
        }

        public void Execute()
        {
            _loadRoundGateway.RoundSequentialLoad()
                .Do(_ => _characterTable.characterMoney.characterMoney.Value = _loadRoundGateway.RndData.playerMoney)
                .Do(_ => LoadTable(_loadRoundGateway.RndData))
                .Subscribe();
        }

        void LoadTable(Rnd rndData)
        {
            Table table = JsonUtility.FromJson<Table>(rndData.playerTable);
            Debug.Log($"Loading current player table {rndData.playerTable}");
            _characterTable.OnRestoreTable.OnNext(table);
        }
    }
}
