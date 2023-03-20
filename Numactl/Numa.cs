using System;
using System.Collections.Generic;
using Enumerable = System.Linq.Enumerable;

namespace Numactl;

/// <summary>Mock representing the numa service.</summary>
class Numa
{
    public static NumaConfig CurrentConfig { get; private set; }

    public static void PrintHardwareConfiguration()
    {
        Console.WriteLine("Hardware configuration");
    }

    /// <summary>
    /// Print CPU and NUMA node configuration.
    /// Expects that configuration is valid.
    /// </summary>
    public static void PrintConfiguration(NumaConfig config)
    {
        Console.Write("physcpubind: ");
        if (config.Physcpubind == null)
        {
            Console.WriteLine("default");
        }
        else
        {
            PrintIds(config.Physcpubind.Value, Enumerable.Range(0, 8));
        }

        if (config.Interleave != null)
        {
            Console.Write("interleave: ");
            PrintIds(config.Interleave.Value, Enumerable.Range(0, 4));
        }
        if (config.Membind != null)
        {
            Console.Write("membind: ");
            PrintIds(config.Membind.Value, Enumerable.Range(0, 4));
        }
        if (config.Preferred != null)
        {
            Console.Write("preferred: {0}", config.Preferred);
        }
    }

    /// <summary>Run a command with given configuration.</summary>
    public static void RunCommand(string command, string[] args, NumaConfig config)
    {
        Console.WriteLine("Running command: {0} {1}", command, string.Join(" ", args));
        PrintConfiguration(config);
    }

    private static void PrintIds(IdList ids, IEnumerable<int> allIds)
    {
        IEnumerable<int> idsToPrint = ids.Ids ?? allIds;
        Console.WriteLine(string.Join(" ", idsToPrint));
    }
}


struct NumaConfig
{
    public IdList? Interleave;
    public int? Preferred;
    public IdList? Membind;
    public IdList? Physcpubind;

    /// <summary>Checks for more than option configuring NUMA nodes.</summary>
    public bool HasConflict()
    {
        int numNodesOptions = 0;
        if (Interleave != null) numNodesOptions++;
        if (Preferred != null) numNodesOptions++;
        if (Membind != null) numNodesOptions++;
        return numNodesOptions > 1;
    }
}
