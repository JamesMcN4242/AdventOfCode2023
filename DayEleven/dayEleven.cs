var input = File.ReadAllLines("input.txt").Select(str => str.ToCharArray().ToList()).ToList();
ExpandMap();
var galaxies = GetAllGalaxyPositions();
Console.WriteLine($"Part One: {GetStepsBetweenAllGalaxies()}");

void ExpandMap()
{
    for (int i = input.Count - 1; i >= 0; i--)
    {
        if (input[i].All(ch => ch == '.'))
        {
            input.Insert(i, new List<char>(input[i]));
        }
    }

    for (int i = input[0].Count - 1; i >= 0; i--)
    {
        if (input.All(chArr => chArr[i] == '.'))
        {
            for (int j = 0; j < input.Count; ++j)
            {
                input[j].Insert(i, '.');
            }
        }
    }
}

List<(int x, int y)> GetAllGalaxyPositions()
{
    var positions = new List<(int x, int y)>();
    for (int i = 0; i < input.Count; ++i)
    {
        for(int j = 0; j < input[i].Count; ++j)
        {
            if (input[i][j] == '#') positions.Add((j, i));
        }
    }
    return positions;
}

int GetStepsBetweenAllGalaxies()
{
    int steps = 0;

    for (int i = 0; i < galaxies.Count; ++i)
    {
        for (int j = i + 1; j < galaxies.Count; ++j)
        {
            // Using Manhattan distance
            steps += Math.Abs(galaxies[i].x - galaxies[j].x) + Math.Abs(galaxies[i].y - galaxies[j].y);
        }
    }

    return steps;
}
