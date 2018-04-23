namespace MineSweeper
{
	public abstract class MineSweeperCell
	{
		public bool IsFlagged { get; set; }

		public bool IsFlipped { get; set; }
	
		public abstract void OnSelect();

		public virtual string ToStringCore()
		{
			return string.Empty;
		}

		public void OnFlag()
		{
			IsFlagged = !IsFlagged;
		}

		public override string ToString()
		{
			if (IsFlagged)
				return "  \ud83d\udea9  ";

			if (!IsFlipped)
				return "  -  ";

			return ToStringCore();
		}
	}
}
