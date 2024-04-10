using Infrastructure;
using Scripts.ROULETTE.ScriptsGame.Commands.payment_system;
using Scripts.ROULETTE.ScriptsGame.Commands.save_system;
using Scripts.ROULETTE.ScriptsGame.Infrastructure.save;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands
{    
    [CreateAssetMenu(fileName = "New CharacterCmdFactory", menuName = "Factory/Character Command Factory")]
    public class CharacterFactory : ScriptableObject
    {  
        public static LoadRound LoadPlayer(CharacterTable characterTable)
        {
            return new LoadRound(characterTable, new SaveRoundGateway());
        }        
        public static SaveRound SavePlayer(CharacterTable characterTable)
        {
            return new SaveRound(characterTable, new SaveRoundGateway());
        }        
        public static SaveCashTurn SaveCash(CharacterFactory characterFactory, CharacterTable characterTable, int payment)
        {
            return new SaveCashTurn(characterFactory, characterTable, payment, new SaveRoundGateway());
        }        
    }
}
