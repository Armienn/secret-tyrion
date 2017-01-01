using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame {
	public class Chess {
		public Board Board;
		public Colour Turn = Colour.White;
		private Piece CastlingRook;

		public Chess() {
			Board = new Board();
		}

		public bool MoveIsLegal(int sourceX, int sourceY, int destinationX, int destinationY) {
			Position source = new Position(sourceX, sourceY);
			Position destination = new Position(destinationX, destinationY);
			Piece movingPiece = Board.GetPiece(source);
			Piece destinationPiece = Board.GetPiece(destination);

			//Checks if piece is selected
			if (movingPiece == null) {
				return false;
			}

			//Checks if it's the correct turn
			if (movingPiece.Colour != Turn) {
				return false;
			}

			//Checks if the destination is on the board
			if (destinationX < 0 || Board.Width <= destinationX
				|| destinationY < 0 || Board.Height <= destinationY) {
				return false;
			}

			//Checks if destination tile is occupied by one of current players pieces
			if (destinationPiece != null && destinationPiece.Colour == Turn) {
				return false;
			}

			//Checks if the move is legal and returns the rook to castle, if there is any
			if (!movingPiece.MoveIsLegal(Board, destination, out CastlingRook)) {
				return false;
			}

			//Makes a copy of the board
			Board boardCopy = new Board(Board);
			//Moves and removes the pieces that should be affected if the move were to be executed
			movingPiece.Move(Board, destination, CastlingRook);
			//Checks if your own king would be in check if one were to make the move
			bool a = Board.KingIsInCheck(Turn);
			//The board goes back to how it was before
			Board = boardCopy;

			return !a;
		}

		public bool Move(int sourceX, int sourceY, int destinationX, int destinationY) {
			//Checks if the move is legal
			if (!MoveIsLegal(sourceX, sourceY, destinationX, destinationY)) {
				return false;
			}

			//Moves the piece to destination and removes the previous piece at the destination if is was any
			//and promotes pawn or moves rook if king uses castling if the conditions are met
			Position source = new Position(sourceX, sourceY);
			Position destination = new Position(destinationX, destinationY);
			Piece movingPiece = Board.GetPiece(source);
			Piece destinationPiece = Board.GetPiece(destination);
			movingPiece.Move(Board, destination, CastlingRook);

			//Changes turn
			Turn = Turn == Colour.White ? Colour.Black : Colour.White;
			return true;
		}

		public bool IsInCheckmate(Colour colour) {
			//Goes through all pieces with the given colour
			foreach (Piece piece in Board.Pieces) {
				if (piece.Colour == colour) {
					//Checks if this piece can move to any position on the board
					for (int y = 0; y < Board.Height; y++) {
						for (int x = 0; x < Board.Width; x++) {
							if (MoveIsLegal(piece.Position.X, piece.Position.Y, x, y)) {
								return false;
							}
						}
					}
				}
			}
			return true;
		}
	}
}
