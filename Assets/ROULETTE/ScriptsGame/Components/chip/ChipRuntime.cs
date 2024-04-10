using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.chip
{
    public class ChipRuntime : MonoBehaviour, IChipRuntime
    {
        public Chip CurrentChipData { get; set; }
        public Vector2 CurrentPosition { get; set; }
        public ButtonTbl CurrentButton { get; set; }

        public void StartChip(Chip chipData, Vector2 position, ButtonTbl buttonPressed, SpriteRenderer currentSprite)
        {
            this.CurrentChipData = chipData;
            this.CurrentPosition = position;
            this.CurrentButton = buttonPressed;
        }
    }
}
