using Scripts.ROULETTE.ScriptsGame.Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.round
{
    public class PlayerSaveIn : MonoBehaviour
    {

        public CharacterTable characterTable;
        [FormerlySerializedAs("characterCmdFactory")]
        public CharacterFactory characterFactory;

        private void Start()
        {
            characterTable.OnSaveGame
                .Subscribe(SaveRound)
                .AddTo(this);
            
            characterTable.OnLoadGame
                .Subscribe(LoadRound)
                .AddTo(this);
        }
        public void SaveRound(bool value) 
        {
            if(!value)
                return;

            CharacterFactory.SavePlayer(characterTable).Execute();
        }
        public void LoadRound(bool value)
        {   
            CharacterFactory.LoadPlayer(characterTable).Execute();
        }
    }
}
