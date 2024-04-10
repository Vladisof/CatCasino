using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.ROULETTE.ScriptsGame.Components.chip;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Infrastructure.payment
{
    public class PaymentGateway : IPayment
    {
        private int _payment;
        private int _number;
        
        public int PaymentValue
        {
            get {return _payment;}
            set {_payment = value;}
        }

        public IObservable<Unit> PaymentSystem(CharacterTable characterTable)
        {
            Debug.Log($"Payment system is being executed in {characterTable.tableName}");
            _payment = 0;
            _number = characterTable.lastNumber;

            var _chipsWinner = characterTable.currentTable.Where(chip => chip.HasNumber(_number));
            var _chipsLosted = characterTable.currentTable.Where(chip => !chip.HasNumber(_number));

            int paymentWin = PaymentHandler.CalculateEarnedPayment(_chipsWinner.ToArray());
            int paymentLost = PaymentHandler.GetPaymentBack(_chipsLosted.ToArray());
            int paymentChipsReturn = PaymentHandler.GetPaymentBack(_chipsWinner.ToArray());

            _payment = paymentWin - (paymentLost);
            _payment = _payment + paymentChipsReturn;

            return Observable.Return(Unit.Default);
        }
    }

    public static class PaymentHandler
    {
        // Calculate and return the all values of payment.
        public static int CalculateEarnedPayment(ChipGame[] chips)
        {
            int earnedPayment = 0;

            IEnumerable<ChipGame> plenos =  chips.Where(chip => chip.chipRuntime.CurrentButton.isPlano);
            IEnumerable<ChipGame> middles =  chips.Where(chip => !chip.chipRuntime.CurrentButton.isPlano);

            int paymentPleno = GetPaymentChips(plenos.ToArray());
            int paymentMiddle = GetPaymentChips(middles.ToArray());

            earnedPayment = paymentPleno + paymentMiddle;
            return earnedPayment;
        }

        // Calculate returned payment of chips
        public static int GetPaymentBack(ChipGame[] chips)
        {
            int total = 0;

            foreach(ChipGame chip in chips)
            {
                int value = chip.chipRuntime.CurrentChipData.chipValue;
                total = total + value;
            }

            return total;
        }
    
        // Calculate the payment of middle or pleno with equation
        public static int GetPaymentChips(ChipGame[] chips)
        {
            int total = 0;
            foreach (ChipGame chip in chips)
            {
                int value = EquationRoullete.EquationPayment(chip.chipRuntime.CurrentButton.buttonValue.Count(), chip.chipRuntime.CurrentChipData.chipValue);
                total = total + value;
            }
            return total;
        }
    }
}
