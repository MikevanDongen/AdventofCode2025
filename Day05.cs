using Xunit.Abstractions;

namespace AdventofCode2025;

public class Day05(ITestOutputHelper output)
{
    [Fact]
    public void Part1_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";
        var numberOfFreshIngredients = GetNumberOfFreshIngredients(inputFilename);

        output.WriteLine($"Number of fresh ingredients: {numberOfFreshIngredients}");

        Assert.Equal(3, numberOfFreshIngredients);
    }

    [Fact]
    public void Part1()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";
        var numberOfFreshIngredients = GetNumberOfFreshIngredients(inputFilename);

        output.WriteLine($"Number of fresh ingredients: {numberOfFreshIngredients}");

        Assert.Equal(517, numberOfFreshIngredients);
    }

    [Fact]
    public void Part2_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";
        var numberOfFreshIngredients = GetNumberOfFreshIngredients2(inputFilename);

        output.WriteLine($"Number of fresh ingredients: {numberOfFreshIngredients}");

        Assert.Equal(14, numberOfFreshIngredients);
    }

    [Fact]
    public void Part2()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";
        var numberOfFreshIngredients = GetNumberOfFreshIngredients2(inputFilename);

        output.WriteLine($"Number of fresh ingredients: {numberOfFreshIngredients}");

        Assert.Equal(336173027056994, numberOfFreshIngredients);
    }

    private static long GetNumberOfFreshIngredients(string inputFilename)
    {
        var lines = File.ReadLines(inputFilename).ToList();
        var splitIndex = lines.IndexOf(string.Empty);
        var freshRanges = lines[..splitIndex].Select(r => r.Split('-').Select(long.Parse).ToArray()).ToList();
        var ingredients = lines[(splitIndex + 1)..].Select(long.Parse).ToList();

        var freshIngredients = ingredients
            .Where(i => freshRanges.Any(r => i >= r[0] && i <= r[1]));

        return freshIngredients.Count();
    }

    private static long GetNumberOfFreshIngredients2(string inputFilename)
    {
        var lines = File.ReadLines(inputFilename).ToList();
        var splitIndex = lines.IndexOf(string.Empty);

        var freshRanges = lines[..splitIndex]
            .Select(r => r.Split('-').Select(long.Parse).ToArray())
            .OrderBy(r => r[0]);

        var numberOfFreshIngredients = 0L;
        var lastHigh = 0L;

        foreach (var freshRange in freshRanges)
        {
            if (freshRange[1] <= lastHigh)
            {
                continue;
            }

            if (freshRange[0] <= lastHigh)
            {
                numberOfFreshIngredients += freshRange[1] - lastHigh;
            }
            else
            {
                numberOfFreshIngredients += freshRange[1] - freshRange[0] + 1;
            }

            lastHigh = freshRange[1];
        }

        return numberOfFreshIngredients;
    }
}