using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeper
{
	public sealed class MineSweeperGame
	{
		readonly IGrid<MineSweeperCell> _grid;

		int _numberBombs;
		int _numberFlippedOrFlaggedCells;

		public MineSweeperGame()
		{
			_grid = new Grid<MineSweeperCell>();
		}

		public (int rows, int columns) BoardSize => (_grid.Rows, _grid.Columns);

		public GameStatus Status { get; private set; }

		public IEnumerable<MineSweeperCell> GetAllCells()
		{
			foreach ((var row, var col) in GetAllCellPositions())
				yield return _grid.Get(row, col);
		}

		public void InitializeGame(int rows, int columns, int numberBombs)
		{
			_grid.Resize(rows, columns);
			_numberFlippedOrFlaggedCells = 0;
			_numberBombs = numberBombs;

			AddBombsToGrid();
			ShuffleGrid();
			CountAdjacentBombs();
		}

		public void OnFlag(int row, int column)
		{
			ExecuteCellAction(() =>
			{
				var cell = _grid.Get(row, column);
				if (cell.IsFlipped)
					return;

				cell.OnFlag();

				_numberFlippedOrFlaggedCells++;
				if (_numberFlippedOrFlaggedCells == _grid.Rows * _grid.Columns)
					Status = GameStatus.Success;
			});
		}

		public void OnSelect(int row, int column)
		{
			ExecuteCellAction(() =>
			{
				var cell = _grid.Get(row, column);
				if (cell.IsFlipped || cell.IsFlagged)
					return;

				cell.OnSelect();

				_numberFlippedOrFlaggedCells++;
				if (_numberFlippedOrFlaggedCells == _grid.Rows * _grid.Columns)
					Status = GameStatus.Success;

				if (cell is MineSweeperSafeCell safeCell && safeCell.NumberAdjacentBombs == 0)
				{
					foreach (var (r, c, _) in GetAdjacentCells(row, column).Where(x => x.cell is MineSweeperSafeCell))
						OnSelect(r, c);
				}
			});
		}

		private void AddBombsToGrid()
		{
			for (int i = 0; i < _numberBombs; i++)
				_grid.Set(GetRow(i), GetColumn(i), new MineSweeperBombCell());	
		}

		private void ShuffleGrid()
		{
			// fisher-yates shuffle
			var random = new Random();
			var totalCells = _grid.Rows * _grid.Columns;
			for (int i = 0; i < totalCells; i++)
			{
				var randomIndex = random.Next(i, totalCells);
				var cellFromBack = _grid.Get(GetRow(randomIndex), GetColumn(randomIndex)) ?? new MineSweeperSafeCell();
				var cellFromFront = _grid.Get(GetRow(i), GetColumn(i)) ?? new MineSweeperSafeCell();

				_grid.Set(GetRow(randomIndex), GetColumn(randomIndex), cellFromFront);
				_grid.Set(GetRow(i), GetColumn(i), cellFromBack);
			}
		}

		private void CountAdjacentBombs()
		{
			foreach (var (row, col) in GetAllCellPositions())
			{
				var currentCell = _grid.Get(row, col);
				if (currentCell is MineSweeperBombCell)
				{
					foreach (var (_, _, cell) in GetAdjacentCells(row, col))
					{
						if (cell is MineSweeperSafeCell safeCell)
							safeCell.NumberAdjacentBombs += 1;
					}
				}
			}
		}

		private IEnumerable<(int row, int column, MineSweeperCell cell)> GetAdjacentCells(int row, int col)
		{
			for (int i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, _grid.Rows - 1); i++)
			{
				for (int j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, _grid.Columns - 1); j++)
				{
					if (i == row && j == col) continue;
					yield return (i, j, _grid.Get(i, j));
				}
			}
		}

		private IEnumerable<(int, int)> GetAllCellPositions()
		{
			for (int i = 0; i < _grid.Rows; i++)
			{
				for (int j = 0; j < _grid.Columns; j++)
					yield return (i, j);
			}
		}

		private int GetRow(int flatIndex)
		{
			return (int) Math.Floor((double) flatIndex / _grid.Rows);
		}

		private int GetColumn(int flatIndex)
		{
			return flatIndex % _grid.Columns;
		}

		private void ExecuteCellAction(Action cellAction)
		{
			try
			{
				cellAction();
			}
			catch (ExplosionException)
			{
				Console.WriteLine("Failed");
				Status = GameStatus.Failure;
			}
		}
	}
}
