using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyrionConsole {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Daniel lugter af gud");
			string input = Console.ReadLine();
			if (input == "Daniel er gud") {
				Console.WriteLine("Ja");
			}
			if(input == "Fibonacci"){
				Console.WriteLine("Ikke nu");
			}
		}
	}
}
