using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skak {
	public class Skak {
		private Brik[] brikker;
		public Farve Tur { get; private set; }

		public Skak() {
			brikker = new Brik[32];
			brikker[0] = new Brik(this, BrikType.Tårn, Farve.Hvid, 0, 0);
			brikker[1] = new Brik(this, BrikType.Springer, Farve.Hvid, 1, 0);
			brikker[2] = new Brik(this, BrikType.Løber, Farve.Hvid, 2, 0);
			brikker[3] = new Brik(this, BrikType.Dronning, Farve.Hvid, 3, 0);
			brikker[4] = new Brik(this, BrikType.Konge, Farve.Hvid, 4, 0);
			brikker[5] = new Brik(this, BrikType.Løber, Farve.Hvid, 5, 0);
			brikker[6] = new Brik(this, BrikType.Springer, Farve.Hvid, 6, 0);
			brikker[7] = new Brik(this, BrikType.Tårn, Farve.Hvid, 7, 0);
			for (int i = 0; i < 8; i++) {
				brikker[i + 8] = new Brik(this, BrikType.Bonde, Farve.Hvid, i, 1);
			}
			brikker[16] = new Brik(this, BrikType.Tårn, Farve.Sort, 0, 7);
			brikker[17] = new Brik(this, BrikType.Springer, Farve.Sort, 1, 7);
			brikker[18] = new Brik(this, BrikType.Løber, Farve.Sort, 2, 7);
			brikker[19] = new Brik(this, BrikType.Dronning, Farve.Sort, 3, 7);
			brikker[20] = new Brik(this, BrikType.Konge, Farve.Sort, 4, 7);
			brikker[21] = new Brik(this, BrikType.Løber, Farve.Sort, 5, 7);
			brikker[22] = new Brik(this, BrikType.Springer, Farve.Sort, 6, 7);
			brikker[23] = new Brik(this, BrikType.Tårn, Farve.Sort, 7, 7);
			for (int i = 0; i < 8; i++) {
				brikker[i + 24] = new Brik(this, BrikType.Bonde, Farve.Sort, i, 6);
			}
		}

		public Brik BrikPå(int x, int y) {
			for (int i = 0; i < brikker.Length; i++) {
				if (brikker[i].X == x && brikker[i].Y == y) {
					return brikker[i];
				}
			}
			return null;
		}

		public Boolean ErTrækGyldig(Brik brik, int x, int y) {
			Brik destinationsbrik = BrikPå(x, y);
			if (brik.Farve != Tur) {
				return false;
			}
			if (brik.X == x && brik.Y == y) {
				return false;
			}
			if (7 < x || x < 0 || 7 < y || y < 0) {
				return false;
			}
			if (!brik.ILive()) {
				return false;
			}
			if (destinationsbrik != null && destinationsbrik.Farve == brik.Farve) {
				return false;
			}
			switch (brik.Type) {
				case BrikType.Bonde:
					//dobbelt hop
					if (brik.Farve == Farve.Hvid) {
						if ((brik.X - 1 == x && brik.Y + 1 == y) || (brik.X + 1 == x && brik.Y + 1 == y) && BrikPå(x, y) == null) {
							return false;
						}
						if (brik.Y + 1 != y) {
							return false;
						}
					}
					if ((brik.X - 1 == x && brik.Y - 1 == y) || (brik.X + 1 == x && brik.Y - 1 == y) && BrikPå(x, y) == null) {
						return false;
					}
					if (brik.Y - 1 != y) {
						return false;
					}
					break;
				case BrikType.Tårn:
					//konge byt
					if (brik.X != x && brik.Y != y) {
						return false;
					}
					if () {
						//i vejen
					}
					break;
				case BrikType.Springer:
					if ((Math.Abs(x - brik.X) != 2 && Math.Abs(y - brik.Y) != 1) || (Math.Abs(x - brik.X) != 1 && Math.Abs(y - brik.Y) != 2)) {
						return false;
					}
					break;
				case BrikType.Løber:
					if (Math.Abs(x - brik.X) != Math.Abs(y - brik.Y)) {
						return false;
					}
					if () {
						//i vejen
					}
					break;
				case BrikType.Dronning:
					if (Math.Abs(x - brik.X) != Math.Abs(y - brik.Y) && brik.X != x && brik.Y != y) {
						return false;
					}
					if () {
						//i vejen
					}
					break;
				case BrikType.Konge:
					if (brik.X < x - 1 || x + 1 < brik.X || brik.Y < y - 1 || y + 1 < brik.Y) {
						return false;
					}
					break;
			}
			return true;
		}
	}

	public class Brik {
		private Skak skak;
		public BrikType Type { get; private set; }
		public Farve Farve { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }

		public Brik(Skak skak, BrikType type, Farve farve, int x, int y) {
			this.skak = skak;
			Type = type;
			Farve = farve;
			X = x;
			Y = y;
		}

		public Boolean ILive() {
			if (7 < X || X < 0 || 7 < Y || Y < 0) {
				return false;
			}
			return true;
		}

		public void RykBrik(int x, int y) {
			if (skak.ErTrækGyldig(this, x, y)) {
				Brik brik = skak.BrikPå(x, y);
				if (brik != null) {
					brik.X -= 8;
				}
				X = x;
				Y = y;
			}
		}


	}

	public enum BrikType {
		Bonde, Tårn, Springer, Løber, Dronning, Konge
	}

	public enum Farve {
		Hvid, Sort
	}
}
