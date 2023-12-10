var input = File.ReadAllLines("input.txt").Select(str => str.Split(' ')).Select(strings => strings.Select(long.Parse).ToList()).ToList();
List<List<long>> historyCache = new List<List<long>>();
long partOneSum = 0;
long partTwoSum = 0;

foreach (var line in input)
{
    historyCache.Clear();
    historyCache.Add(line);
    var lastLine = line;

    // All are 0's at the last line at the end of this polong.
    while (lastLine.Any(i => i != 0))
    {
        List<long> newLine = new List<long>();
        for (int i = 1; i < lastLine.Count; i++)
        {
            newLine.Add(lastLine[i] - lastLine[i-1]);
        }

        historyCache.Add(newLine);
        lastLine = newLine;
    }

    long differenceEnd = 0;
    long differenceStart = 0;
    for (int i = historyCache.Count - 2; i >= 0; i--)
    {
        var nextLine = historyCache[i];
        differenceEnd = nextLine[nextLine.Count - 1] + differenceEnd;
        differenceStart = nextLine[0] - differenceStart;
    }

    partOneSum += differenceEnd;
    partTwoSum += differenceStart;
}

Console.WriteLine($"Part One: {partOneSum}");
Console.WriteLine($"Part Two: {partTwoSum}");
