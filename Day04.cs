using Xunit.Abstractions;

namespace AdventofCode2025;

public class Day04(ITestOutputHelper output)
{
    [Fact]
    public void Part1_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";
        var numberOfAccessibleRolls = GetNumberOfAccessibleRolls(inputFilename);

        output.WriteLine($"Number of accessible rolls: {numberOfAccessibleRolls}");

        Assert.Equal(13, numberOfAccessibleRolls);
    }

    [Fact]
    public void Part1()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";
        var numberOfAccessibleRolls = GetNumberOfAccessibleRolls(inputFilename);

        output.WriteLine($"Number of accessible rolls: {numberOfAccessibleRolls}");

        Assert.Equal(1449, numberOfAccessibleRolls);
    }

    [Fact]
    public void Part2_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";
        var numberOfTotalAccessibleRolls = GetNumberOfTotalAccessibleRolls(inputFilename);

        output.WriteLine($"Number of accessible rolls: {numberOfTotalAccessibleRolls}");

        Assert.Equal(43, numberOfTotalAccessibleRolls);
    }

    [Fact]
    public void Part2()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";
        var numberOfTotalAccessibleRolls = GetNumberOfTotalAccessibleRolls(inputFilename);

        output.WriteLine($"Number of accessible rolls: {numberOfTotalAccessibleRolls}");

        Assert.Equal(8746, numberOfTotalAccessibleRolls);
    }

    private int GetNumberOfTotalAccessibleRolls(string inputFilename)
    {
        var lines = File.ReadLines(inputFilename).ToList();
        var maxHorizontalIndex = lines[0].Length - 1;
        var maxVerticalIndex = lines.Count - 1;

        var rollsCache = new List<(int, int)>();
        List<(int, int)> accessibleRolls;

        do
        {
            accessibleRolls = GetAccessibleRolls(IsRoll, maxHorizontalIndex, maxVerticalIndex);
            rollsCache.AddRange(accessibleRolls);
        } while (accessibleRolls.Count > 0);

        return rollsCache.Count;

        bool IsRoll(int vi, int hi) => !rollsCache.Contains((hi, vi)) && lines[vi][hi] == '@';
    }

    private int GetNumberOfAccessibleRolls(string inputFilename)
    {
        const int maxAdjacentRolls = 3;

        var lines = File.ReadLines(inputFilename).ToList();
        var maxHorizontalIndex = lines[0].Length - 1;
        var maxVerticalIndex = lines.Count - 1;
        var numberOfAccessibleRolls = 0;

        output.WriteLine($"Grid size: {maxHorizontalIndex + 1}x{maxVerticalIndex + 1}");

        for (var hi = 0; hi <= maxHorizontalIndex; hi++)
        {
            for (var vi = 0; vi <= maxVerticalIndex; vi++)
            {
                var currentIsRoll = IsRoll(vi, hi);

                if (!currentIsRoll)
                {
                    continue;
                }

                var numberOfAdjacentRolls = CountAdjacentRolls(IsRoll, vi, hi, maxVerticalIndex, maxHorizontalIndex);

                output.WriteLine($"Roll found at ({vi},{hi}) has {numberOfAdjacentRolls} adjacent rolls.");

                if (numberOfAdjacentRolls <= maxAdjacentRolls)
                {
                    numberOfAccessibleRolls++;
                }
            }
        }

        return numberOfAccessibleRolls;

        bool IsRoll(int vi, int hi) => lines[vi][hi] == '@';
    }

    private static int CountAdjacentRolls(Func<int, int, bool> isRoll,
        int vi, int hi, int maxVerticalIndex, int maxHorizontalIndex)
    {
        var adjacentRolls = 0;

        // Left
        if (vi > 0 && isRoll(vi - 1, hi))
        {
            adjacentRolls++;
        }

        // Right
        if (vi < maxVerticalIndex && isRoll(vi + 1, hi))
        {
            adjacentRolls++;
        }

        // Up
        if (hi > 0 && isRoll(vi, hi - 1))
        {
            adjacentRolls++;
        }

        // Down
        if (hi < maxHorizontalIndex && isRoll(vi, hi + 1))
        {
            adjacentRolls++;
        }

        // Left-up
        if (vi > 0 && hi > 0 && isRoll(vi - 1, hi - 1))
        {
            adjacentRolls++;
        }

        // Left-down
        if (vi > 0 && hi < maxHorizontalIndex && isRoll(vi - 1, hi + 1))
        {
            adjacentRolls++;
        }

        // Right-up
        if (vi < maxVerticalIndex && hi > 0 && isRoll(vi + 1, hi - 1))
        {
            adjacentRolls++;
        }

        // Right-down
        if (vi < maxVerticalIndex && hi < maxHorizontalIndex && isRoll(vi + 1, hi + 1))
        {
            adjacentRolls++;
        }

        return adjacentRolls;
    }

    private List<(int, int)> GetAccessibleRolls(Func<int, int, bool> isRoll, int maxHorizontalIndex,
        int maxVerticalIndex)
    {
        const int maxAdjacentRolls = 3;

        var accessibleRolls = new List<(int, int)>();

        for (var hi = 0; hi <= maxHorizontalIndex; hi++)
        {
            for (var vi = 0; vi <= maxVerticalIndex; vi++)
            {
                var currentIsRoll = isRoll(vi, hi);

                if (!currentIsRoll)
                {
                    continue;
                }

                var numberOfAdjacentRolls = CountAdjacentRolls(isRoll, vi, hi, maxVerticalIndex, maxHorizontalIndex);

                if (numberOfAdjacentRolls <= maxAdjacentRolls)
                {
                    accessibleRolls.Add((hi, vi));
                }
            }
        }

        return accessibleRolls;
    }
}