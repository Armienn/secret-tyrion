using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skak {
	public class Skak {
		public Skak() {

		}
	}

	public class Brik {

		private BrikType type;
		private Farve farve;
		private int x;
		private int y;

		public Brik(BrikType type, Farve farve, int x, int y) {
			this.type = type;
			this.farve = farve;
			this.x = x;
			this.y = y;
		}

		public Boolean ILive() {
			if (7 < x || x < 0 || 7 < y || y < 0) {
				return false;
			}
			return true;
		}


	}

	public enum BrikType {
		Bonde, Tårn, Springer, Løber, Dronning, Konge
	}

	public enum Farve {
		Hvid, Sort
	}
}
