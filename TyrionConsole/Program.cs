using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldDomination;
using ChessGame;

namespace TyrionConsole {
	class Program {
		static void Main(string[] args) {
			Chess chess = new Chess();
			ShowBoard(chess.Board);

			while (!chess.Board.InCheckmate(chess)) {
				Console.WriteLine("Turn: " + chess.Turn.ToString());
				String command = Console.ReadLine();
				int sx = int.Parse(command[0].ToString());
				int sy = int.Parse(command[1].ToString());
				int dx = int.Parse(command[2].ToString());
				int dy = int.Parse(command[3].ToString());
				Console.WriteLine(chess.Move(new Position(sx, sy), new Position(dx, dy)));
				ShowBoard(chess.Board);
			}
		}

		private static void ShowBoard(Board board) {
			for (int y = board.Height - 1; y >= 0; y--) {
				String line = "";
				for (int x = 0; x < board.Width; x++) {
					Piece piece = board.GetPiece(new Position(x, y));
					if (piece != null) {
						line += piece.PieceType.ToString().Substring(0, 1);
					}
					else {
						line += "O";
					}
				}
				Console.WriteLine(line);
			}
		}
	}
}
