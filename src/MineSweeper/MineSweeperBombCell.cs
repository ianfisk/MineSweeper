using System;

namespace MineSweeper
{
	public sealed class MineSweeperBombCell : MineSweeperCell
	{
		public override void OnSelect()
		{
			throw new ExplosionException();
		}

		public override string ToStringCore()
		{
			return "  \ud83d\udca5  ";
		}
	}
}
