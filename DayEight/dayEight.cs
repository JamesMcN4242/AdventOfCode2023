var input = File.ReadAllLines("input.txt").Where(str => !string.IsNullOrEmpty(str)).ToList();
var mapDict = input.TakeLast(input.Count - 1)
    .Select(str => (
        key: str.Substring(0, str.IndexOf(' ')),
        left: str.Substring(str.IndexOf('(') + 1, 3),
        right: str.Substring(str.Length - 4, 3)))
    .ToDictionary(inp => inp.Item1);

var instructions = input[0];
PartOne("AAA");
PartTwo();

void PartOne(string currentNode)
{
    int steps;
    for (steps = 0; currentNode != "ZZZ"; ++steps)
    {
        var dir = instructions[steps % instructions.Length];
        currentNode = dir == 'L' ? mapDict[currentNode].left : mapDict[currentNode].right;
    }

    Console.WriteLine($"Part One: {steps}");
}

void PartTwo()
{
    var offsets = ZOffsetOccurences(mapDict.Where(content => content.Key.EndsWith('A')).Select(content => content.Key).ToList());
    long stepSize = offsets[0];
    long leastCommonMultiple = 0;
    int i = 1;

    // I remember there being some formula for doing this _smart_, but I'm at an airport and lacking good internet to look it up.
    // So here is my "closest" LCM approach.
    while (i != offsets.Count)
    {
        leastCommonMultiple += stepSize;
        for (i = 1; i < offsets.Count; ++i)
        {
            if (leastCommonMultiple % offsets[i] > 0) break;
        }
    }

    Console.WriteLine($"Part Two: {leastCommonMultiple}");
}

List<long> ZOffsetOccurences(List<string> currentNodes)
{
    return currentNodes.Select(ZOffsetOccurence).ToList();
}

long ZOffsetOccurence(string startNode)
{
    Dictionary<string, long> lastHit = new();
    for (int steps = 0; true; ++steps)
    {
        var dir = instructions[steps % instructions.Length];
        startNode = dir == 'L' ? mapDict[startNode].left : mapDict[startNode].right;
        
        if (startNode.EndsWith('Z'))
        {
            if (lastHit.TryGetValue(startNode, out long firstHit))
            {
                return steps - firstHit;
            }
            else
            {
                lastHit.Add(startNode, steps);
            }
        }
    }
}
