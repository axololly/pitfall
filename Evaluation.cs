/****************************
Taken from PeSTO's Evaluation
Function on the CPW.
****************************/

using Chess;

namespace AI.Evaluation
{
    public struct Evaluation
    {
        static int PAWN = 0;
        static int KNIGHT = 1;
        static int BISHOP = 2;
        static int ROOK = 3;
        static int QUEEN = 4;
        static int KING = 5;

        static int WHITE = 0;
        static int BLACK = 1;

        static int WHITE_PAWN = 2 * PAWN + WHITE;
        static int BLACK_PAWN = 2 * PAWN + BLACK;
        static int WHITE_KNIGHT = 2 * KNIGHT + WHITE;
        static int BLACK_KNIGHT = 2 * KNIGHT + BLACK;
        static int WHITE_BISHOP = 2 * BISHOP + WHITE;
        static int BLACK_BISHOP = 2 * BISHOP + BLACK;
        static int WHITE_ROOK = 2 * ROOK + WHITE;
        static int BLACK_ROOK = 2 * ROOK + BLACK;
        static int WHITE_QUEEN = 2 * QUEEN + WHITE;
        static int BLACK_QUEEN = 2 * QUEEN + BLACK;
        static int WHITE_KING = 2 * KING + WHITE;
        static int BLACK_KING = 2 * KING + BLACK;

        static int GetPieceColour(int p) => p & 1;
        static int FlipSquare(int sq) => sq ^ 56;

        static int[] mgValue = [82, 337, 365, 477, 1025, 0];
        static int[] egValue = [94, 281, 297, 512, 936, 0];

        static int[] mgPawnTable = [
            0,   0,   0,   0,   0,   0,  0,   0,
            98, 134,  61,  95,  68, 126, 34, -11,
            -6,   7,  26,  31,  65,  56, 25, -20,
            -14,  13,   6,  21,  23,  12, 17, -23,
            -27,  -2,  -5,  12,  17,   6, 10, -25,
            -26,  -4,  -4, -10,   3,   3, 33, -12,
            -35,  -1, -20, -23, -15,  24, 38, -22,
            0,   0,   0,   0,   0,   0,  0,   0,
        ];

        static int[] egPawnTable = [
            0,   0,   0,   0,   0,   0,   0,   0,
            178, 173, 158, 134, 147, 132, 165, 187,
            94, 100,  85,  67,  56,  53,  82,  84,
            32,  24,  13,   5,  -2,   4,  17,  17,
            13,   9,  -3,  -7,  -7,  -8,   3,  -1,
            4,   7,  -6,   1,   0,  -5,  -1,  -8,
            13,   8,   8,  10,  13,   0,   2,  -7,
            0,   0,   0,   0,   0,   0,   0,   0,
        ];

        static int[] mgKnightTable = [
            -167, -89, -34, -49,  61, -97, -15, -107,
            -73, -41,  72,  36,  23,  62,   7,  -17,
            -47,  60,  37,  65,  84, 129,  73,   44,
            -9,  17,  19,  53,  37,  69,  18,   22,
            -13,   4,  16,  13,  28,  19,  21,   -8,
            -23,  -9,  12,  10,  19,  17,  25,  -16,
            -29, -53, -12,  -3,  -1,  18, -14,  -19,
            -105, -21, -58, -33, -17, -28, -19,  -23,
        ];

        static int[] egKnightTable = [
            -58, -38, -13, -28, -31, -27, -63, -99,
            -25,  -8, -25,  -2,  -9, -25, -24, -52,
            -24, -20,  10,   9,  -1,  -9, -19, -41,
            -17,   3,  22,  22,  22,  11,   8, -18,
            -18,  -6,  16,  25,  16,  17,   4, -18,
            -23,  -3,  -1,  15,  10,  -3, -20, -22,
            -42, -20, -10,  -5,  -2, -20, -23, -44,
            -29, -51, -23, -15, -22, -18, -50, -64,
        ];

