var input = File.ReadAllLines("input.txt").Select(x => x.Split(' ')).Select(str => (line: str[0], combo: str[1].Split(',').Select(int.Parse).ToList())).ToList();

long sum = 0;
foreach (var line in input)
{
    var cacheDict = new Dictionary<(int index, int hashValues, int comboIndex), long>();
    sum += RunPossibilities(line.line, line.combo, 0, 0, 0, line.combo.Sum(), cacheDict);
}
Console.WriteLine($"Part One: {sum}");
Console.WriteLine($"Part Two: {RunPartTwo()}");

long RunPartTwo()
{
    long sum = 0;
    var cacheDict = new Dictionary<(int index, int hashValues, int comboIndex), long>();

    for (int i = 0; i < input.Count; i++)
    {
        var line = input[i];
        string newLine = line.line + "?" + line.line + "?" + line.line + "?" + line.line + "?" + line.line;

        List<int> combo = new List<int>(line.combo);
        for (int j = 0; j < 4; j++) combo.AddRange(line.combo);

        cacheDict.Clear();
        sum += RunPossibilities(newLine, combo, 0, 0, 0, combo.Sum(), cacheDict);
    }

    return sum;
}

long RunPossibilities(string line, List<int> comboRequired, int indexToRunFrom, int currentHashVals,
    int comboIndex, int hashesRemaining, Dictionary<(int index, int hashValues, int comboIndex), long> cache)
{
    if (cache.ContainsKey((indexToRunFrom, currentHashVals, comboIndex))) return cache[(indexToRunFrom, currentHashVals, comboIndex)];

    if (indexToRunFrom == line.Length || line.Length - indexToRunFrom < hashesRemaining + comboRequired.Count - comboIndex - 1)
    {
        return comboIndex >= comboRequired.Count || (comboIndex + 1 == comboRequired.Count && comboRequired[comboIndex] == currentHashVals) ? 1 : 0;
    }

    long retVal = 0;
    switch (line[indexToRunFrom])
    {
        case '.':
            {
                retVal = RunDot(line, comboRequired, indexToRunFrom, currentHashVals, comboIndex, hashesRemaining, cache);
                break;
            }

        case '#':
            {
                retVal = RunHash(line, comboRequired, indexToRunFrom, currentHashVals, comboIndex, hashesRemaining, cache);
                break;
            }

        case '?':
            {
                retVal = RunDot(line, comboRequired, indexToRunFrom, currentHashVals, comboIndex, hashesRemaining, cache)
                    + RunHash(line, comboRequired, indexToRunFrom, currentHashVals, comboIndex, hashesRemaining, cache);
                break;
            }
    }

    cache.Add((indexToRunFrom, currentHashVals, comboIndex), retVal);
    return retVal;
}

long RunDot(string line, List<int> comboRequired, int indexToRunFrom, int currentHashVals,
    int comboIndex, int hashesRemaining, Dictionary<(int index, int hashValues, int comboIndex), long> cache)
{
    if (currentHashVals != 0)
    {
        if (currentHashVals != comboRequired[comboIndex]) return 0;
        currentHashVals = 0;
        comboIndex++;
    }

    return RunPossibilities(line, comboRequired, indexToRunFrom + 1, currentHashVals, comboIndex, hashesRemaining, cache);
}

long RunHash(string line, List<int> comboRequired, int indexToRunFrom, int currentHashVals,
    int comboIndex, int hashesRemaining, Dictionary<(int index, int hashValues, int comboIndex), long> cache)
{
    if (comboIndex >= comboRequired.Count || ++currentHashVals > comboRequired[comboIndex] || --hashesRemaining < 0) return 0;
    return RunPossibilities(line, comboRequired, indexToRunFrom + 1, currentHashVals, comboIndex, hashesRemaining, cache);
}
