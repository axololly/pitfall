using Opponents.Bot;
using Opponents.RandomMoves;
using Chess;

Board board = new();
Pitfall bot = new();

while (true)
{
    string command = Console.ReadLine() ?? "quit";
    List<string> tokens = command.Split(' ').ToList();

    switch (tokens[0])
    {
        case "uci":
        {
            Console.WriteLine("id name Pitfall");
            Console.WriteLine("id author axololly");
            Console.WriteLine("uciok");
            break;
        }

        case "ucinewgame":
        {
            board = new();
            break;
        }

        case "isready":
        {
            Console.WriteLine("readyok");
            break;
        }

        case "position":
        {
            // "fen" is the second argument, so go from the third
            // and continue for 6 "arguments" that we join together.
            string FEN = string.Join(" ", tokens[2..8]);

            board = new(FEN);

            // The 10th argument will be "moves", so skip this and
            // read until the end of the string, playing moves as we
            // go along.
            foreach (string moveToken in tokens[10..])
            {
                board.MakeMove(
                    board
                        .GenerateLegalMoves()
                        .Where(m => $"{m}" == moveToken)
                        .First()
                );
            }

            break;
        }

        case "go":
        {
            int wTime = int.Parse(tokens[2]);
            int bTime = int.Parse(tokens[4]);

            int wInc = int.Parse(tokens[6]);
            int bInc = int.Parse(tokens[8]);

            SearchData data = new(board);

            bot.Think(ref data, 8);

            Console.WriteLine($"bestmove {data.bestMoveRoot}");

            break;
        }

        case "quit":
        {
            return 0;
        }

        default:
        {
            Console.WriteLine("unknown uci command");
            break;
        }
    }
}