using Xunit.Abstractions;

namespace AdventofCode2025;

public class Day01(ITestOutputHelper output)
{
    [Fact]
    public void Part1()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";
        var lines = File.ReadLines(inputFilename);

        var pointingAtNumber = 50; // The dial starts by pointing at 50.
        var numberOfTimesPointingAtZero = 0;

        foreach (var line in lines)
        {
            var amount = int.Parse(line[1..]);
            var rotation = line[0] == 'L' ? -amount : amount;

            pointingAtNumber = Rotate(pointingAtNumber, rotation);

            if (pointingAtNumber == 0)
            {
                numberOfTimesPointingAtZero++;
            }
        }

        output.WriteLine($"The dial is pointing at number {pointingAtNumber} after all the rotations.");
        output.WriteLine($"The dial pointed at 0 a total of {numberOfTimesPointingAtZero} times.");

        Assert.Equal(1074, numberOfTimesPointingAtZero);
    }

    private static int Rotate(int start, int rotation)
    {
        var newPointingAtNumber = start + rotation;

        while (true)
        {
            switch (newPointingAtNumber)
            {
                case > 99:
                    newPointingAtNumber -= 100;
                    break;

                case < 0:
                    newPointingAtNumber += 100;
                    break;

                default:
                    return newPointingAtNumber;
            }
        }
    }

    [Fact]
    public void Part2()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";
        var lines = File.ReadLines(inputFilename);

        var pointingAtNumber = 50; // The dial starts by pointing at 50.
        var numberOfTimesPointingAtZero = 0;

        output.WriteLine("The dial starts by pointing at 50.");

        foreach (var line in lines)
        {
            var amount = int.Parse(line[1..]);
            var rotation = line[0] == 'L' ? -amount : amount;

            (pointingAtNumber, var additionalNumberOfTimesPointingAtZero) = Rotate2(pointingAtNumber, rotation);

            numberOfTimesPointingAtZero += additionalNumberOfTimesPointingAtZero;

            output.WriteLine(
                $"The dial is rotated {line} to point at {pointingAtNumber}{(additionalNumberOfTimesPointingAtZero > 0
                    ? $"; during this rotation, it points at zero {additionalNumberOfTimesPointingAtZero} time(s)"
                    : "")}.");
        }

        output.WriteLine($"The dial is pointing at number {pointingAtNumber} after all the rotations.");
        output.WriteLine($"The dial pointed at 0 a total of {numberOfTimesPointingAtZero} times.");

        Assert.Equal(6254, numberOfTimesPointingAtZero);
    }

    private static (int, int) Rotate2(int start, int rotation)
    {
        var newPointingAtNumber = start + rotation;

        var numberOfTimesPassingZero = newPointingAtNumber / 100;

        newPointingAtNumber -= numberOfTimesPassingZero * 100;

        var numberOfTimesPointingAtZero = Math.Abs(numberOfTimesPassingZero);

        if (newPointingAtNumber == 0 && rotation < 0)
        {
            numberOfTimesPointingAtZero++;
        }
        else if (newPointingAtNumber < 0)
        {
            newPointingAtNumber += 100;

            if (start != 0)
            {
                numberOfTimesPointingAtZero++;
            }
        }

        return (newPointingAtNumber, numberOfTimesPointingAtZero);
    }
}