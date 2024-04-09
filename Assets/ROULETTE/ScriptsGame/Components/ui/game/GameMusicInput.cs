using Commands;
using UnityEngine;
using ViewModel;
using BE;

namespace Scripts.ROULETTE.ScriptsGame.Components.ui.game
{
    public class GameMusicInput : MonoBehaviour
    {
        public GameSound gameSound;
        public GameCmdFactory gameCmdFactory;
        public BeAudioManager audioManager;
        
        public void OnClick()
        {
            gameCmdFactory.MusicTurnCmd(gameSound, !gameSound.isMusicOn.Value).Execute();
        }
    }
}
