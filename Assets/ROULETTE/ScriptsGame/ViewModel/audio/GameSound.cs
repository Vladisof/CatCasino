using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.ViewModel.audio
{
    [CreateAssetMenu(fileName = "New Game Sound", menuName = "Scriptable/Game Sound")]
    public class GameSound : ScriptableObject
    {
        public BoolReactiveProperty isFxOn;
        public BoolReactiveProperty isMusicOn;
        public SimpleAudio[] audioReferences;
        public readonly ISubject<SimpleAudio> OnSound = new Subject<SimpleAudio>();
        public readonly ISubject<SimpleAudio> OnMusic = new Subject<SimpleAudio>();
    }
}
