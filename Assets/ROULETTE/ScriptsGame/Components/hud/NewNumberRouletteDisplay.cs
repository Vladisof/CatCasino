using System.Collections;
using Scripts.ROULETTE.Common.utils;
using Scripts.ROULETTE.ScriptsGame.ViewModel.round;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.hud
{
    public class NewNumberRouletteDisplay : MonoBehaviour
    {
        [FormerlySerializedAs("gameRoulette")]
        public GameRoullete gameRoullete;
        public GameObject numberContainer;
        public GameObject goContainer;
        public GameObject anchorPos;
        public SpriteRenderer anchorSprite;
        [FormerlySerializedAs("_secondToDisplay")]
        public int secondToDisplay;
        private const int DELAY = 3;

        void Start()
        {
            gameRoullete.OnNumber
                .Subscribe(FxNewNumber)
                .AddTo(this);
        }

        private void FxNewNumber(int num)
        {
            Debug.Log("FxNewNumber");
            StartCoroutine(FxNumber(secondToDisplay, num));
        }

        IEnumerator FxNumber(int seg, int num)
        {
            yield return new WaitForSeconds(DELAY);
            goContainer.SetActive(true);
            GameObject goNum = Instantiate(numberContainer.transform.GetChild(num).gameObject);
            if(num == 0)
            {
                goNum.transform.GetChild(0).transform.localPosition = new Vector3(-0.08f, 0.19f, 0);
            }
            goNum.transform.localPosition = anchorPos.transform.position;
            goNum.GetComponent<LeanTweenScale>()._scaleXYZ = new Vector3(0.87f,0.87f,0.87f);
            goNum.transform.SetParent(anchorPos.transform);
            goNum.SetActive(true);
            yield return new WaitForSeconds(seg);
            Destroy(goNum);
            goContainer.SetActive(false);
        }
    }
}
