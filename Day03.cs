namespace AdventofCode2025;

public class Day03
{
    [Fact]
    public void Part1_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";

        var totalOutputJoltage = CalculateTotalOutputJoltage(inputFilename);

        Assert.Equal(357, totalOutputJoltage);
    }

    [Fact]
    public void Part1()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";

        var totalOutputJoltage = CalculateTotalOutputJoltage(inputFilename);

        Assert.Equal(16927, totalOutputJoltage);
    }

    [Fact]
    public void Part2_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";

        var totalOutputJoltage = CalculateTotalOutputJoltage12(inputFilename);

        Assert.Equal(3121910778619, totalOutputJoltage);
    }

    [Fact]
    public void Part2()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";

        var totalOutputJoltage = CalculateTotalOutputJoltage12(inputFilename);

        Assert.Equal(167384358365132, totalOutputJoltage);
    }

    private static int CalculateTotalOutputJoltage(string inputFilename)
    {
        var banks = File.ReadLines(inputFilename).ToList();
        var results = new List<int>();

        foreach (var bank in banks)
        {
            var highestFirstNumber = bank[^2] - '0';
            var highestSecondNumber = bank[^1] - '0';

            for (var i = bank.Length - 3; i >= 0; i--)
            {
                var currentNumber = bank[i] - '0';
                if (currentNumber > highestFirstNumber)
                {
                    if (highestFirstNumber > highestSecondNumber)
                    {
                        highestSecondNumber = highestFirstNumber;
                    }

                    highestFirstNumber = currentNumber;
                }
                else if (currentNumber == highestFirstNumber && highestFirstNumber > highestSecondNumber)
                {
                    highestSecondNumber = currentNumber;
                }
            }

            results.Add(highestFirstNumber * 10 + highestSecondNumber);
        }

        return results.Sum();
    }

    private static long CalculateTotalOutputJoltage12(string inputFilename)
    {
        var banks = File.ReadLines(inputFilename).ToList();
        var results = new List<long>();

        foreach (var bank in banks)
        {
            var highestNumbers = bank[^12..].Select(long (c) => c - '0').ToArray();

            for (var i = bank.Length - 13; i >= 0; i--)
            {
                long currentNumber = bank[i] - '0';

                for (var j = 0; j < highestNumbers.Length && currentNumber >= highestNumbers[j]; j++)
                {
                    (highestNumbers[j], currentNumber) = (currentNumber, highestNumbers[j]);
                }
            }

            results.Add(highestNumbers.Aggregate((a, b) => a * 10 + b));
        }

        return results.Sum();
    }
}