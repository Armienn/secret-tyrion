using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess {
	public class Chess {
		public Board Board;
		public Colour Turn;

		public Chess() {
			Board = new Board();
		}

		public bool Move(Position source, Position destination) {
			Piece movingPiece = Board.GetPiece(source);
			Piece defensivePiece = Board.GetPiece(destination);
			if (movingPiece == null) {
				return false;
			}
			if (movingPiece.Colour == defensivePiece.Colour) {
				return false;
			}
			if (movingPiece.Colour != Turn) {
				return false;
			}
			if (!movingPiece.Move(Board, destination)) {
				return false;
			}
			Turn = Turn == Colour.White ? Colour.Black : Colour.White;
			return true;
		}
	}
}
