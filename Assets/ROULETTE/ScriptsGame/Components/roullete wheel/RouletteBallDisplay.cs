using System.Collections.Generic;
using System.Linq;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.round;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.roullete_wheel
{
    public class RouletteBallDisplay : MonoBehaviour
    {
        public CharacterTable characterTable;
        [FormerlySerializedAs("gameRoulette")]
        public GameRoullete gameRoullete;
        [FormerlySerializedAs("_anchorNumbers")]
        public GameObject[] anchorNumbers;
        public GameObject sphereContainer;
        public GameObject sphereDefault;

        private GameObject _sphere;
        private Vector3 _ballPosition;
        
        void Start()
        {
            characterTable.OnRound
                .Subscribe(OnRound)
                .AddTo(this);

            gameRoullete.OnNumber
                .Subscribe(SetBallInRoulette)
                .AddTo(this);
        }

        private void OnRound(bool isRound)
        {
            if(isRound)
            {
                DestroyLastSphere();
                sphereDefault.SetActive(true);
            }
        }

        public void SetBallInRoulette(int num)
        {    
            sphereDefault.SetActive(false);
            DestroyLastSphere();
            
            IEnumerable<GameObject> anchor = anchorNumbers.Where(anc => anc.name == $"handler_{num}");
            _ballPosition = anchor.ToArray()[0].gameObject.transform.position;
            
            _sphere = Instantiate(gameRoullete.sphere);
            _sphere.transform.position = _ballPosition;
            _sphere.SetActive(true);
            _sphere.transform.SetParent(sphereContainer.transform);

            Debug.Log($"Roullete positioning ball in number {num}!");
        }

        void DestroyLastSphere()
        {
            if(sphereContainer.transform.childCount > 0)
                Destroy(_sphere);
        }
    }
}
