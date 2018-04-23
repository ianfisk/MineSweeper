namespace MineSweeper
{
	public interface IGrid<T>
	{
		int Rows { get; }
		int Columns { get; }
		void Resize(int rows, int columns);
		T Get(int row, int column);
		void Set(int row, int column, T value);
	}
}
