using System;
using System.Collections;
using Commands;
using Infrastructure;
using Scripts.ROULETTE.Common.controllers;
using Scripts.ROULETTE.ScriptsGame.Infrastructure.payment;
using Scripts.ROULETTE.ScriptsGame.Infrastructure.save.@interface;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.round;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.round_controller
{    
    public class PlayTur : ICommand
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly CharacterTable _characterTable;
        private readonly GameRoullete _gameRolled;
        private readonly IRound _roundGateway;
        private readonly IPayment _paymentGateway;

        public PlayTur(MonoBehaviour monoBehaviour, CharacterTable characterTable, GameRoullete gameRolled, IRound roundGateway, IPayment paymentGateway)
        {
            this._monoBehaviour = monoBehaviour;
            this._characterTable = characterTable;
            this._gameRolled = gameRolled;
            this._roundGateway = roundGateway;
            this._paymentGateway = paymentGateway;
        }

        public void Execute()
        {
            if(_characterTable.currentTableCount <= 0)
                return;

            _roundGateway.PlayTurn()
                .Do(_ => _monoBehaviour.StartCoroutine(RuleGame(_roundGateway.randomNumber)))
                .Do(_ => _characterTable.lastNumber = _roundGateway.randomNumber)
                .Subscribe();         
        }

        private IEnumerator RuleGame(int num)
        {
            _characterTable.OnRound.OnNext(true);
            _characterTable.currentTableActive.Value = false;
            _gameRolled.OnRotate.OnNext(true);

            yield return new WaitForSeconds(2.0f);
            _gameRolled.currentSpeed = 75f;
            yield return new WaitForSeconds(1.0f);
            _gameRolled.currentSpeed = 145f;
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[9]);
            yield return new WaitForSeconds(0.5f);
            _gameRolled.currentSpeed = 240f;
            yield return new WaitForSeconds(1.2f);
            _gameRolled.currentSpeed = 245f;
            yield return new WaitForSeconds(2.0f);
            _gameRolled.currentSpeed = 265;
            yield return new WaitForSeconds(3.8f);
            _gameRolled.currentSpeed = 245;
            yield return new WaitForSeconds(1.5f);
            _gameRolled.currentSpeed = 240f;
            yield return new WaitForSeconds(1.5f);
            _gameRolled.currentSpeed = 145;
            _gameRolled.OnNumber.OnNext(num);

            yield return new WaitForSeconds(1.8f);
            _gameRolled.currentSpeed = 75f;
   
            yield return new WaitForSeconds(5.0f);
            _gameRolled.currentSpeed = _gameRolled.defaultSpeed;
            _characterTable.OnRound.OnNext(false); 
            
            _paymentGateway.PaymentSystem(_characterTable)
                .Delay(TimeSpan.FromSeconds(3))
                .Do(_ => OnPayment(_paymentGateway.PaymentValue))
                .Do(_ => _characterTable.OnWinButton.OnNext(num))
                .Subscribe();
        }

        private void OnPayment(int value)
        {
            _characterTable.characterMoney.currentPayment.Value = value;
            _characterTable.characterMoney.PaymentSystem(value);
        }
    }
}