        static int[] mgBishopTable = [
            -29,   4, -82, -37, -25, -42,   7,  -8,
            -26,  16, -18, -13,  30,  59,  18, -47,
            -16,  37,  43,  40,  35,  50,  37,  -2,
            -4,   5,  19,  50,  37,  37,   7,  -2,
            -6,  13,  13,  26,  34,  12,  10,   4,
            0,  15,  15,  15,  14,  27,  18,  10,
            4,  15,  16,   0,   7,  21,  33,   1,
            -33,  -3, -14, -21, -13, -12, -39, -21,
        ];

        static int[] egBishopTable = [
            -14, -21, -11,  -8, -7,  -9, -17, -24,
            -8,  -4,   7, -12, -3, -13,  -4, -14,
            2,  -8,   0,  -1, -2,   6,   0,   4,
            -3,   9,  12,   9, 14,  10,   3,   2,
            -6,   3,  13,  19,  7,  10,  -3,  -9,
            -12,  -3,   8,  10, 13,   3,  -7, -15,
            -14, -18,  -7,  -1,  4,  -9, -15, -27,
            -23,  -9, -23,  -5, -9, -16,  -5, -17,
        ];

        static int[] mgRookTable = [
            32,  42,  32,  51, 63,  9,  31,  43,
            27,  32,  58,  62, 80, 67,  26,  44,
            -5,  19,  26,  36, 17, 45,  61,  16,
            -24, -11,   7,  26, 24, 35,  -8, -20,
            -36, -26, -12,  -1,  9, -7,   6, -23,
            -45, -25, -16, -17,  3,  0,  -5, -33,
            -44, -16, -20,  -9, -1, 11,  -6, -71,
            -19, -13,   1,  17, 16,  7, -37, -26,
        ];

        static int[] egRookTable = [
            13, 10, 18, 15, 12,  12,   8,   5,
            11, 13, 13, 11, -3,   3,   8,   3,
            7,  7,  7,  5,  4,  -3,  -5,  -3,
            4,  3, 13,  1,  2,   1,  -1,   2,
            3,  5,  8,  4, -5,  -6,  -8, -11,
            -4,  0, -5, -1, -7, -12,  -8, -16,
            -6, -6,  0,  2, -9,  -9, -11,  -3,
            -9,  2,  3, -1, -5, -13,   4, -20,
        ];

        static int[] mgQueenTable = [
            -28,   0,  29,  12,  59,  44,  43,  45,
            -24, -39,  -5,   1, -16,  57,  28,  54,
            -13, -17,   7,   8,  29,  56,  47,  57,
            -27, -27, -16, -16,  -1,  17,  -2,   1,
            -9, -26,  -9, -10,  -2,  -4,   3,  -3,
            -14,   2, -11,  -2,  -5,   2,  14,   5,
            -35,  -8,  11,   2,   8,  15,  -3,   1,
            -1, -18,  -9,  10, -15, -25, -31, -50,
        ];

        static int[] egQueenTable = [
            -9,  22,  22,  27,  27,  19,  10,  20,
            -17,  20,  32,  41,  58,  25,  30,   0,
            -20,   6,   9,  49,  47,  35,  19,   9,
            3,  22,  24,  45,  57,  40,  57,  36,
            -18,  28,  19,  47,  31,  34,  39,  23,
            -16, -27,  15,   6,   9,  17,  10,   5,
            -22, -23, -30, -16, -16, -23, -36, -32,
            -33, -28, -22, -43,  -5, -32, -20, -41,
        ];

        static int[] mgKingTable = [
            -65,  23,  16, -15, -56, -34,   2,  13,
            29,  -1, -20,  -7,  -8,  -4, -38, -29,
            -9,  24,   2, -16, -20,   6,  22, -22,
            -17, -20, -12, -27, -30, -25, -14, -36,
            -49,  -1, -27, -39, -46, -44, -33, -51,
            -14, -14, -22, -46, -44, -30, -15, -27,
            1,   7,  -8, -64, -43, -16,   9,   8,
            -15,  36,  12, -54,   8, -28,  24,  14,
        ];

