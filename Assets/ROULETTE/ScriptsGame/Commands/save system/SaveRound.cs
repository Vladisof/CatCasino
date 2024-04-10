using Commands;
using Infrastructure;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.save_system
{    
    public class SaveRound : ICommand
    {
        private readonly CharacterTable _characterTable;
        private readonly ISaveRound _saveRoundGateway;

        public SaveRound(CharacterTable characterTable, ISaveRound saveRoundGateway)
        {
            this._characterTable = characterTable;
            this._saveRoundGateway = saveRoundGateway;
        }

        public void Execute()
        {
            Rnd rndData = GetRoundData();

            _saveRoundGateway.RoundSequentialSave(rndData)
                .Subscribe();
        }

        private Rnd GetRoundData()
        {
            Table table = new Table()
            {
                TableChips = _characterTable.currentTableInGame
            };

            string json = JsonUtility.ToJson(table);

            Rnd rndData = new Rnd()
            {
                idPlayer = _characterTable.tableName,
                playerMoney = _characterTable.characterMoney.characterMoney.Value + _characterTable.characterMoney.characterBet.Value,
                playerTable = json
            };
            return rndData;
        }
    }
}
