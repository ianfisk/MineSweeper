using System;

namespace MineSweeper
{
	public sealed class Grid<T> : IGrid<T>
	{
		int _rows;
		int _cols;
		T[] _grid;

		public Grid()
			: this(0, 0)
		{
		}

		public Grid(int rows, int columns)
		{
			Resize(rows, columns);
		}

		public int Rows => _rows;

		public int Columns => _cols;

		public void Resize(int rows, int columns)
		{
			if (rows < 0)
				throw new ArgumentOutOfRangeException(nameof(rows));
			if (columns < 0)
				throw new ArgumentOutOfRangeException(nameof(columns));

			_rows = rows;
			_cols = columns;
			_grid = new T[_rows * _cols];
		}

		public T Get(int row, int column)
		{
			if (row < 0 || row >= _rows)
				throw new ArgumentOutOfRangeException(nameof(row));
			if (column < 0 || column >= _cols)
				throw new ArgumentOutOfRangeException(nameof(column));
			
			var index = row * _rows + column;
			return _grid[index];
		}

		public void Set(int row, int column, T value)
		{
			if (row < 0 || row >= _rows)
				throw new ArgumentOutOfRangeException(nameof(row));
			if (column < 0 || column >= _cols)
				throw new ArgumentOutOfRangeException(nameof(column));

			var index = row * _rows + column;
			_grid[index] = value;
		}
	}
}
