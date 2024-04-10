using Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.ROULETTE.ScriptsGame.Components.buttons
{
    [RequireComponent (typeof(Button))]
    public class ButtonTableInput : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public CharacterTable characterTable;
        public ButtonTbl buttonData;

        private IStatusButton _statusButton;
        private IResetTableButton _resetTableButton;
        private IInterButton _interactButton;
        private ILongPress _longPress;

        private void Awake() 
        {
            _statusButton = GetComponent<IStatusButton>();    
            _resetTableButton = GetComponent<IResetTableButton>();    
            _interactButton = GetComponent<IInterButton>();    
            _longPress = GetComponent<ILongPress>();    
        }
        
        void Start()
        {
            _resetTableButton.ResetButton(buttonData);

            characterTable.currentTableActive
                .Subscribe(OnActiveButton)
                .AddTo(this);
        }
        
        private void OnActiveButton(bool isActive)
        {
            _statusButton.IsActive = isActive;
            if(_statusButton.IsActive)
                _resetTableButton.ResetButton(buttonData);       
        }

        public void Click()
        {
            if (!_statusButton.IsActive)
                return;

            _interactButton.InstantiateChip(characterTable, buttonData);
        }

        public void OnPointerDown (PointerEventData eventData) 
        {
            _longPress.SetPointerDown(true);   
        }

        private void Update ()
        {
            _longPress.LongPressCheck(characterTable, buttonData);
        }
        
        public  void OnPointerUp (PointerEventData eventData) 
        {
            _longPress.LongPress(characterTable, buttonData, false);
            _longPress.ResetPointer();
            Click();
        }
    }
}
