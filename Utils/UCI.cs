using Opponents.Bot;
using Opponents.RandomMoves;
using Chess;

Board board = new();
RandomBot bot = new();

while (true)
{
    string command = Console.ReadLine() ?? "quit";
    List<string> tokens = command.Split(' ').ToList();

    switch (tokens[0])
    {
        case "uci":
        {
            Console.WriteLine("id name Random");
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
            board = tokens[1] == "startpos" ? new() : new(string.Join(" ", tokens[2..8]));
            int whereToReadMoves = tokens[1] == "startpos" ? 3 : 10;

            // The 10th argument will be "moves", so skip this and
            // read until the end of the string, playing moves as we
            // go along.
            foreach (string moveToken in tokens[whereToReadMoves..])
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

            int remaining = board.SideToMove == 0 ? wTime : bTime;
            int inc = board.SideToMove == 0 ? wInc : bInc;

            // bot.timeLeft = remaining / 20 + inc / 2;

            // bot.sw.Reset();
            // bot.sw.Start();

            bot.Think(ref data);

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