using System;
using Scripts.ROULETTE.ScriptsGame.Infrastructure.save.@interface;
using UniRx;
using UnityEngine;
using Random=UnityEngine.Random;

namespace Scripts.ROULETTE.ScriptsGame.Infrastructure.save
{
    public class PlayRoundGateway : IRound
    {
        public int randomNumber { get; set; }

        public IObservable<Unit> PlayTurn()
        {
            randomNumber = Random.Range(0, 18);
            return Observable.Return(Unit.Default)
                    .Do(_ => Debug.Log($"Generating number {randomNumber} for the roullete game round!"));
        }
    }
}

