using System;
using System.Collections;
using System.IO;
using Infrastructure;
using Managers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Infrastructure.save
{
    public class SaveRoundGateway : ISaveRound
    {
        protected private static readonly string FILE_NAME = "player";
        public Rnd RndData {get; set;}

        public IObservable<Unit> RoundSequentialSave(Rnd rndData)
        {
            return Observable.FromCoroutine<Unit>(observer => SavePlayer(observer, rndData));
        }

        public IObservable<Unit> RoundSequentialLoad()
        {
            return Observable.FromCoroutine<Unit>(observer => LoadPlayer(observer));
        }

        IEnumerator SavePlayer(IObserver<Unit> observer, Rnd rndData)
        {
            string path = GameManager.Instance.UrlDataPath + FILE_NAME;
            string json = JsonUtility.ToJson(rndData);

            File.WriteAllText(path, json);
            Debug.Log($"Saved data JSON with the table {rndData.idPlayer} with {json}");

            yield return new WaitUntil(() => File.Exists(path));
            
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }

        IEnumerator LoadPlayer(IObserver<Unit> observer) 
        {
            string path = GameManager.Instance.UrlDataPath + FILE_NAME;
            string json = File.ReadAllText(path);

            yield return new WaitUntil(() => json != null);
            
            RndData = JsonUtility.FromJson<Rnd>(json);
            Debug.Log($"Loaded data JSON with the table {RndData.idPlayer} with {json}");

            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
    }
}

