using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldDomination {
	public class World {
		Area[, ,] map;
		public Area this[int x, int y, int z] {
			get {
				return map[x, y, z];
			}
		}

		public World(int x, int y, int z) {
			map = new Area[x, y, z];
			for (int i = 0; i < x; i++) {
				for (int j = 0; j < y; j++) {
					for (int k = 0; k < z; k++) {
						map[i,j,k] = new Area();
					}
				}
			}
		}
	}
}
