using Commands;
using Infrastructure;
using Managers;
using Scripts.ROULETTE.ScriptsGame.Commands.game_audio;
using Scripts.ROULETTE.ScriptsGame.Commands.game_ui;
using Scripts.ROULETTE.ScriptsGame.Commands.payment_system;
using Scripts.ROULETTE.ScriptsGame.Commands.reward_system;
using Scripts.ROULETTE.ScriptsGame.Commands.round_controller;
using Scripts.ROULETTE.ScriptsGame.Commands.table;
using Scripts.ROULETTE.ScriptsGame.Commands.table.manager;
using Scripts.ROULETTE.ScriptsGame.Components.player.round.table;
using Scripts.ROULETTE.ScriptsGame.Infrastructure.payment;
using Scripts.ROULETTE.ScriptsGame.Infrastructure.save;
using Scripts.ROULETTE.ScriptsGame.ViewModel.audio;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using Scripts.ROULETTE.ScriptsGame.ViewModel.reward;
using Scripts.ROULETTE.ScriptsGame.ViewModel.round;
using Scripts.ROULETTE.ScriptsGame.ViewModel.table;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Commands
{    
    [CreateAssetMenu(fileName = "New GameCmdFactory", menuName = "Factory/Game Command Factory")]
    public class GameFactory : ScriptableObject
    {   
        public static ButtonPushChipTable ButtonTableTurn(CharacterTable characterTable, GameObject chipInstance, ButtonTbl buttonData)
        {
            return new ButtonPushChipTable(new TableManage(characterTable, new GameTables(), new TableInter()), chipInstance, buttonData);
        }    
        public static ChipsSelectCm ChipSelect(CharacterTable characterTable, Chip arrayValue)
        {
            return new ChipsSelectCm(characterTable, arrayValue);
        }      
        public static PlayTur PlayTurn(CharacterTable characterTable, GameRoullete gameRoullete)
        {
            return new PlayTur(GameManager.Instance, characterTable, gameRoullete, new PlayRoundGateway(), new PaymentGateway());
        }
        
        public static MusicTurCm MusicTurnCmd(GameSound gameSound, bool isOn)
        {
            return new MusicTurCm(gameSound, isOn);
        }    
        public static ResetTur ResetTableTurn(MagnetDestroyDisplay magnetDestroyDisplay, CharacterTable characterTable, int delayTime = 0)
        {
            return new ResetTur(magnetDestroyDisplay, characterTable, delayTime);
        }      
        public static UndoTur UndoTableTurn(CharacterTable characterTable)
        {
            return new UndoTur(characterTable);
        }      
        public static RestoreTurn RestoreTableTurn(CharacterTable characterTable)
        {
            return new RestoreTurn(characterTable);
        } 
        
        public static RewardSceneOpen RewardTurn(string rewardScene)
        {
            return new RewardSceneOpen(rewardScene);
        }      
        public static FortunePayment FortuneTurn(CharacterFactory characterFactory, CharacterTable characterTable, RewardFort rewardFort, int finalPosition)
        {
            return new FortunePayment(characterFactory, characterTable, rewardFort, finalPosition);
        }      
        public static FortunePlay FortuneRewardTurn(RewardFort rewardFort)
        {
            return new FortunePlay(rewardFort);
        }      
    }
}
