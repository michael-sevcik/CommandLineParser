using ArgumentParsing;

internal class Program
{
    static void Main(string[] args)
    {
        int x = 7;
        IntOption IO = new IntOption(ref x);
        IO._output = 100;
        Console.WriteLine(x);

        ParamOutput<int> ahoj;
    }
}