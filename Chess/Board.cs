using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess {
	public class Board {
		public List<Piece> Pieces = new List<Piece>();
		public int Width = 8;
		public int Height = 8;

		public Board() {
			Pieces.Add(new Piece(Colour.White, PieceType.Rook, 0, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Knight, 1, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Bishop, 2, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.King, 3, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Queen, 4, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Bishop, 5, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Knight, 6, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Rook, 7, 0));
			for (int i = 0; i < 8; i++) {
				Pieces.Add(new Piece(Colour.White, PieceType.Pawn, i, 1));
			}

			Pieces.Add(new Piece(Colour.Black, PieceType.Rook, 0, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Knight, 1, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Bishop, 2, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.King, 3, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Queen, 4, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Bishop, 5, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Knight, 6, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Rook, 7, 7));
			for (int i = 0; i < 8; i++) {
				Pieces.Add(new Piece(Colour.Black, PieceType.Pawn, i, 6));
			}
		}

		public Piece GetPiece(Position position) {
			foreach (Piece piece in Pieces) {
				if (piece.Position.Equals(position)) {
					return piece;
				}
			}
			return null;
		}

		public bool InCheck(Colour colour) {
			//TODO
			return false;
		}
	}
}
