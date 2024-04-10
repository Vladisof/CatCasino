using UnityEngine;
using Scripts.ROULETTE.ScriptsGame.Components.chip;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;

namespace Commands
{
    public interface ITableInteract
    {
        public void PushChipInButton(CharacterTable characterTable, ButtonTbl buttonData, ChipGame chipGame, Chip chipData, GameObject chipInstance, Vector2 spritePivot, Vector2 offsetPosition, bool fichasOnTop);
    }
}
