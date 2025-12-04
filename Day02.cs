using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace AdventofCode2025;

public class Day02(ITestOutputHelper output)
{
    [Fact]
    public void Part1()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";
        var ranges = File.ReadAllText(inputFilename).Split(',');

        output.WriteLine($"Validating {ranges.Length} ranges.");

        var invalidIDHalves = new ConcurrentBag<int>();
        ranges.AsParallel().ForAll(s => Validate(s, invalidIDHalves));

        output.WriteLine($"Found {invalidIDHalves.Count} invalid IDs: {string.Join(", ", invalidIDHalves)}");

        var sum = invalidIDHalves.Sum(h => long.Parse($"{h}{h}"));
        Assert.Equal(17077011375, sum);
    }

    private void Validate(string range, ConcurrentBag<int> invalidIDHalves)
    {
        var parts = range.Split('-');
        var startLength = parts[0].Length;
        var endLength = parts[1].Length;

        int startHalf;
        int endHalf;

        var startLengthIsEven = startLength % 2 == 0;
        if (!startLengthIsEven)
        {
            var startAndEndAreTheSameLength = startLength == endLength;
            if (startAndEndAreTheSameLength)
            {
                return;
            }

            var startHalfLength = startLength / 2;
            startHalf = int.Parse($"1{new string('0', startHalfLength)}");
        }
        else
        {
            var startHalf1 = int.Parse(parts[0][..(startLength / 2)]);
            var startHalf2 = int.Parse(parts[0][(startLength / 2)..]);

            startHalf = startHalf1 >= startHalf2 ? startHalf1 : startHalf1 + 1;
        }

        var endLengthIsEven = endLength % 2 == 0;
        if (!endLengthIsEven)
        {
            var endHalfLength = endLength / 2;
            endHalf = int.Parse($"1{new string('0', endHalfLength)}") - 1;
        }
        else
        {
            var endHalf1 = int.Parse(parts[1][..(endLength / 2)]);
            var endHalf2 = int.Parse(parts[1][(endLength / 2)..]);

            endHalf = endHalf1 <= endHalf2 ? endHalf1 : endHalf1 - 1;
        }

        output.WriteLine($"Range {range} has been corrected to {startHalf}{startHalf}-{endHalf}{endHalf}");

        while (startHalf <= endHalf)
        {
            invalidIDHalves.Add(startHalf);

            startHalf++;
        }
    }
}