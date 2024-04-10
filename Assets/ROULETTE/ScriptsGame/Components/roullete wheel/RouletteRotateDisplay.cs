using Scripts.ROULETTE.ScriptsGame.ViewModel.round;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.roullete_wheel
{
    public class RouletteRotateDisplay : MonoBehaviour
    {    
        [FormerlySerializedAs("gameRoulette")]
        public GameRoullete gameRoullete;
        public GameObject ballRotator;
        public GameObject wheelRotator;

        void Start()
        {
            gameRoullete.currentSpeed = gameRoullete.defaultSpeed;
            gameRoullete.OnRotate
                .Subscribe(OnRotateRoullete)
                .AddTo(this);
        }

        private void OnRotateRoullete(bool isRotate)
        {
            gameRoullete.currentSpeed = gameRoullete.defaultSpeed;
        }

        void FixedUpdate()
        {
            wheelRotator.transform.Rotate(Vector3.forward * gameRoullete.currentSpeed * Time.deltaTime);
            ballRotator.transform.Rotate(Vector3.back * gameRoullete.currentSpeed * 3 * Time.deltaTime);  
        }
    }
}
