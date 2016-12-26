using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame {
	public class Chess {
		public Board Board;
		public Colour Turn = Colour.White;

		public Chess() {
			Board = new Board();
		}

		public bool IsMoveLegal(int sourceX, int sourceY, int destinationX, int destinationY) {
			Position source = new Position(sourceX, sourceY);
			Position destination = new Position(destinationX, destinationY);
			Piece movingPiece = Board.GetPiece(source);
			if (movingPiece == null) {
				return false;
			}
			return movingPiece.IsMoveLegal(this, destination);
		}

		public bool Move(int sourceX, int sourceY, int destinationX, int destinationY) {
			Position source = new Position(sourceX, sourceY);
			Position destination = new Position(destinationX, destinationY);
			Piece movingPiece = Board.GetPiece(source);
			if (movingPiece == null) {
				return false;
			}
			return movingPiece.Move(this, destination);
		}

		public bool Move2(int sourceX, int sourceY, int destinationX, int destinationY) {
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

			//Checks if the move is legal
			if (!movingPiece.IsMoveLegal2(Board, destination)) {
				return false;
			}

			//Moves the piece to destination and removes the previous piece at the destination if is was any
			movingPiece.Move2(Board, destination);

			//In case a king uses castle
			//TODO

			//In case a pawn reaches rear line
			//TODO

			Turn = Turn == Colour.White ? Colour.Black : Colour.White;
			return true;
		}
	}
}
