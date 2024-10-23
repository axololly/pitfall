using System.Diagnostics;
using Utils.Evaluation;
using Chess;
using Chess.MoveGen;

namespace Opponents.Bot
{
    public struct SearchData
    {
        public Board pos;
        public Move? bestMoveRoot;

        public SearchData(Board pos)
        {
            this.pos = pos;
            bestMoveRoot = null;
        }
    }

    public struct Pitfall
    {
        const int Inf = 999_999_999;
        Evaluation eval = new();
        int countedNodes = 0;

        int checkEvery = 2048;
        
        public Stopwatch sw = new();
        public int timeLeft;

        public bool stopped = false;

        public Pitfall() {}

        // Sort by MVV-LVA (Most Valuable Victim - Least Valuable Agressor)
        static void SortInPlace(Board board, ref List<Move> moves)
        {
            static int ScoreMoveWithMVV_LVA(Board board, Move m)
            {
                // If the move is quiet, assign it a base score of 10.
                // If the move is a capture, order using MVV-LVA.
                return board.Mailbox[m.dst] == Piece.Empty
                    ? 10
                    : (100 * (int)board.Mailbox[m.dst]) - (100 - (int)board.Mailbox[m.src]);
            }

            static int DelegateSort(int score1, int score2)
            {
                return score1 < score2 ? -1 : (score1 == score2) ? 0 : 1;
            }

            moves.Sort(
                (m1, m2) => DelegateSort(
                    ScoreMoveWithMVV_LVA(board, m1),
                    ScoreMoveWithMVV_LVA(board, m2)
                )
            );
        }

        public int Think(
            ref SearchData data,
            int depth,
            int ply = 0,
            int alpha = -Inf,
            int beta = Inf
        )
        {
            if (stopped) return 0;

            if (countedNodes++ % checkEvery == 0) timeLeft -= (int)sw.ElapsedMilliseconds / 1000;

            if (depth == 0) return eval.Eval(data.pos);
            if (data.pos.IsDraw) return 0;

            List<Move> moves = data.pos.GenerateLegalMoves();

            if (moves.Count == 0)
            {
                // If in check, it's checkmate
                // Otherwise, it's stalemate
                return data.pos.InCheck ? -Inf + ply : 0;
            }

            SortInPlace(data.pos, ref moves);

            Move? bestMove = null;
            int bestScore = -Inf;

            foreach (Move move in moves)
            {
                data.pos.MakeMove(move);
                int score = -Think(ref data, depth - 1, ply + 1, -beta, -alpha);
                data.pos.UndoMove();

                // Debug
                // if (ply == 1) Console.WriteLine($"{move} - {score}");

                if (score > bestScore)
                {
                    bestScore = score;

                    if (score > alpha)
                    {
                        alpha = score;
                        bestMove = move;
                    }

                    if (score >= beta) break;
                }
            }

            if (ply == 0) data.bestMoveRoot = bestMove;

            return bestScore;
        }
    }
}