﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using ViewModel;
using System;
using Controllers;
using TMPro;

namespace Components
{
    public class GameMoneyDisplay : MonoBehaviour
    {
        public CharacterTable characterTable;
        public TextMeshProUGUI moneyLabel;
        public TextMeshProUGUI betLabel;


        public void Start()
        {
            characterTable.characterMoney.characterBet
                .Subscribe(OnChangeBet)
                .AddTo(this);

            characterTable.characterMoney.characterMoney
                .Subscribe(OnChangeMoney)
                .AddTo(this);
        }

        private void OnChangeBet(int value)
        {
            betLabel.text = value.ToString();
        }

        private void OnChangeMoney(int value)
        {
            moneyLabel.text = value.ToString();
        }
    }
}
