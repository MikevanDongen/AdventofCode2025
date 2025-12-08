using Xunit.Abstractions;

namespace AdventofCode2025;

public class Day08(ITestOutputHelper output)
{
    [Fact]
    public void Part1_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";

        var sizesOfTheThreeLargestCircuitsMultiplied = Calculate(inputFilename, maxConnections: 10);

        output.WriteLine($"Sizes of the three largest circuits multiplied: {sizesOfTheThreeLargestCircuitsMultiplied}");

        Assert.Equal(40, sizesOfTheThreeLargestCircuitsMultiplied);
    }

    [Fact]
    public void Part1()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";

        var sizesOfTheThreeLargestCircuitsMultiplied = Calculate(inputFilename, maxConnections: 1000);

        output.WriteLine($"Sizes of the three largest circuits multiplied: {sizesOfTheThreeLargestCircuitsMultiplied}");

        Assert.Equal(83520, sizesOfTheThreeLargestCircuitsMultiplied);
    }

    [Fact]
    public void Part2_Demo()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}-demo.txt";

        var result = Calculate2(inputFilename);

        output.WriteLine($"Result: {result}");

        Assert.Equal(25272, result);
    }

    [Fact]
    public void Part2()
    {
        var inputFilename = $"inputs/{GetType().Name.ToLower()}.txt";

        var result = Calculate2(inputFilename);

        output.WriteLine($"Result: {result}");

        Assert.Equal(1131823407, result);
    }

    private long Calculate(string inputFilename, int maxConnections)
    {
        var lines = File.ReadLines(inputFilename);
        var coordinates = lines.Select(Coordinate.FromString).ToArray();
        var connections = GetCoordinateSets(coordinates);
        var circuits = CreateCircuits(connections.OrderBy(cs => cs.Distance).Take(maxConnections));
        var largestCircuits = circuits.OrderByDescending(c => c.Size).Take(3);

        output.WriteLine(
            $"The circuits contain the following coordinates: \n{string.Join("\n", circuits.OrderByDescending(c => c.Size))}");

        return largestCircuits.Aggregate(1L, (acc, circuit) => acc * circuit.Size);
    }

    private long Calculate2(string inputFilename)
    {
        var lines = File.ReadLines(inputFilename);
        var coordinates = lines.Select(Coordinate.FromString).ToArray();
        var connections = GetCoordinateSets(coordinates);
        var lastCircuitConnectingConnection =
            CreateCircuits2(connections.OrderBy(cs => cs.Distance), coordinates.Length);

        return lastCircuitConnectingConnection.CoordinateA.X * lastCircuitConnectingConnection.CoordinateB.X;
    }

    private static List<Circuit> CreateCircuits(IEnumerable<Connection> connections)
    {
        var circuits = new List<Circuit>();

        foreach (var connection in connections)
        {
            var matchingCircuits = circuits
                .Where(circuit => circuit.ContainsEitherCoordinate(connection))
                .ToList();

            switch (matchingCircuits.Count)
            {
                case 0:
                    circuits.Add(new Circuit(connection));
                    break;

                case 1:
                    matchingCircuits[0].AddConnection(connection);
                    break;

                default:
                    circuits.Remove(matchingCircuits[1]);
                    matchingCircuits[0].Merge(matchingCircuits[1]);
                    break;
            }
        }

        return circuits;
    }

    private static Connection CreateCircuits2(IEnumerable<Connection> connections, int maxConnections)
    {
        var circuits = new List<Circuit>();

        foreach (var connection in connections)
        {
            var matchingCircuits = circuits
                .Where(circuit => circuit.ContainsEitherCoordinate(connection))
                .ToList();

            switch (matchingCircuits.Count)
            {
                case 0:
                    circuits.Add(new Circuit(connection));
                    continue;

                case 1:
                    matchingCircuits[0].AddConnection(connection);
                    break;

                default:
                    circuits.Remove(matchingCircuits[1]);
                    matchingCircuits[0].Merge(matchingCircuits[1]);
                    break;
            }

            if (circuits.Count == 1 && circuits[0].Size == maxConnections)
            {
                return connection;
            }
        }

        throw new InvalidOperationException("Could not connect all coordinates into a single circuit.");
    }

    private static IEnumerable<Connection> GetCoordinateSets(Coordinate[] coordinates)
    {
        for (var i = 0; i < coordinates.Length; i++)
        {
            for (var j = i + 1; j < coordinates.Length; j++)
            {
                yield return new Connection(coordinates[i], coordinates[j]);
            }
        }
    }

    private record Coordinate(int X, int Y, int Z)
    {
        public static Coordinate FromString(string input)
        {
            var parts = input.Split(',').Select(int.Parse).ToArray();
            return new Coordinate(parts[0], parts[1], parts[2]);
        }

        public double CalculateEuclideanDistanceTo(Coordinate other) =>
            Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }
    }

    private record Connection(Coordinate CoordinateA, Coordinate CoordinateB)
    {
        public double Distance { get; } = CoordinateA.CalculateEuclideanDistanceTo(CoordinateB);
    }

    private class Circuit(Connection connection)
    {
        private readonly HashSet<Coordinate> _coordinates = new([connection.CoordinateA, connection.CoordinateB]);

        public long Size => _coordinates.Count;

        public bool ContainsEitherCoordinate(Connection connection) =>
            _coordinates.Contains(connection.CoordinateA)
            || _coordinates.Contains(connection.CoordinateB);

        public void AddConnection(Connection connection)
        {
            _coordinates.Add(connection.CoordinateA);
            _coordinates.Add(connection.CoordinateB);
        }

        public void Merge(Circuit otherCircuit)
        {
            foreach (var otherCoordinate in otherCircuit._coordinates)
            {
                _coordinates.Add(otherCoordinate);
            }
        }

        public override string ToString()
        {
            return $"{string.Join(" <-> ", _coordinates)} (Size: {Size})";
        }
    }
}