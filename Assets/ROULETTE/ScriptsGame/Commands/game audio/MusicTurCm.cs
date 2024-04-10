using Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.audio;

namespace Scripts.ROULETTE.ScriptsGame.Commands.game_audio
{
    public class MusicTurCm : ICommand
    {
        private readonly GameSound _gameSound;
       
        private readonly bool _isOn;

        public MusicTurCm(GameSound gameSound, bool isOn)
        {
            _gameSound = gameSound;
            this._isOn = isOn;
        }

        public void Execute()
        {
            _gameSound.isMusicOn.Value = _isOn;
        }
    }
}
