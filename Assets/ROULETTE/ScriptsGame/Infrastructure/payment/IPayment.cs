using System;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;

namespace Scripts.ROULETTE.ScriptsGame.Infrastructure.payment
{
    public interface IPayment 
    {
        public IObservable<Unit> PaymentSystem(CharacterTable characterTable);
        public int PaymentValue
        {
            get;
            set;
        }
    }
}