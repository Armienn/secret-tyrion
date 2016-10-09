using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame {
	public class Board {
		public List<Piece> Pieces = new List<Piece>();
		public int Width = 8;
		public int Height = 8;

		public Board() {
			Pieces.Add(new Piece(Colour.White, PieceType.Rook, 0, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Knight, 1, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Bishop, 2, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Queen, 3, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.King, 4, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Bishop, 5, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Knight, 6, 0));
			Pieces.Add(new Piece(Colour.White, PieceType.Rook, 7, 0));
			for (int i = 0; i < 8; i++) {
				Pieces.Add(new Piece(Colour.White, PieceType.Pawn, i, 1));
			}

			Pieces.Add(new Piece(Colour.Black, PieceType.Rook, 0, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Knight, 1, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Bishop, 2, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.Queen, 3, 7));
			Pieces.Add(new Piece(Colour.Black, PieceType.King, 4, 7));
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

		public bool InCheckmate(Chess chess) {
			if (InCheck(chess.Turn)) {
				return !CanPlayerMove(chess);
			}
			if (!CanPieceMove(chess, GetKing(chess.Turn))) {
				return !CanPlayerMove(chess);
			}
			return false;
		}

		public bool InCheck(Colour colour) {
			foreach (Piece piece in Pieces) {
				if (piece.Colour != colour) {
					if (piece.IsPieceTypeMoveLegal(this, GetKing(colour).Position)) {
						return true;
					}
				}
			}
			return false;
		}

		private Piece GetKing(Colour colour) {
			foreach (Piece piece in Pieces) {
				if (piece.PieceType == PieceType.King && piece.Colour == colour) {
					return piece;
				}
			}
			return null;
		}

		private bool CanPlayerMove(Chess chess) {
			foreach (Piece piece in Pieces) {
				if (piece.Colour == chess.Turn) {
					if (CanPieceMove(chess, piece)) {
						return true;
					}
				}
			}
			return false;
		}

		private bool CanPieceMove(Chess chess, Piece piece) {
			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Width; x++) {
					if (piece.IsMoveLegal(chess, new Position(x, y))) {
						return true;
					}
				}
			}
			return false;
		}
	}
}
