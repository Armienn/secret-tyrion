using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess {
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
				if (PieceType == PieceType.King) {
					Position rookPosition;
					if (Colour == Colour.White) {
						if (Position.X + 2 == destination.X) {
							rookPosition = new Position(board.Width - 1, 0);
							board.GetPiece(rookPosition).Position.X = Position.X + 1;
						}
						else if (Position.X - 2 == destination.X) {
							rookPosition = new Position(0, 0);
							board.GetPiece(rookPosition).Position.X = Position.X - 1;
						}
					}
					else {
						if (Position.X + 2 == destination.X) {
							rookPosition = new Position(board.Width - 1, board.Height - 1);
							board.GetPiece(rookPosition).Position.X = Position.X + 1;
						}
						else if (Position.X - 2 == destination.X) {
							rookPosition = new Position(0, board.Height - 1);
							board.GetPiece(rookPosition).Position.X = Position.X - 1;
						}
					}
				}

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
			if (Colour == defensivePiece.Colour) {
				return false;
			}

			//Checks if destination is on the board
			if (0 <= destination.X && destination.X < board.Width
				&& 0 <= destination.Y && destination.Y < board.Height) {
				return false;
			}

			//Checks if move is valid for the given type
			switch (PieceType) {
				case PieceType.King:
					if (!IsKingMoveLegal(board, destination)) {
						return false;
					}
					break;
				case PieceType.Queen:
					if (!IsQueenMoveLegal(board, destination)) {
						return false;
					}
					break;
				case PieceType.Bishop:
					if (!IsBishopMoveLegal(board, destination)) {
						return false;
					}
					break;
				case PieceType.Knight:
					if (!IsKnightMoveLegal(board, destination)) {
						return false;
					}
					break;
				case PieceType.Rook:
					if (!IsRookMoveLegal(board, destination)) {
						return false;
					}
					break;
				case PieceType.Pawn:
					if (!IsPawnMoveLegal(board, destination)) {
						return false;
					}
					break;
			}

			//Checks if current players king is in check after move

			//skal ændres så vi også husker at ændre tårnet frem of tilbage (bonde/dronninge checket er ligegyldigt)
			//Vi kan støde i problemer med InCheck afhængigt af hvordan den laves
			if (defensivePiece != null) {
				board.Pieces.Remove(defensivePiece);
			}
			int X = Position.X;
			int Y = Position.Y;
			Position.X = destination.X;
			Position.Y = destination.Y;

			bool check = board.InCheck(Colour);

			Position.X = X;
			Position.Y = Y;
			if (defensivePiece != null) {
				board.Pieces.Add(defensivePiece);
			}
			return !check;
		}

		private bool IsKingMoveLegal(Board board, Position destination) {
			if (Math.Abs(Position.X - destination.X) == 1 || Math.Abs(Position.Y - destination.Y) == 1) {
				return true;
			}

			if (!HasMoved) {
				if (Colour == Colour.White) {
					if (Position.X - destination.X == 2 || Position.Y == destination.Y) {
						Piece cornerPiece = board.GetPiece(new Position(board.Width - 1, 0));
						if (cornerPiece.PieceType != null
							&& cornerPiece.PieceType == PieceType.Rook
							&& !cornerPiece.HasMoved) {
							for (int i = Position.X + 1; i < cornerPiece.Position.X; i++) {
								if (board.GetPiece(new Position(i, 0)) != null) {
									return false;
								}
							}
							return true;
						}
					}
					else if (Position.X - destination.X == -2 || Position.Y == destination.Y) {
						Piece cornerPiece = board.GetPiece(new Position(0, 0));
						if (cornerPiece.PieceType != null
							&& cornerPiece.PieceType == PieceType.Rook
							&& !cornerPiece.HasMoved) {
							for (int i = Position.X - 1; i > cornerPiece.Position.X; i--) {
								if (board.GetPiece(new Position(i, 0)) != null) {
									return false;
								}
							}
							return true;
						}
					}
				}
				else {
					if (Position.X - destination.X == 2 || Position.Y == destination.Y) {
						Piece cornerPiece = board.GetPiece(new Position(board.Width - 1, board.Height - 1));
						if (cornerPiece.PieceType != null
							&& cornerPiece.PieceType == PieceType.Rook
							&& !cornerPiece.HasMoved) {
							for (int i = Position.X + 1; i < cornerPiece.Position.X; i++) {
								if (board.GetPiece(new Position(i, board.Height - 1)) != null) {
									return false;
								}
							}
							return true;
						}
					}
					else if (Position.X - destination.X == -2 || Position.Y == destination.Y) {
						Piece cornerPiece = board.GetPiece(new Position(0, board.Height - 1));
						if (cornerPiece.PieceType != null
							&& cornerPiece.PieceType == PieceType.Rook
							&& !cornerPiece.HasMoved) {
							for (int i = Position.X - 1; i > cornerPiece.Position.X; i--) {
								if (board.GetPiece(new Position(i, board.Height - 1)) != null) {
									return false;
								}
							}
							return true;
						}
					}
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
					Position tilesBetween = new Position(Position.X + 1, Position.Y + 1);
					while (!tilesBetween.Equals(destination)) {
						if (board.GetPiece(tilesBetween) != null) {
							return false;
						}
						tilesBetween.X++;
						tilesBetween.Y++;
					}
				}
				else if ((Position.X - destination.X) > 0 && (Position.Y - destination.Y) < 0) {
					Position tilesBetween = new Position(Position.X + 1, Position.Y - 1);
					while (!tilesBetween.Equals(destination)) {
						if (board.GetPiece(tilesBetween) != null) {
							return false;
						}
						tilesBetween.X++;
						tilesBetween.Y--;
					}
				}
				else if ((Position.X - destination.X) < 0 && (Position.Y - destination.Y) < 0) {
					Position tilesBetween = new Position(Position.X - 1, Position.Y - 1);
					while (!tilesBetween.Equals(destination)) {
						if (board.GetPiece(tilesBetween) != null) {
							return false;
						}
						tilesBetween.X--;
						tilesBetween.Y--;
					}
				}
				else if ((Position.X - destination.X) < 0 && (Position.Y - destination.Y) > 0) {
					Position tilesBetween = new Position(Position.X - 1, Position.Y + 1);
					while (!tilesBetween.Equals(destination)) {
						if (board.GetPiece(tilesBetween) != null) {
							return false;
						}
						tilesBetween.X--;
						tilesBetween.Y++;
					}
				}
				return true;
			}
			else {
				return false;
			}
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
					for (int i = Position.X - 1; i > destination.X; i--) {
						if (board.GetPiece(new Position(i, Position.Y)) != null) {
							return false;
						}
					}
				}
				else if (Position.X - destination.X > 0) {
					for (int i = Position.X + 1; i < destination.X; i++) {
						if (board.GetPiece(new Position(i, Position.Y)) != null) {
							return false;
						}
					}
				}
				else if (Position.Y - destination.Y < 0) {
					for (int i = Position.Y - 1; i > destination.Y; i--) {
						if (board.GetPiece(new Position(Position.X, i)) != null) {
							return false;
						}
					}
				}
				else if (Position.Y - destination.Y > 0) {
					for (int i = Position.Y + 1; i < destination.Y; i++) {
						if (board.GetPiece(new Position(Position.X, i)) != null) {
							return false;
						}
					}
				}
				return true;
			}
			else {
				return false;
			}
		}

		private bool IsPawnMoveLegal(Board board, Position destination) {
			if (Colour == Colour.White) {
				if (!HasMoved) {
					if (Position.X == destination.X && Position.Y + 2 == destination.Y) {
						if (board.GetPiece(destination) == null) {
							return true;
						}
					}
				}
				if (Position.X == destination.X && Position.Y + 1 == destination.Y) {
					if (board.GetPiece(destination) == null) {
						return true;
					}
				}
				if (Math.Abs(Position.X - destination.X) == 1 && Position.Y + 1 == destination.Y) {
					if (board.GetPiece(destination) != null) {
						return true;
					}
				}
			}
			else {
				if (!HasMoved) {
					if (Position.X == destination.X && Position.Y - 2 == destination.Y) {
						if (board.GetPiece(destination) == null) {
							return true;
						}
					}
				}
				if (Position.X == destination.X && Position.Y - 1 == destination.Y) {
					if (board.GetPiece(destination) == null) {
						return true;
					}
				}
				if (Math.Abs(Position.X - destination.X) == 1 && Position.Y - 1 == destination.Y) {
					if (board.GetPiece(destination) != null) {
						return true;
					}
				}
			}
			return false;
		}
	}

	public enum Colour {
		White, Black
	}

	public enum PieceType {
		King, Queen, Bishop, Knight, Rook, Pawn
	}
}
