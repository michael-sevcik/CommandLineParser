using System;
using System.Collections.Generic;
using Enumerable = System.Linq.Enumerable;

namespace Numactl;

/// <summary>List of ids parsable from a string representation.</summary>
struct IdList
{
    /// <summary>List of ids. <c>null</c> represents the entire available range.</summary>
    public IReadOnlyList<int>? Ids { get; }

    /// <summary>Create a new <see cref="IdList"/> from string representation.</summary>
    /// <exception cref="FormatException">
    /// List or numbers are not in correct format.
    /// </exception>
    /// <exception cref="OverflowException">
    /// Numbers are too large.
    /// </exception>
    public IdList(string idList)
    {
        if (idList == "all")
        {
            Ids = null;
            return;
        }

        List<int> idsBuilder = new();
        var items = idList.Split(',');
        foreach (var item in items)
        {
            var range = ParseItem(item);
            idsBuilder.AddRange(range);
        }
        Ids = idsBuilder.AsReadOnly();
    }

    /// <summary>Parse a single value or range.</summary>
    /// <exception cref="FormatException">
    /// List or numbers are not in correct format.
    /// </exception>
    /// <exception cref="OverflowException">
    /// Numbers are too large.
    /// </exception>
    private static IEnumerable<int> ParseItem(string item)
    {
        var splits = item.Split('-');
        if (splits.Length == 1)
        {
            int value = int.Parse(splits[0]);
            return Enumerable.Range(value, 1);
        }
        if (splits.Length == 2)
        {
            int start = int.Parse(splits[0]);
            int end = int.Parse(splits[1]);
            return Enumerable.Range(start, end - start + 1);
        }
        throw new FormatException("Too many '-' in item.");
    }
}
