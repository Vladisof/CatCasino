using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using System;

namespace Components
{
    public class InterfaceDisplay : MonoBehaviour
    {    
        public CharacterTable characterTable;
        
        public GameObject optionsAnchor;
        public GameObject bottomAnchor;
        public GameObject leftAnchor;
        
        public Animator shadowGame;

        private void Start()
        {
            characterTable.OnRound
                .Subscribe(DisplayInterface)
                .AddTo(this);
            
            characterTable.characterMoney.currentPayment
                .Subscribe(OnPaymentDisplay)
                .AddTo(this);
        }

        private void OnPaymentDisplay(int value)
        {
            bool display = value is <= 0 and >= 0;

            optionsAnchor.SetActive(display);
            bottomAnchor.SetActive(display);
            leftAnchor.SetActive(display);
        }

        public void DisplayInterface(bool isRound)
        {
            shadowGame.SetBool("Shadow", isRound);
            
            if(!isRound)
                return;
            
            optionsAnchor.SetActive(!isRound);
            bottomAnchor.SetActive(!isRound);
            leftAnchor.SetActive(!isRound);
        }
    }
}