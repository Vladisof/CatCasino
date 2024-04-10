using Commands;
using Scripts.ROULETTE.ScriptsGame.Components.chip;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands.table.manager
{
    public class TableInter : ITableInteract
    {
        public void PushChipInButton(CharacterTable characterTable, ButtonTbl buttonData, ChipGame chipGame, Chip chipData, GameObject chipInstance, Vector2 spritePivot, Vector2 offsetPosition, bool fichasOnTop)
        {
            characterTable.currentTableCount++;

            chipInstance.SetActive(true);
            chipInstance.name = $"{chipData.chipName}_{characterTable.currentTableCount.ToString()}";
            
            if(buttonData.isPlano)
                chipInstance.transform.SetParent(chipGame.chipsContainer.transform.GetChild(0));
            else 
                chipInstance.transform.SetParent(chipGame.chipsContainer.transform.GetChild(1));
            
            Vector2 position = Vector2.zero;

            if (fichasOnTop)
            {
                position = spritePivot + offsetPosition;
                chipInstance.transform.position = position;
            } else
            {
                position = spritePivot;
                chipInstance.transform.position = position;
            }
            
            chipGame.chipRuntime.StartChip(chipData, position, buttonData, chipGame.spriteRenderer);
            
            characterTable.currentTable.Add(chipGame);

            TableChp buttonChp = new TableChp(){
                idButton = buttonData.buttonName, 
                idChip = chipData.chipKay.ToString()
            };

            characterTable.currentTableInGame.Add(buttonChp);
        }
    }
}