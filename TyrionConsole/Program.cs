﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldDomination;
using Chess;

namespace TyrionConsole {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Daniel lugter af gud");
			string input = Console.ReadLine();
			if (input == "Daniel er gud") {
				Console.WriteLine("Haha joke");
			}
			if(input == "Fibonacci"){
				Console.WriteLine("Ikke nu");
			}
			World world = new World(50,50,10);
			Console.WriteLine(world[0, 0, 0].hej);

		}
	}
}
