using Managers;
using Scripts.ROULETTE.ScriptsGame.Commands;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.Components.reward
{
    public class GameRewardCloseIn : MonoBehaviour
    {
        [FormerlySerializedAs("gameCmdFactory")]
        public GameFactory gameFactory;
        
        public void OnClick()
        {
            GameManager.Instance.LoadScene("Game");
            GameManager.Instance.ToggleGame();
        }
    }
}
