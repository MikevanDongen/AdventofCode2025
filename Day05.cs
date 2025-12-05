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

    private long GetNumberOfFreshIngredients(string inputFilename)
    {
        var lines = File.ReadLines(inputFilename).ToList();
        var splitIndex = lines.IndexOf(string.Empty);
        var freshRanges = lines[..splitIndex].Select(r => r.Split('-').Select(long.Parse).ToArray()).ToList();
        var ingredients = lines[(splitIndex + 1)..].Select(long.Parse).ToList();

        var freshIngredients = ingredients
            .Where(i => freshRanges.Any(r => i >= r[0] && i <= r[1]));

        return freshIngredients.Count();
    }
}