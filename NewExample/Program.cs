using System;
using ArgumentParsing;

namespace NewExample {
	class Program {
		public static void Main(string[] args) {
            Parser parser = new(null, "Terminate option list.");

            var builder = new OptionBuilder();
			builder.WithParametrizedAction<string>(InterleaveAction).
				RequiresParameter()
				.WithShortSynonyms('i').
				WithLongSynonyms("interleave").
				WithHelpString("Interleave memory allocation across given nodes.").
				RegisterOption(parser);

            builder.Reset().
				WithParametrizedAction<string>(PreferredAction).
                RequiresParameter()
                .WithShortSynonyms('p').
                WithLongSynonyms("preferred").
                WithHelpString("Prefer memory allocations from given node.").
                RegisterOption(parser);

            builder.Reset().
                WithParametrizedAction<string>(MembindAction).
                RequiresParameter()
                .WithShortSynonyms('m').
                WithLongSynonyms("membind").
                WithHelpString("Allocate memory from given nodes only.").
                RegisterOption(parser);

            builder.Reset().
                WithParametrizedAction<string>(PhyscpubindAction).
                RequiresParameter()
                .WithShortSynonyms('C').
                WithLongSynonyms("physcpubind").
                WithHelpString("Run on given CPUs only.").
                RegisterOption(parser);

            builder.Reset().
                WithAction(ShowAction)
                .WithShortSynonyms('s').
                WithLongSynonyms("show").
                WithHelpString("Show current NUMA policy.").
                RegisterOption(parser);

            builder.Reset().
                WithAction(HardwareAction)
                .WithShortSynonyms('H').
                WithLongSynonyms("hardware").
                WithHelpString("Print hardware configuration.").
                RegisterOption(parser);

			if (args.Length == 0)
			{
				DisplayHelp();
				return;
			}

			parser.ParseCommandLine(args);
		}

		private static void InterleaveAction(string nodes) {
			Console.WriteLine($"Interleave memory allocation across nodes: {nodes}");
		}

		private static void PreferredAction(string node) {
			Console.WriteLine($"Prefer memory allocations from node: {node}");
		}

		private static void MembindAction(string nodes) {
			Console.WriteLine($"Allocate memory from nodes only: {nodes}");
		}

		private static void PhyscpubindAction(string cpus) {
			Console.WriteLine($"Run on given CPUs only: {cpus}");
		}

		private static void ShowAction() {
			Console.WriteLine("Show current NUMA policy.");
			EmulateShowNumaPolicy();
		}

		private static void HardwareAction() {
			Console.WriteLine("Print hardware configuration.");
			EmulateHardwareInfo();
		}

		static void DisplayHelp() {
			Console.WriteLine("Usage: numactl [--interleave= | -i <nodes>] [--preferred= | -p <node>]");
			Console.WriteLine("               [--physcpubind= | -C <cpus>] [--membind= | -m <nodes>]");
			Console.WriteLine("               command args ...");
			Console.WriteLine("       numactl [--show | -S]");
			Console.WriteLine("       numactl [--hardware | -H]");
			Console.WriteLine();
			Console.WriteLine("<nodes> is a comma-delimited list of node numbers or A-B ranges or all.");
			Console.WriteLine("<cpus> is a comma-delimited list of cpu numbers or A-B ranges or all.");
			Console.WriteLine();
			Console.WriteLine("--interleave, -i   Interleave memory allocation across given nodes.");
			Console.WriteLine("--preferred, -p    Prefer memory allocations from given node.");
			Console.WriteLine("--membind, -m      Allocate memory from given nodes only.");
			Console.WriteLine("--physcpubind, -C  Run on given CPUs only.");
			Console.WriteLine("--show, -S         Show current NUMA policy.");
			Console.WriteLine("--hardware, -H     Print hardware configuration.");
		}


		static void EmulateHardwareInfo() {
			Console.WriteLine("available: 2 nodes (0-1)");
			Console.WriteLine("node 0 cpus: 0 2 4 6 8 10 12 14 16 18 20 22");
			Console.WriteLine("node 0 size: 24189 MB");
			Console.WriteLine("node 0 free: 18796 MB");
			Console.WriteLine("node 1 cpus: 1 3 5 7 9 11 13 15 17 19 21 23");
			Console.WriteLine("node 1 size: 24088 MB");
			Console.WriteLine("node 1 free: 16810 MB");
			Console.WriteLine("node distances:");
			Console.WriteLine("node   0   1");
			Console.WriteLine("  0:  10  20");
			Console.WriteLine("  1:  20  10");
		}

		static void EmulateShowNumaPolicy() {
			Console.WriteLine("policy: default");
			Console.WriteLine("preferred node: current");
			Console.WriteLine("physcpubind: 0 1 2 3 4 5 6 7 8");
			Console.WriteLine("cpubind: 0 1");
			Console.WriteLine("nodebind: 0 1");
			Console.WriteLine("membind: 0 1");
		}
	}
}