using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.reward
{
    public class RewardToActionMotionDis : MonoBehaviour
    {
        public float _amplitud;
        public float _frequency;

        public Vector3 _offset;

        void Update()
        {
            float x = transform.localPosition.x;
            float y = Mathf.Sin(Time.time * _frequency) * _amplitud;
            float z = transform.localPosition.z;

            transform.localPosition = new Vector3(x,y,z) + _offset;
        }
    }
}
