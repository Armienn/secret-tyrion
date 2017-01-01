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

			while (!chess.IsInCheckmate(chess.Turn)) {
				//Tells which turn it is
				Console.WriteLine("Turn: " + chess.Turn.ToString());

				//Tells whether your king is in check or not
				if (chess.Board.KingIsInCheck(chess.Turn)) {
					Console.WriteLine("Your king is in Check!");
				}
				else {
					Console.WriteLine("Your king is safe.");
				}

				//Waits for move command
				String command = Console.ReadLine();
				int sx = int.Parse(command[0].ToString());
				int sy = int.Parse(command[1].ToString());
				int dx = int.Parse(command[2].ToString());
				int dy = int.Parse(command[3].ToString());
				Console.WriteLine(chess.Move(sx, sy, dx, dy));
				Console.Clear();
				ShowBoard(chess.Board);
			}

			Console.WriteLine("\n" + chess.Turn + " lost the game");
			String waitingForUser = Console.ReadLine();
		}

		private static void ShowBoard(Board board) {
			Console.WriteLine("---------------------------------------------------------");
			for (int y = board.Height - 1; y >= 0; y--) {
				String line1 = "|";
				String line2 = "|";
				String line3 = "|";
				for (int x = 0; x < board.Width; x++) {
					Piece piece = board.GetPiece(new Position(x, y));
					if (piece != null) {
						line2 += piece.Colour.ToString() + " ";
						line3 += piece.PieceType.ToString();
						while (line3.Length / (x + 1) - 1 < 6) {
							line3 += " ";
						}
					}
					else {
						line2 += "      ";
						line3 += "      ";
					}
					line1 += "      |";
					line2 += "|";
					line3 += "|";
				}
				Console.WriteLine(line1);
				Console.WriteLine(line2);
				Console.WriteLine(line3);
				Console.WriteLine(line1);
				Console.WriteLine("---------------------------------------------------------");
			}
			Console.WriteLine("\n");
		}
	}
}
