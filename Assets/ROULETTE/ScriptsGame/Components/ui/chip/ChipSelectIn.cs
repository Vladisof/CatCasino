using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.ui.chip
{
    public class ChipSelectIn : MonoBehaviour
    {
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        public CharacterTable characterTable;
        private bool _selectorAnchor;

        void Start()
        {
            characterTable.currentTableActive
                .Subscribe(IsTableActive)
                .AddTo(this);
        }

        private void IsTableActive(bool isActive)
        {
            _selectorAnchor = isActive;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("ChipSelectUI") && characterTable.currentTableActive.Value && _selectorAnchor)
            {
                ChipSelected chipSelected = other.gameObject.GetComponent<ChipSelected>();
                GameFactory.ChipSelect(characterTable, chipSelected.chipData).Execute();
            }
        }
    }
}
