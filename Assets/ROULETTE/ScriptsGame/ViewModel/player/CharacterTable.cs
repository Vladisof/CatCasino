using System.Collections.Generic;
using Scripts.ROULETTE.ScriptsGame.Components.chip;
using Scripts.ROULETTE.ScriptsGame.ViewModel.handlers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.ViewModel.player
{
    [CreateAssetMenu(fileName = "New Character Table", menuName = "Scriptable/Character Table")]
    public class CharacterTable : ScriptableObject
    {
        [Header("Parameters")]
        public string tableName;
        public GameObject chipPrefab;
        public CharacterMoney characterMoney;
        public Chip[] chipData;
        
        [Header("Runtime Execution")]
        public int currentTableCount;
        public List<ChipGame> currentTable = new List<ChipGame>();
        public List<TableChp> currentTableInGame = new List<TableChp>();
        public List<int> currentNumbers = new List<int>();
        public Chip currentChipSelected;
        public BoolReactiveProperty currentTableActive;

        [Header("Last Execution")]
        public int lastNumber;
        public List<TableChp> lastTable = new List<TableChp>();
        
        public readonly ISubject<ChipGame> OnDestroyChip = new Subject<ChipGame>();
        public readonly ISubject<bool> OnDestroyLastChip = new Subject<bool>();

        public readonly ISubject<int> OnWinButton = new Subject<int>();
        public readonly ISubject<LngPress> OnPressedButton = new Subject<LngPress>();

        public readonly ISubject<Table> OnRestoreTable = new Subject<Table>();
        public readonly ISubject<bool> OnSaveGame = new Subject<bool>();
        public readonly ISubject<bool> OnLoadGame = new Subject<bool>();
        public readonly ISubject<bool> OnResetGame = new Subject<bool>();
        public readonly ISubject<bool> OnRound = new Subject<bool>();
    }
}
