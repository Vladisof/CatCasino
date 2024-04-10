using System.Linq;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.buttons
{
    public class ButtonTableDisplay : MonoBehaviour
    {
        public CharacterTable characterTable;
        public ButtonTbl buttonData;
        public Animator animatorButton;

        private void Start()
        {
            characterTable.OnWinButton
                .Subscribe(OnWin)
                .AddTo(this);

            characterTable.OnPressedButton
                .Subscribe(OnPressed)
                .AddTo(this);
        }

        private void OnWin(int num)
        {
            if(!buttonData.isPlano)
                return;

            bool containNumber = buttonData.buttonValue.Contains(num);
            
            if(containNumber)
            {
                FxWin();
            } 
        }
        public void OnPressed(LngPress lngPress) 
        {
            if(!buttonData.isPlano)
                return;
                
            if(CheckIfIsLongPressed(lngPress.values))          
                FxPressed(lngPress.isPressed);
        }

        private bool CheckIfIsLongPressed(int[] longPressValues)
        {
            if(buttonData.buttonValue.Count() > 1)
                return false;
            
            return longPressValues.Contains(buttonData.buttonValue[0]);
        }


        public void FxWin()
        {
            animatorButton.SetTrigger("Win");
        }
        public void FxPressed(bool isPress)
        {
            animatorButton.SetBool("IsPressed", isPress);
        }
    }
}
