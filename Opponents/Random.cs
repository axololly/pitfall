using Opponents.Bot;

namespace Opponents.RandomMoves
{
    public struct RandomBot
    {
        public void Think(
            ref SearchData data
        )
        {
            Random rng = new();
            
            var moves = data.pos.GenerateLegalMoves();

            if (moves.Count > 0) data.bestMoveRoot = moves[rng.Next() % moves.Count];
        }
    }
}