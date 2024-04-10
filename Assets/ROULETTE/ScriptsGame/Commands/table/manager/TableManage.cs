using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UnityEngine;

namespace Commands
{
    public class TableManage
    {
        public readonly ITables tables;
        public readonly ITableInteract tableInteract;

        public TableManage(CharacterTable characterTable, ITables tables, ITableInteract tableInteraction)
        {
            this.tables = tables;
            this.tables.characterTable = characterTable;

            this.tableInteract = tableInteraction;
        }
    }
}
