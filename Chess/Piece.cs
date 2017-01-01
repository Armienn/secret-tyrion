using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame {
	public class Piece {
		public Colour Colour { get; private set; }
		public PieceType PieceType { get; private set; }
		public Position Position { get; private set; }
		public bool HasMoved { get; private set; }

		public Piece(Colour colour, PieceType pieceType, int x, int y) {
			Colour = colour;
			PieceType = pieceType;
			Position = new Position(x, y);
			HasMoved = false;
		}

		public Piece(Colour colour, PieceType pieceType, Position position)
			: this(colour, pieceType, position.X, position.Y) { }

		public Piece(Piece newPiece)
			: this(newPiece.Colour, newPiece.PieceType, newPiece.Position.X, newPiece.Position.Y) { }

		public bool MoveIsLegal(Board board, Position destination, out Piece castlingRook) {
			castlingRook = null;
			switch (PieceType) {
				case PieceType.King:
					return IsKingMoveLegal(board, destination, out castlingRook);
				case PieceType.Queen:
					return IsQueenMoveLegal(board, destination);
				case PieceType.Bishop:
					return IsBishopMoveLegal(board, destination);
				case PieceType.Knight:
					return IsKnightMoveLegal(board, destination);
				case PieceType.Rook:
					return IsRookMoveLegal(board, destination);
				case PieceType.Pawn:
					return IsPawnMoveLegal(board, destination);
				default:
					return false;
			}
		}

		private bool IsKingMoveLegal(Board board, Position destination, out Piece castlingRook) {
			castlingRook = null;

			//Checks if the step size is exactly one
			if (Math.Abs(Position.X - destination.X) <= 1 && Math.Abs(Position.Y - destination.Y) <= 1) {
				return true;
			}

			//The rest in this method is in case of castling
			if (HasMoved || board.KingIsInCheck(this)) {
				return false;
			}
			if (Colour == Colour.White) {
				//If the castling is done with the rook to the left of the white king
				if (Position.X - destination.X == 2 && Position.Y == destination.Y) {
					Piece cornerPiece = board.GetPiece(new Position(0, 0));
					//Checks if the corner piece is a rook of the correct colour and it has not moved
					if (cornerPiece == null
						|| cornerPiece.Colour != Colour.White
						|| cornerPiece.PieceType != PieceType.Rook
						|| cornerPiece.HasMoved) {
						return false;
					}
					//Checks if there is any pieces between the king and the rook
					for (int i = Position.X - 1; i > cornerPiece.Position.X; i--) {
						if (board.GetPiece(new Position(i, 0)) != null) {
							return false;
						}
					}
					//Checks if the king would be in check if it was standing between current position and the destination
					Position.X--;
					if (board.KingIsInCheck(this)) {
						Position.X++;
						return false;
					}
					Position.X++;
					//Returns true and also return the rook that should move
					castlingRook = cornerPiece;
					return true;
				}
				//If the castling is done with the rook to the right of the white king
				else if (Position.X - destination.X == -2 && Position.Y == destination.Y) {
					Piece cornerPiece = board.GetPiece(new Position(board.Width - 1, 0));
					//Checks if the corner piece is a rook of the correct colour and it has not moved
					if (cornerPiece == null
						|| cornerPiece.Colour != Colour.White
						|| cornerPiece.PieceType != PieceType.Rook
						|| cornerPiece.HasMoved) {
						return false;
					}
					//Checks if there is any pieces between the king and the rook
					for (int i = Position.X + 1; i < cornerPiece.Position.X; i++) {
						if (board.GetPiece(new Position(i, 0)) != null) {
							return false;
						}
					}
					//Checks if the king would be in check if it was standing between current position and the destination
					Position.X++;
					if (board.KingIsInCheck(this)) {
						Position.X--;
						return false;
					}
					Position.X--;
					//Returns true and also return the rook that should move
					castlingRook = cornerPiece;
					return true;
				}
			}
			else {
				//If the castling is done with the rook to the left of the black king
				if (Position.X - destination.X == 2 && Position.Y == destination.Y) {
					Piece cornerPiece = board.GetPiece(new Position(0, board.Height - 1));
					//Checks if the corner piece is a rook of the correct colour and it has not moved
					if (cornerPiece == null
						|| cornerPiece.Colour != Colour.Black
						|| cornerPiece.PieceType != PieceType.Rook
						|| cornerPiece.HasMoved) {
						return false;
					}
					//Checks if there is any pieces between the king and the rook
					for (int i = Position.X - 1; i > cornerPiece.Position.X; i--) {
						if (board.GetPiece(new Position(i, board.Height - 1)) != null) {
							return false;
						}
					}
					//Checks if the king would be in check if it was standing between current position and the destination
					Position.X--;
					if (board.KingIsInCheck(this)) {
						Position.X++;
						return false;
					}
					Position.X++;
					//Returns true and also return the rook that should move
					castlingRook = cornerPiece;
					return true;
				}
				//If the castling is done with the rook to the right of the black king
				else if (Position.X - destination.X == -2 && Position.Y == destination.Y) {
					Piece cornerPiece = board.GetPiece(new Position(board.Width - 1, board.Height - 1));
					//Checks if the corner piece is a rook of the correct colour and it has not moved
					if (cornerPiece == null
						|| cornerPiece.Colour != Colour.Black
						|| cornerPiece.PieceType != PieceType.Rook
						|| cornerPiece.HasMoved) {
						return false;
					}
					//Checks if there is any pieces between the king and the rook
					for (int i = Position.X + 1; i < cornerPiece.Position.X; i++) {
						if (board.GetPiece(new Position(i, board.Height - 1)) != null) {
							return false;
						}
					}
					//Checks if the king would be in check if it was standing between current position and the destination
					Position.X++;
					if (board.KingIsInCheck(this)) {
						Position.X--;
						return false;
					}
					Position.X--;
					//Returns true and also return the rook that should move
					castlingRook = cornerPiece;
					return true;
				}
			}
			return false;
		}

		private bool IsQueenMoveLegal(Board board, Position destination) {
			if (IsBishopMoveLegal(board, destination) || IsRookMoveLegal(board, destination)) {
				return true;
			}
			return false;
		}

		private bool IsBishopMoveLegal(Board board, Position destination) {
			//Checks if the destination is on any of the same diagonally lines this piece
			if (Math.Abs(Position.X - destination.X) != Math.Abs(Position.Y - destination.Y)) {
				return false;
			}
			//Checks which direction it's moving and then checks for blocking pieces
			if ((Position.X - destination.X) > 0 && (Position.Y - destination.Y) > 0) {
				Position tilesBetween = new Position(Position.X - 1, Position.Y - 1);
				while (!tilesBetween.Equals(destination)) {
					if (board.GetPiece(tilesBetween) != null) {
						return false;
					}
					tilesBetween.X--;
					tilesBetween.Y--;
				}
			}
			else if ((Position.X - destination.X) > 0 && (Position.Y - destination.Y) < 0) {
				Position tilesBetween = new Position(Position.X - 1, Position.Y + 1);
				while (!tilesBetween.Equals(destination)) {
					if (board.GetPiece(tilesBetween) != null) {
						return false;
					}
					tilesBetween.X--;
					tilesBetween.Y++;
				}
			}
			else if ((Position.X - destination.X) < 0 && (Position.Y - destination.Y) < 0) {
				Position tilesBetween = new Position(Position.X + 1, Position.Y + 1);
				while (!tilesBetween.Equals(destination)) {
					if (board.GetPiece(tilesBetween) != null) {
						return false;
					}
					tilesBetween.X++;
					tilesBetween.Y++;
				}
			}
			else if ((Position.X - destination.X) < 0 && (Position.Y - destination.Y) > 0) {
				Position tilesBetween = new Position(Position.X + 1, Position.Y - 1);
				while (!tilesBetween.Equals(destination)) {
					if (board.GetPiece(tilesBetween) != null) {
						return false;
					}
					tilesBetween.X++;
					tilesBetween.Y--;
				}
			}
			return true;
		}

		private bool IsKnightMoveLegal(Board board, Position destination) {
			if ((Math.Abs(Position.X - destination.X) == 2 && Math.Abs(Position.Y - destination.Y) == 1)
				|| (Math.Abs(Position.X - destination.X) == 1 && Math.Abs(Position.Y - destination.Y) == 2)) {
				return true;
			}
			return false;
		}

		private bool IsRookMoveLegal(Board board, Position destination) {
			//Checks if the destination is on the same horisontal or vertical axis
			if (!(Position.X == destination.X || Position.Y == destination.Y)) {
				return false;
			}
			//Checks which direction it's moving and then checks for blocking pieces
			if (Position.X - destination.X < 0) {
				for (int i = Position.X + 1; i < destination.X; i++) {
					if (board.GetPiece(new Position(i, Position.Y)) != null) {
						return false;
					}
				}
			}
			else if (Position.X - destination.X > 0) {
				for (int i = Position.X - 1; i > destination.X; i--) {
					if (board.GetPiece(new Position(i, Position.Y)) != null) {
						return false;
					}
				}
			}
			else if (Position.Y - destination.Y < 0) {
				for (int i = Position.Y + 1; i < destination.Y; i++) {
					if (board.GetPiece(new Position(Position.X, i)) != null) {
						return false;
					}
				}
			}
			else if (Position.Y - destination.Y > 0) {
				for (int i = Position.Y - 1; i > destination.Y; i--) {
					if (board.GetPiece(new Position(Position.X, i)) != null) {
						return false;
					}
				}
			}
			return true;
		}

		private bool IsPawnMoveLegal(Board board, Position destination) {
			//Checks whether the pawn should move upward or downward
			int stepDirection;
			if (Colour == Colour.White) {
				stepDirection = 1;
			}
			else {
				stepDirection = -1;
			}
			//Checks if it can move one step forward
			if (Position.X == destination.X && Position.Y + stepDirection == destination.Y) {
				if (board.GetPiece(destination) == null) {
					return true;
				}
			}
			//Checks if it can move diagonally
			if (Math.Abs(Position.X - destination.X) == 1 && Position.Y + stepDirection == destination.Y) {
				if (board.GetPiece(destination) != null) {
					return true;
				}
			}
			//Checks if it can move two steps from spawn
			if (!HasMoved) {
				if (Position.X == destination.X && Position.Y + (2 * stepDirection) == destination.Y) {
					Position tilebetween = new Position(Position.X, Position.Y + stepDirection);
					if (board.GetPiece(destination) == null && board.GetPiece(tilebetween) == null) {
						return true;
					}
				}
				//TODO - en passant
			}
			return false;
		}

		public void Move(Board board, Position destination, Piece castlingRook) {
			//Removes the piece at the destination if there is any
			Piece pieceAtDestination = board.GetPiece(destination);
			if (pieceAtDestination != null) {
				board.Pieces.Remove(pieceAtDestination);
				board.DeadPieces.Add(pieceAtDestination);
			}

			//Moves the piece
			Position.X = destination.X;
			Position.Y = destination.Y;
			HasMoved = true;

			//Promotes the pawn to queen if it on the rear line
			TryPromotePawn(board);

			//Moves rook if king uses castling
			//TODO
			if (castlingRook != null) {
				MoveCastlingRook(castlingRook);
			}
		}

		private void TryPromotePawn(Board board) {
			//Checks if it is a pawn
			if (PieceType != PieceType.Pawn) {
				return;
			}

			//Checks if the pawn is on the rear line
			if ((Colour == Colour.White && Position.Y == board.Height - 1)
				|| (Colour == Colour.Black && Position.Y == 0)) {
				PieceType = PieceType.Queen;
			}
		}

		private void MoveCastlingRook(Piece rook) {
			//Checks whether the rook is to the left or right of the king and then moves it accordingly
			if (rook.Position.X < Position.X) {
				rook.Position.X = Position.X + 1;
			}
			else {
				rook.Position.X = Position.X - 1;
			}
		}
	}

	public enum Colour {
		White, Black
	}

	public enum PieceType {
		King, Queen, Bishop, Knight, Rook, Pawn
	}
}
