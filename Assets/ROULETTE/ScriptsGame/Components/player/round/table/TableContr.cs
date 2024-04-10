using System.Linq;
using Scripts.ROULETTE.ScriptsGame.Components.buttons;
using Scripts.ROULETTE.ScriptsGame.Components.chip;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.round.table
{
    public class TableContr : MonoBehaviour, ITableController
    {
        public CharacterTable characterTable;

        public void LastChipDestroy(bool value)
        {
            if(characterTable.currentTableCount > 0)
            {
                Debug.Log("Undo chip of the table!");
                Destroy(characterTable.currentTable[characterTable.currentTable.Count - 1].gameObject);
            }
        }

        public void DestroyChipFromTable(ChipGame fiche) 
        {

            if(fiche.chipRuntime.CurrentChipData.chipValue > 0 && characterTable.currentTableCount > 0)
            {
                characterTable.characterMoney.DeleteChip(fiche.chipRuntime.CurrentChipData.chipValue);
                characterTable.currentTableCount--;
                characterTable.currentTableInGame.RemoveAt(characterTable.currentTableInGame.Count() - 1);     
            }   

            fiche.chipRuntime.CurrentButton.SubstrateCurrentOffset();
            characterTable.currentTable.Remove(fiche);
        }   

        public void ResetTable(bool destroyChips)
        {
            characterTable.characterMoney.characterBet.Value = 0;
            characterTable.currentTableCount = 0;
            characterTable.currentTableInGame.Clear();

            if(!destroyChips)
                return;

            foreach(ChipGame go in characterTable.currentTable)
            {
                Destroy(go.gameObject);
            }

            characterTable.currentTableActive.Value = true;
            characterTable.currentTable.Clear();
            characterTable.lastTable.Clear();
        }

        public void RestoreTable(Table table)
        {
            if(!characterTable.currentTableActive.Value)
                return;
            
            Debug.Log($"Loading current player table with {table.TableChips.Count()} chips");

            foreach(TableChp buttonChip in table.TableChips)
            {
                GameObject buttonInstance = GameObject.Find(buttonChip.idButton);
                GameObject chipInstance = Instantiate(characterTable.chipPrefab);
                chipInstance.SetActive(false);
                GameObject chipContainer = GameObject.FindGameObjectWithTag("ChipContainer");
                ButtonTbl buttonData = buttonInstance.GetComponent<ButtonTableInput>().buttonData;
                Chip chipData = characterTable.chipData.Where(chip => chip.chipKay.ToString() == buttonChip.idChip).First();
                
            }
        }
    }
}
