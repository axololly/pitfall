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
        
        public int countedNodes = 0;
        public int maxDepthReached = 0;
        int checkEvery = 2048;
        const int MAX_DEPTH = 255;
        
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
            ref SearchData data
        )
        {
            stopped = false;
            maxDepthReached = 0;
            
            int bestScore = -Inf;

            for (int localDepth = 1; localDepth < MAX_DEPTH + 1; localDepth++)
            {
                if (stopped) break;

                maxDepthReached++;

                bestScore = Math.Max(bestScore, Search(ref data, localDepth));
            }

            return bestScore;
        }

        int Search(
            ref SearchData data,
            int depth,
            int ply = 0,
            int alpha = -Inf,
            int beta = Inf
        )
        {
            if (stopped) return 0;

            if (countedNodes++ % checkEvery == 0)
            {
                timeLeft -= (int)sw.ElapsedMilliseconds;

                if (timeLeft < 0)
                {
                    stopped = true;
                    sw.Stop();
                    return 0;
                }
            }

            // Search all captures after we reach the end of the search.
            // if (depth == 0) return -QSearch(ref data.pos, -beta, -alpha);
            if (depth == 0) return eval.Eval(ref data.pos);

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
                int score = -Search(ref data, depth - 1, ply + 1, -beta, -alpha);
                data.pos.UndoMove();

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

        // Search all captures using quiescence search,
        // otherwise called QSearch
        int QSearch(ref Board board, int alpha, int beta)
        {
            int staticEval = eval.Eval(ref board);

            if (staticEval >= beta) return beta;
            if (staticEval < alpha) alpha = staticEval;

            List<Move> moves = board.GenerateLegalMoves(onlyCaptures: true);

            foreach (Move move in moves)
            {
                board.MakeMove(move);

                int score = -QSearch(ref board, -beta, -alpha);

                board.UndoMove();

                if (score >= beta) return beta;
                if (score < alpha) alpha = score;
            }

            return alpha;
        }
    }
}