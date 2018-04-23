using System;

namespace MineSweeper
{
	public class MineSweeperSafeCell : MineSweeperCell
	{
		public int NumberAdjacentBombs { get; set; }

		public override void OnSelect()
		{
			IsFlipped = true;
		}

		public override string ToStringCore()
		{
			return $"  {NumberAdjacentBombs}  ";
		}
	}
}
