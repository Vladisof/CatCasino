using Commands;
using UnityEngine;
using Cat;
using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.audio;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.ui.game
{
    public class GameMusicInp : MonoBehaviour
    {
        public GameSound gameSound;
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        public BenAudioManager audioManager;
        
        public void OnClick()
        {
            GameFactory.MusicTurnCmd(gameSound, !gameSound.isMusicOn.Value).Execute();
        }
    }
}
