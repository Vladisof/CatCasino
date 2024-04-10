using System;
using UniRx;

namespace Scripts.ROULETTE.ScriptsGame.Infrastructure.save.@interface
{
    public interface IRound 
    {
        IObservable<Unit> PlayTurn();
        public int randomNumber {get; set;}    
    }
}
