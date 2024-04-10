﻿using Scripts.ROULETTE.ScriptsGame.ViewModel.audio;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.Common.controllers
{
    public class PlayerSound : Singlenton<PlayerSound>
    {
        public GameSound gameSound;
        public AudioSource _audioSourceFx;
        public AudioSource _audioSourceMusic;
        
        private bool _isPlayingFx = false;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            gameSound.OnSound
                .Subscribe(OnSound)
                .AddTo(this);

            gameSound.OnMusic
                .Subscribe(OnMusic)
                .AddTo(this);
            
            gameSound.isFxOn
                .Subscribe(OnFxIs)
                .AddTo(this);
            
            gameSound.isMusicOn
                .Subscribe(OnMusicIs)
                .AddTo(this);

            OnGameOpened();
        }
        
        private void OnMusicIs(bool value)
        {
            _audioSourceMusic.mute = !value;
        }
        private void OnFxIs(bool value)
        {
            _audioSourceFx.mute = !value;
        }

        private void OnMusic(AudioEvent audioEvent)
        {
            audioEvent.Play(_audioSourceMusic);
            _audioSourceMusic.loop = true;
        }
        private void OnSound(AudioEvent audioEvent)
        {
            audioEvent.Play(_audioSourceFx);
        }

        void OnGameOpened()
        {
            gameSound.isFxOn.Value = true;
            gameSound.OnMusic.OnNext(gameSound.audioReferences[0]);
        }
    }
}
