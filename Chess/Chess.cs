using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame {
	public class Chess {
		public Board Board;
		public Colour Turn;

		public Chess() {
			Board = new Board();
			Turn = Colour.White;
		}

		public bool IsMoveLegal(Position source, Position destination) {
			Piece movingPiece = Board.GetPiece(source);
			if (movingPiece == null) {
				return false;
			}
			return movingPiece.IsMoveLegal(this, destination);
		}

		public bool Move(Position source, Position destination) {
			Piece movingPiece = Board.GetPiece(source);
			if (movingPiece == null) {
				return false;
			}
			return movingPiece.Move(this, destination);
		}
	}
}
