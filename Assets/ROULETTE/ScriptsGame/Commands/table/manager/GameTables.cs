using Commands;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;

namespace Scripts.ROULETTE.ScriptsGame.Commands.table.manager
{
    public class GameTables : ITables
    {
        public CharacterTable characterTable { get; set; }
    }
}