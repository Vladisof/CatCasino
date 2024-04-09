using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ViewModel;
using Controllers;
using Infrastructure;
using System;

namespace Commands
{
    public class MusicTurnCmd : ICommand
    {
        private readonly GameSound _gameSound;
       
        private readonly bool _isOn;

        public MusicTurnCmd(GameSound gameSound, bool isOn)
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
