using System;
using System.Threading.Tasks;
using Commands;
using Managers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.payment_system
{
    public class FortunePayment : ICommand
    {
        private readonly CharacterFactory _characterFactory;
        private readonly CharacterTable _characterTable;
        private readonly RewardFort _rewardFort;
        private readonly int _finalPosition;

        public FortunePayment(CharacterFactory characterFactory, CharacterTable characterTable, RewardFort rewardFort, int finalPosition)
        {
            this._characterFactory = characterFactory;
            this._characterTable = characterTable;
            this._rewardFort = rewardFort;
            this._finalPosition = finalPosition;
        }

        public void Execute()
        {
            int payment = (int)_rewardFort.payment[_finalPosition];
            Debug.Log($"You win: ${payment}");

            CharacterFactory.SaveCash(_characterFactory, _characterTable, payment).Execute();

            _rewardFort.isPlay = false;
            _rewardFort.isPayment = false;

            OpenScene(payment);
        }

        private async void OpenScene(int payment)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            GameManager.Instance.LoadScene("Game");
            _characterTable.characterMoney.currentPayment.Value = payment;
            await Task.Delay(TimeSpan.FromSeconds(2));
            GameManager.Instance.ToggleGame();
        }
    }
}
