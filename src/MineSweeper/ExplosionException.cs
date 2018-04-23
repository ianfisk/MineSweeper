using System;

namespace MineSweeper
{
	public sealed class ExplosionException : Exception
	{
		public ExplosionException()
		{
		}

		public ExplosionException(string message)
			: base(message)
		{
		}

		public ExplosionException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
