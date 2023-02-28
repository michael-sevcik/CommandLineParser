using ArgumentParsing;

internal class Program
{
    static void Experiment(int[]? ints)
    {
        foreach (var i in ints) { Console.WriteLine(i); }
    }
    static void Main(string[] args)
    {
        int[] ints = { 1, 2,3 };
        Experiment(ints);
        Action<int[]?> action = new(Experiment);
        action(ints);
        
    }
}