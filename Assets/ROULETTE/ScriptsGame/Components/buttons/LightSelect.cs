using Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.ROULETTE.ScriptsGame.Components.buttons
{
    public class LightSelect : MonoBehaviour, ILongPress
    {
        [Tooltip ("Hold duration in seconds"),Range (0.1f, 5f)]
        
        public float holdDuration = 0.5f ;
        public Button button;

        private bool _isPointerDown = false ;
        private bool _isLongPressed = false ;
        private float _elapsedTime = 0f;
        
        public void SetPointerDown(bool value)
        {
            _isPointerDown = value;
        }
        public void ResetPointer()
        {
            _isPointerDown = false;
            _isLongPressed = false;
            _elapsedTime = 0f ;
        }
        public void LongPressCheck(CharacterTable characterTable, ButtonTbl buttonData)
        {
            if (_isPointerDown && !_isLongPressed)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= holdDuration)
                {
                    _isLongPressed = true;
                    _elapsedTime = 0f;
                    if (button.interactable)
                    {
                        LongPress(characterTable, buttonData, true);
                    }
                }
            }
        }
        public void LongPress(CharacterTable characterTable, ButtonTbl buttonData, bool currentStatus)
        {
            LngPress lngPress = new LngPress()
            {
                isPressed = currentStatus,
                values = buttonData.buttonValue
            };

            characterTable.OnPressedButton.OnNext(lngPress);
        }
    }
}
