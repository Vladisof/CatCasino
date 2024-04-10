using System;
using System.Collections;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using UniRx;
using UnityEngine;

namespace Infrastructure
{
    public interface ISaveRound 
    {
        IObservable<Unit> RoundSequentialSave(Rnd rndData);
        IObservable<Unit> RoundSequentialLoad();
        public Rnd RndData {get; set;}    
    }
}
