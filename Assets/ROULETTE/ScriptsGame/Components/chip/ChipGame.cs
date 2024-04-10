using System.Linq;
using Scripts.ROULETTE.Common.controllers;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.chip
{
    public class ChipGame : MonoBehaviour
    {
        public Transform chipsContainer;
        public SpriteRenderer spriteRenderer;
        public CharacterTable characterTable;
        public IChipRuntime chipRuntime;

        void Awake()
        {
            chipsContainer = GameObject.Find("ChipsContainer").GetComponent<Transform>();
            chipRuntime = GetComponent<IChipRuntime>();
        }
        
        public bool HasNumber(int num)
        {
            return chipRuntime.CurrentButton.buttonValue.Contains(num);
        }

        public void DestroyChip()
        {
            Destroy(this.gameObject);
        }

        void OnDestroy()
        {
            if(chipRuntime.CurrentChipData == null)
                return;

            characterTable.OnDestroyChip
                .OnNext(this);
            
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[4]);
        }
    }
}
