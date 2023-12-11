var input = File.ReadAllLines("input.txt").Select(str => str.ToCharArray().ToList()).ToList();
ExpandMap();

var galaxies = GetAllGalaxyPositions(1, 1);
Console.WriteLine($"Part One: {GetStepsBetweenAllGalaxies()}");

galaxies = GetAllGalaxyPositions(1000000 - 1, 1000000 - 1);
Console.WriteLine($"Part Two: {GetStepsBetweenAllGalaxies()}");

void ExpandMap()
{
    for (int i = input.Count - 1; i >= 0; i--)
    {
        if (input[i].All(ch => ch == '.'))
        {
            input[i] = Array.ConvertAll(input[i].ToArray(), ch => 'V').ToList();
        }
    }

    for (int i = input[0].Count - 1; i >= 0; i--)
    {
        if (input.All(chArr => chArr[i] == '.' || chArr[i] == 'V'))
        {
            for (int j = 0; j < input.Count; ++j)
            {
                input[j][i] = 'H';
            }
        }
    }
}

List<(long x, long y)> GetAllGalaxyPositions(long hVal, long vVal)
{
    long hOffset = 0;
    long vOffset = 0;
    var positions = new List<(long x, long y)>();

    for (int i = 0; i < input.Count; ++i)
    {
        hOffset = 0;

        for(int j = 0; j < input[i].Count; ++j)
        {
            if (input[i][j] == '#') positions.Add((hOffset + j, vOffset + i));
            else if (input[i][j] == 'H') hOffset += hVal;
            else if (input[i][j] == 'V') { vOffset += vVal; break; }
        }
    }

    return positions;
}

long GetStepsBetweenAllGalaxies()
{
    long steps = 0;

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
