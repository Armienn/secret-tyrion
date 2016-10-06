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

		public bool Move(Board board, Position destination) {
			if (0 <= destination.X && destination.X < board.Width
				&& 0 <= destination.Y && destination.Y < board.Height) {
				return false;
			}
			switch (PieceType) {
				case PieceType.King:
					//TODO
					break;
				case PieceType.Queen:
					//TODO
					break;
				case PieceType.Bishop:
					//TODO
					break;
				case PieceType.Knight:
					if (!((Math.Abs(Position.X - destination.X) == 2 && Math.Abs(Position.Y - destination.Y) == 1)
						|| (Math.Abs(Position.X - destination.X) == 1 && Math.Abs(Position.Y - destination.Y) == 2))) {
						return false;
					}
					break;
				case PieceType.Rook:
					if (Position.X == destination.X || Position.Y == destination.Y) {
						if (Position.X - destination.X < 0) {
							for (int i = Position.X - 1; i > destination.X; i--) {
								if (board.GetPiece(new Position(i, Position.Y)) != null) {
									return false;
								}
							}
						}
						if (Position.X - destination.X > 0) {
							for (int i = Position.X + 1; i < destination.X; i++) {
								if (board.GetPiece(new Position(i, Position.Y)) != null) {
									return false;
								}
							}
						}
						if (Position.Y - destination.Y < 0) {
							for (int i = Position.Y - 1; i > destination.Y; i--) {
								if (board.GetPiece(new Position(Position.X, i)) != null) {
									return false;
								}
							}
						}
						if (Position.Y - destination.Y > 0) {
							for (int i = Position.Y + 1; i < destination.Y; i++) {
								if (board.GetPiece(new Position(Position.X, i)) != null) {
									return false;
								}
							}
						}
					}
					break;
				case PieceType.Pawn:
					//TODO
					break;
			}
			Piece defensivePiece = board.GetPiece(destination);
			if (defensivePiece != null) {
				board.Pieces.Remove(defensivePiece);
			}
			int X = Position.X;
			int Y = Position.Y;
			Position.X = destination.X;
			Position.Y = destination.Y;
			if (board.InCheck(Colour)) {
				Position.X = X;
				Position.Y = Y;
				if (defensivePiece != null) {
					board.Pieces.Add(defensivePiece);
				}
				return false;
			}
			return true;
		}
	}

	public enum Colour {
		White, Black
	}

	public enum PieceType {
		King, Queen, Bishop, Knight, Rook, Pawn
	}
}
