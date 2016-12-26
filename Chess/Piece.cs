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

		public Piece(Colour colour, PieceType pieceType, Position position)
			: this(colour, pieceType, position.X, position.Y) { }

		public Piece(Colour colour, PieceType pieceType, int x, int y) {
			Colour = colour;
			PieceType = pieceType;
			Position = new Position(x, y);
			HasMoved = false;
		}

		/// <summary>
		/// Moves the piece to destination if the move legal
		/// </summary>
		/// <param name="chess"></param>
		/// <param name="destination">The destination of the move</param>
		/// <returns>Returns true if the move succeeded</returns>
		public bool Move(Chess chess, Position destination) {
			//Checks if requested move is legal
			if (IsMoveLegal(chess, destination)) {
				Board board = chess.Board;
				Piece defensivePiece = board.GetPiece(destination);

				//Moves rook if King uses Castling
				CastlingMove(board, destination);

				//Promotes pawn to queen
				if (PieceType == PieceType.Pawn && (destination.Y == 0 || destination.Y == board.Height - 1)) {
					PieceType = PieceType.Queen;
				}

				//Removes enemy and moves piece
				if (defensivePiece != null) {
					board.Pieces.Remove(defensivePiece);
				}
				Position.X = destination.X;
				Position.Y = destination.Y;
				HasMoved = true;

				//Changes the turn
				chess.Turn = chess.Turn == Colour.White ? Colour.Black : Colour.White;
				return true;
			}
			//Returns false if move is illegal
			return false;

		}

		public bool IsMoveLegal(Chess chess, Position destination) {
			Board board = chess.Board;
			Piece defensivePiece = board.GetPiece(destination);

			//Checks if it's the right turn
			if (Colour != chess.Turn) {
				return false;
			}

			//Checks if destination tile is occupied by one of current players pieces
			if (defensivePiece != null && Colour == defensivePiece.Colour) {
				return false;
			}

			//Checks if destination is on the board
			if (0 <= destination.X && destination.X < board.Width
				&& 0 <= destination.Y && destination.Y < board.Height) {
				return false;
			}

			//Checks if move is valid for the given piece type
			if (!IsMoveLegal2(board, destination)) {
				return false;
			}

			//Checks if current players king is in check after move
			Position[] rookPositions = CastlingMove(board, destination);
			int X = Position.X;
			int Y = Position.Y;

			if (defensivePiece != null) {
				board.Pieces.Remove(defensivePiece);
			}
			Position.X = destination.X;
			Position.Y = destination.Y;

			bool check = board.InCheck(Colour);

			if (rookPositions[0] != null) {
				board.GetPiece(rookPositions[1]).Position.X = rookPositions[0].X;
			}
			Position.X = X;
			Position.Y = Y;
			if (defensivePiece != null) {
				board.Pieces.Add(defensivePiece);
			}
			return !check;
		}

		public bool IsMoveLegal2(Board board, Position destination) {
			switch (PieceType) {
				case PieceType.King:
					return IsKingMoveLegal(board, destination);
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

		private bool IsKingMoveLegal(Board board, Position destination) {
			//Checks if the step size is exactly one
			if (Math.Abs(Position.X - destination.X) == 1 || Math.Abs(Position.Y - destination.Y) == 1) {
				return true;
			}

			//The rest in this method is in case of castling
			if (HasMoved || board.IsInCheck(this)) {
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
			if (Math.Abs(Position.X - destination.X) == Math.Abs(Position.Y - destination.Y)) {
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
			return false;
		}

		private bool IsKnightMoveLegal(Board board, Position destination) {
			if ((Math.Abs(Position.X - destination.X) == 2 && Math.Abs(Position.Y - destination.Y) == 1)
				|| (Math.Abs(Position.X - destination.X) == 1 && Math.Abs(Position.Y - destination.Y) == 2)) {
				return true;
			}
			return false;
		}

		private bool IsRookMoveLegal(Board board, Position destination) {
			if (Position.X == destination.X || Position.Y == destination.Y) {
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
			return false;
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
			}
			return false;
		}

		private Position[] CastlingMove(Board board, Position destination) {
			Position[] rookPositions = new Position[2];
			if (PieceType == PieceType.King) {
				Position rookOriginalPosition = null;
				Position rookPosition = null;
				if (Colour == Colour.White) {
					if (Position.X + 2 == destination.X) {
						rookOriginalPosition = new Position(board.Width - 1, 0);
						rookPosition = new Position(Position.X + 1, 0);
						board.GetPiece(rookOriginalPosition).Position.X = Position.X + 1;
					}
					else if (Position.X - 2 == destination.X) {
						rookOriginalPosition = new Position(0, 0);
						rookPosition = new Position(Position.X - 1, 0);
						board.GetPiece(rookOriginalPosition).Position.X = Position.X - 1;
					}
				}
				else {
					if (Position.X + 2 == destination.X) {
						rookOriginalPosition = new Position(board.Width - 1, board.Height - 1);
						rookPosition = new Position(Position.X + 1, board.Height - 1);
						board.GetPiece(rookOriginalPosition).Position.X = Position.X + 1;
					}
					else if (Position.X - 2 == destination.X) {
						rookOriginalPosition = new Position(0, board.Height - 1);
						rookPosition = new Position(Position.X - 1, board.Height - 1);
						board.GetPiece(rookOriginalPosition).Position.X = Position.X - 1;
					}
				}
				rookPositions[0] = rookOriginalPosition;
				rookPositions[1] = rookPosition;
			}
			return rookPositions;
		}

		public void Move2(Board board, Position destination) {
			Piece defensivePiece = board.GetPiece(destination);
			if (defensivePiece != null) {
				board.Pieces.Remove(defensivePiece);
				board.DeadPieces.Add(defensivePiece);
			}
			Position.X = destination.X;
			Position.Y = destination.Y;
			HasMoved = true;
		}
	}

	public enum Colour {
		White, Black
	}

	public enum PieceType {
		King, Queen, Bishop, Knight, Rook, Pawn
	}
}
