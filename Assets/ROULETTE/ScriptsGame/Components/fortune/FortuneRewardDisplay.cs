using System.Collections;
using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.fortune
{
    public class FortuneRewardDisplay : MonoBehaviour
    {
        [FormerlySerializedAs("rewardFortune")]
        public RewardFort rewardFort;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        
        private int _randValue;
        private float _timeInterval;
        private float _angleFinished;
        private float _totalAngle;

        private void Start()
        {
            rewardFort.isPayment = false;
            rewardFort.isPlay = false;

            _totalAngle = 360 / rewardFort.sectionCount;

            rewardFort.OnFortune
                .Subscribe(OnFortune)
                .AddTo(this);
        }
            
        public void OnFortune(bool isFortune)
        {
            if(isFortune)
                StartCoroutine(Spin());
        }

        private IEnumerator Spin()
        {
            rewardFort.isPlay = true;
            rewardFort.isPayment = true;

            _randValue = Random.Range(100, 200);
            _timeInterval = 0.0001f * Time.deltaTime * 2;

            for (int i = 0; i < _randValue; i++)
            {
                transform.Rotate(0, 0, (_totalAngle / 2));
                if (i > Mathf.RoundToInt(_randValue * 0.2f))
                    _timeInterval = 0.5f * Time.deltaTime;
                if (i > Mathf.RoundToInt(_randValue * 0.5f))
                    _timeInterval = 1f * Time.deltaTime;
                if (i > Mathf.RoundToInt(_randValue * 0.7f))
                    _timeInterval = 1.5f * Time.deltaTime;
                if (i > Mathf.RoundToInt(_randValue * 0.8f))
                    _timeInterval = 2f * Time.deltaTime;
                yield return new WaitForSeconds(_timeInterval);
            }

            if (Mathf.RoundToInt(transform.eulerAngles.z) % _totalAngle != 0)
            {
                transform.Rotate(0, 0, 22.5f);
            }
            
            rewardFort.isPlay = false;
        }
    }
}
