using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess {
	public class Position {
		public int X;
		public int Y;

		public Position(int x, int y) {
			X = x;
			Y = y;
		}

		public bool Equals(Position position) {
			return X == position.X && Y == position.Y;
		}
	}
}
