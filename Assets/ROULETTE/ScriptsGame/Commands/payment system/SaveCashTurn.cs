using Commands;
using Infrastructure;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.payment_system
{    
    public class SaveCashTurn : ICommand
    {
        private readonly CharacterFactory _characterFactory;
        private readonly CharacterTable _characterTable;
        private readonly int _payment;
        private readonly ISaveRound _saveRoundGateway;

        public SaveCashTurn(CharacterFactory characterFactory, CharacterTable characterTable, int payment, ISaveRound saveRoundGateway)
        {
            this._characterFactory = characterFactory;
            this._characterTable = characterTable;
            this._payment = payment;
            this._saveRoundGateway = saveRoundGateway;
        }

        public void Execute()
        {
            _saveRoundGateway.RoundSequentialLoad()
                .Do(_ => _characterTable.characterMoney.characterMoney.Value = _saveRoundGateway.RndData.playerMoney)
                .Do(_ => UpdateTable(_payment))
                .Do(_ => CharacterFactory.SavePlayer(_characterTable).Execute())
                .Do(_ => _characterTable.currentTableInGame.Clear())
                .Subscribe();
        }

        private void UpdateTable(int payment)
        {
            Table tableLoaded = JsonUtility.FromJson<Table>(_saveRoundGateway.RndData.playerTable);
            _characterTable.currentTableInGame = tableLoaded.TableChips;
            _characterTable.characterMoney.PaymentSystem(payment);
        }
    }
}
