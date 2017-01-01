using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame {
	public class Board {
		public List<Piece> Pieces = new List<Piece>();
		public List<Piece> DeadPieces = new List<Piece>();
		public int Width = 8;
		public int Height = 8;

		public Board() {
			ResetToDefaultBoard();
		}

		public Board(Board newBoard) {
			Pieces = clonePieceList(newBoard.Pieces);
			DeadPieces = clonePieceList(newBoard.DeadPieces);
			Width = newBoard.Width;
			Height = newBoard.Height;
		}

		public Board(int width, int height) {
			Width = width;
			Height = height;
		}

		static private List<Piece> clonePieceList(List<Piece> pieceList) {
			List<Piece> pieceListClone = new List<Piece>();
			foreach (Piece piece in pieceList) {
				pieceListClone.Add(new Piece(piece));
			}
			return pieceListClone;
		}

		/// <summary>
		/// Resets the board and sets up the default chess board if the height and width of the board i 8 or more.
		/// </summary>
		public void ResetToDefaultBoard() {
			Pieces.Clear();
			DeadPieces.Clear();
			Width = 8;
			Height = 8;

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

		public bool KingIsInCheck(Piece piece) {
			//Checks if any enemy piece (except the king) can move to this kings position
			foreach (Piece enemyPiece in Pieces) {
				if (enemyPiece.Colour != piece.Colour && enemyPiece.PieceType != PieceType.King) {
					Piece unusedReturnValue;
					if (enemyPiece.MoveIsLegal(this, piece.Position, out unusedReturnValue)) {
						return true;
					}
				}
			}
			return false;
		}

		public bool KingIsInCheck(Colour colour) {
			Piece king = GetKing(colour);
			//Return false if there is no king
			if (king == null) {
				return false;
			}
			return KingIsInCheck(king);
		}

		private Piece GetKing(Colour colour) {
			//Finds the king of the given colour. Assumes that there is only one king
			foreach (Piece piece in Pieces) {
				if (piece.PieceType == PieceType.King && piece.Colour == colour) {
					return piece;
				}
			}
			return null;
		}
	}
}
