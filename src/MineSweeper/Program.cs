using System;
using System.Text;

namespace MineSweeper
{
	public sealed class Program
	{
		public static void Main(string[] args)
		{
			var game = new MineSweeperGame();
			game.InitializeGame(5, 5, 3);

			Play(game);
		}

		// this should probably return a command that we can turn into an
		// action in a different location than where we parse input
		private static void Play(MineSweeperGame game)
		{
			while (game.Status == GameStatus.InProgress)
			{
				PrintBoard(game);

				var parseError = false;
				var unparsedPosition = Console.ReadLine();
				var parts = unparsedPosition.Split(',');

				if (parts.Length == 2)
				{
					if (int.TryParse(parts[0], out int row) && int.TryParse(parts[1], out int column))
					{
						game.OnSelect(row, column);
					}
					else
					{
						var commandAndRow = parts[0].Split(' ');
						if (commandAndRow.Length == 2 && int.TryParse(commandAndRow[1], out row) && int.TryParse(parts[1], out column))
							game.OnFlag(row, column);
						else
							parseError = true;
					}
				}
				else
				{
					parseError = true;
				}

				if (parseError)
					Console.WriteLine("Input must be in the form <row, column> or <flag row, column> to flag a cell.");
			}

			if (game.Status == GameStatus.Success)
				Console.WriteLine("Nice one!");
			else
				Console.WriteLine("Better luck next time.");

			PrintBoard(game);
		}

		private static void PrintBoard(MineSweeperGame game)
		{
			var stringBuilder = new StringBuilder();
			var (rows, _) = game.BoardSize;
			var currentColumn = 0;
			foreach (var cell in game.GetAllCells())
			{
				if (game.Status != GameStatus.InProgress)
				{
					cell.IsFlipped = true;
					cell.IsFlagged = false;
				}

				stringBuilder.Append(cell.ToString());

				if (currentColumn == rows - 1) {
					stringBuilder.AppendLine();
					currentColumn = 0;
				}
				else
				{
					currentColumn++;
				}
			}

			Console.WriteLine(stringBuilder);
		}
	}
}
