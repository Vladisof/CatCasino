using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.chip
{
    public interface IChipRuntime
    {
        public Chip CurrentChipData {get; set;}
        public Vector2 CurrentPosition {get; set;}
        public ButtonTbl CurrentButton {get; set;}
        public void StartChip(Chip chipData, Vector2 position, ButtonTbl buttonPressed, SpriteRenderer currentSprite);
    }
}
