using Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.buttons
{
    public class StateButton : MonoBehaviour, IResetTableButton, IStatusButton
    {
        public Transform centerPivot;
        public bool IsActive { get; set; }
        public bool IsPressed {get;set;}

        public bool HasFitchOnTop => throw new System.NotImplementedException();

        public void ResetButton(ButtonTbl buttonData)
        {
            buttonData.currentSpritePivot = centerPivot.position;
            buttonData.currentChipsOnTop = 0;
            buttonData.currentOffset = new Vector2(0, 0);
            IsPressed = false;
        }
    }
}
