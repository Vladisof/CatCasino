using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.ViewModel.audio
{
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play(AudioSource audio);
    }
}