        static int[] egKingTable = [
            -74, -35, -18, -18, -11,  15,   4, -17,
            -12,  17,  14,  17,  17,  38,  23,  11,
            10,  17,  23,  15,  20,  45,  44,  13,
            -8,  22,  24,  27,  26,  33,  26,   3,
            -18,  -4,  21,  24,  27,  23,   9, -11,
            -19,  -3,  11,  21,  23,  16,   7,  -9,
            -27, -11,   4,  13,  14,   4,  -5, -17,
            -53, -34, -21, -11, -28, -14, -24, -43
        ];

        static int[] GetMGTable(int index) => index switch
        {
            0 => mgPawnTable,
            1 => mgKnightTable,
            2 => mgBishopTable,
            3 => mgRookTable,
            4 => mgQueenTable,
            5 => mgKingTable,
            
            _ => throw new Exception("invalid index when looking up MG table")
        };

        static int[] GetEGTable(int index) => index switch
        {
            0 => egPawnTable,
            1 => egKnightTable,
            2 => egBishopTable,
            3 => egRookTable,
            4 => egQueenTable,
            5 => egKingTable,
            
            _ => throw new Exception("invalid index when looking up MG table")
        };

        static int[] gamephaseInc = [0, 0, 1, 1, 1, 1, 2, 2, 4, 4, 0, 0];

        static int[,] mgTable = new int[12, 64];
        static int[,] egTable = new int[12, 64];

        public Evaluation()
        {
            // Create all the tables
            int pc, p, sq;

            for (p = PAWN, pc = WHITE_PAWN; p <= KING; pc += 2, p++)
            {
                for (sq = 0; sq < 64; sq++)
                {
                    mgTable[pc, sq] = mgValue[p] + GetMGTable(p)[sq];
                    egTable[pc, sq] = egValue[p] + GetEGTable(p)[sq];

                    mgTable[pc + 1, sq] = mgValue[p] + GetMGTable(p)[FlipSquare(sq)];
                    egTable[pc + 1, sq] = egValue[p] + GetEGTable(p)[FlipSquare(sq)];
                }
            }
        }

        public int Eval(Board board)
        {
            int[] mg = new int[2];
            int[] eg = new int[2];
            int gamePhase = 0;

            mg[WHITE] = 0;
            mg[BLACK] = 0;
            
            eg[WHITE] = 0;
            eg[BLACK] = 0;

            // Evaluate each piece in the mailbox
            for (int sq = 0; sq < 64; sq++)
            {
                int piece = (int)board.Mailbox[sq];
                
                if (piece != (int)Piece.Empty)
                {
                    // Get the colour of the piece
                    mg[piece & 1] += mgTable[piece, sq];
                    eg[piece & 1] += egTable[piece, sq];
                    gamePhase += gamephaseInc[piece];
                }
            }

            // Tapered evaluation
            int mgScore = mg[board.SideToMove] - mg[board.SideToMove ^ 1];
            int egScore = eg[board.SideToMove] - eg[board.SideToMove ^ 1];

            int mgPhase = Math.Min(gamePhase, 24);
            int egPhase = 24 - mgPhase;

            return (mgScore * mgPhase + egScore * egPhase) / 24;
        }

        public int ScoreIndividualPiece(Piece piece)
        {
            return ((int)piece >> 1) switch
            {
                // 1 is a Pawn
                1 => 1,

                // 2 is a Knight
                2 => 3,

                // 3 is a Bishop
                3 => 3,

                // 4 is a Rook
                4 => 5,
                
                // 5 is a Queen
                5 => 9,

                // 6 is a King, which doesn't have a score
                _ => throw new Exception($"piece '{piece}' cannot be scored because it is not accounted for.")
            };
        }
    }
}