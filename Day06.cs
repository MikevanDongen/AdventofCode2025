using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventofCode2025;

public partial class Day06(ITestOutputHelper output)
{
    [GeneratedRegex(@"([+\*] +)(?: |$)")]
    private static partial Regex OperatorRegex();

    [Fact]
    public void Part1_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";

        var result = Calculate(inputFilename);

        output.WriteLine($"Result: {result}");

        Assert.Equal(4277556, result);
    }


    [Fact]
    public void Part1()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";

        var result = Calculate(inputFilename);

        output.WriteLine($"Result: {result}");

        Assert.Equal(5667835681547, result);
    }

    [Fact]
    public void Part2_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";

        var result = Calculate2(inputFilename);

        output.WriteLine($"Result: {result}");

        Assert.Equal(3263827, result);
    }

    [Fact]
    public void Part2()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";

        var result = Calculate2(inputFilename);

        output.WriteLine($"Result: {result}");

        Assert.Equal(9434900032651, result);
    }

    private static long Calculate(string inputFilename)
    {
        var lines = File.ReadLines(inputFilename).Reverse().ToList();
        var calculators = lines[0]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(Calculator.Create)
            .ToList();

        foreach (var line in lines.Skip(1))
        {
            var numbers = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < numbers.Length; i++)
            {
                var number = long.Parse(numbers[i]);
                calculators[i].Apply(number);
            }
        }

        return calculators.Sum(c => c.Result);
    }

    private long Calculate2(string inputFilename)
    {
        var lines = File.ReadLines(inputFilename).Reverse().ToList();
        var calculators = OperatorRegex()
            .Matches(lines[0])
            .Select(m => Calculator.Create(m.Groups[1].Value))
            .ToList();

        output.WriteLine($"Calculators have column sizes: {string.Join(", ", calculators.Select(c => c.ColumnSize))}");

        var inputs = calculators.Select(c => Enumerable.Range(0, c.ColumnSize).Select(_ => new List<int>()).ToArray())
            .ToArray();

        for (var l = 1; l < lines.Count; l++)
        {
            var line = lines[l];
            var lineIndex = 0;

            for (var i = 0; i < calculators.Count; i++)
            {
                var calculator = calculators[i];
                var input = line[lineIndex..(lineIndex + calculator.ColumnSize)];

                for (var j = 0; j < calculator.ColumnSize; j++)
                {
                    if (input[j] == ' ')
                    {
                        continue;
                    }

                    inputs[i][j].Add(input[j] - '0');
                }

                lineIndex += calculator.ColumnSize + 1;
            }
        }

        var total = 0L;

        for (var i = 0; i < calculators.Count; i++)
        {
            foreach (IEnumerable<int> rawInput in inputs[i])
            {
                var input = rawInput.Reverse().Aggregate((a, b) => a * 10 + b);
                calculators[i].Apply(input);
            }

            total += calculators[i].Result;
        }

        return total;
    }

    private class Calculator(Func<long, long, long> operation, int columnSize)
    {
        private bool _isFirstApplication = true;

        public int ColumnSize { get; } = columnSize;

        public long Result { get; private set; }

        public static Calculator Create(string input)
        {
            return input[0] switch
            {
                '+' => new Calculator(Add, input.Length),
                '*' => new Calculator(Multiply, input.Length),
                _ => throw new InvalidOperationException($"Unknown operation: {input}"),
            };

            static long Add(long x, long y) => x + y;
            static long Multiply(long x, long y) => x * y;
        }

        public void Apply(long value)
        {
            if (_isFirstApplication)
            {
                _isFirstApplication = false;
                Result = value;

                return;
            }

            Result = operation(Result, value);
        }
    }
}