using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.Components.player.round.table
{    
    public class MagnetDestroyInput : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Chip"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
