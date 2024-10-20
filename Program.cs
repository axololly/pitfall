using Chess;
using Chess.MoveGen;
using AI.Evaluation;
using System.Diagnostics;
using Chess.Types.Squares;

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

class Program
{
    static int Inf = 999_999_999;
    static Evaluation eval = new();

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

    static int Search(ref SearchData data, int depth, int ply, int alpha, int beta)
    {
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
            int score = -Search(ref data, depth - 1, ply + 1, -beta, -alpha);
            data.pos.UndoMove();

            // Debug
            if (ply == 1) Console.WriteLine($"{move} - {score}");

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

    static void Main()
    {
        Board board = new();
        SearchData data = new(board);
        Stopwatch sw = new();

        sw.Start();

        int result = -Search(
            ref data,
            10, 0, -Inf, Inf
        );

        sw.Stop();

        Console.WriteLine($"\nBoard:\n{board}\n\nResult: {result}\nBest move: {data.bestMoveRoot}\nTime taken: {(double)sw.ElapsedMilliseconds / 1000}");
    }
}