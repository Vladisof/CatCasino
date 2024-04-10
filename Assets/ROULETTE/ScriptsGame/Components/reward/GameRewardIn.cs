using Scripts.ROULETTE.ScriptsGame.Commands;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.reward
{
    public class GameRewardIn : MonoBehaviour
    {
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        
        public void OnClick(string scene)
        {
            GameFactory.RewardTurn(scene).Execute();
        }
    }
}
