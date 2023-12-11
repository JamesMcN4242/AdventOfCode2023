var input = File.ReadAllLines("input.txt").ToList();

// Manually setting this since I don't care to handle *all* possible cases for subbing with S is later on - which would be essential for part 2.
var startPos = (63, 118);

// Format as a List<List<string>> for simplicity in substituting values
var map = input.Select(str => str.Select(ch => ch.ToString()).ToList()).ToList();
var canConnectDict = new Dictionary<string, HashSet<char>>()
{
    { "-L", new HashSet<char>() {'-', 'J', '7' } },
    { "-R", new HashSet<char>() {'-', 'F', 'L' } },
    { "|D", new HashSet<char>() {'|', 'F', '7' } },
    { "|U", new HashSet<char>() {'|', 'J', 'L' } },
    { "JR", new HashSet<char>() {'-', 'F', 'L' } },
    { "JD", new HashSet<char>() {'|', 'F', '7' } },
    { "7R", new HashSet<char>() {'-', 'F', 'L' } },
    { "7U", new HashSet<char>() {'|', 'L', 'J' } },
    { "LL", new HashSet<char>() {'-', 'J', '7' } },
    { "LD", new HashSet<char>() {'|', '7', 'F' } },
    { "FL", new HashSet<char>() {'-', 'J', '7' } },
    { "FU", new HashSet<char>() {'|', 'J', 'L' } },
};

var nextToCheck = new List<(int x, int y)>() { startPos };
var checking = new List<(int x, int y)>();
int step = 0;
for (; nextToCheck.Count > 0; ++step)
{
    (checking, nextToCheck) = (nextToCheck, checking);
    nextToCheck.Clear();

    for (int i = 0; i < checking.Count; ++i)
    {
        var coord = checking[i];
        switch (map[coord.y][coord.x][0])
        {
            case '-':
                CheckAndAddLabel(step, coord.x + 1, coord.y, canConnectDict["-L"]);
                CheckAndAddLabel(step, coord.x - 1, coord.y, canConnectDict["-R"]);
                break;

            case '|':
                CheckAndAddLabel(step, coord.x, coord.y + 1, canConnectDict["|U"]);
                CheckAndAddLabel(step, coord.x, coord.y - 1, canConnectDict["|D"]);
                break;

            case 'J':
                CheckAndAddLabel(step, coord.x - 1, coord.y, canConnectDict["JR"]);
                CheckAndAddLabel(step, coord.x, coord.y - 1, canConnectDict["JD"]);
                break;

            case 'F':
                CheckAndAddLabel(step, coord.x + 1, coord.y, canConnectDict["FL"]);
                CheckAndAddLabel(step, coord.x, coord.y + 1, canConnectDict["FU"]);
                break;

            case 'L':
                CheckAndAddLabel(step, coord.x + 1, coord.y, canConnectDict["LL"]);
                CheckAndAddLabel(step, coord.x, coord.y - 1, canConnectDict["LD"]); 
                break;

            case '7':
                CheckAndAddLabel(step, coord.x - 1, coord.y, canConnectDict["7R"]);
                CheckAndAddLabel(step, coord.x, coord.y + 1, canConnectDict["7U"]);
                break;

            case 'S':
                CheckAndAddLabel(step, coord.x - 1, coord.y, canConnectDict["-R"]);
                CheckAndAddLabel(step, coord.x + 1, coord.y, canConnectDict["-L"]);
                CheckAndAddLabel(step, coord.x, coord.y - 1, canConnectDict["|D"]);
                CheckAndAddLabel(step, coord.x, coord.y + 1, canConnectDict["|U"]);
                break;
        }
    }
}

Console.WriteLine($"Part One: {step - 1}");
for(int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (map[i][j].Length == 1)
        {
            var arr = input[i].ToCharArray();
            arr[j] = '0';
            input[i] = new string(arr);
        }
    }
}

// Let's try to use the "in and out" method that I've used in navigation systems before to define whether we're inside a polygon or not by essentially seeing how many lines we pass through.
int numberInside = 0;
for (int y = 0; y < input.Count; ++y)
{
    for (int x = 0; x < input[y].Length; ++x)
    {
        string line = input[y];
        char point = line[x];
        if (point == '0' && IsFullyEnclosed(x, y))
        {
            ++numberInside;
        }
    }
}

Console.WriteLine($"Part two: {numberInside}");


void CheckAndAddLabel(int i, int x, int y, HashSet<char> allowedChars)
{
    if (x < 0 || y < 0 || y >= map.Count || x >= map[y].Count) return;

    if (map[y][x].Length == 1 && allowedChars.Contains(map[y][x][0]))
    {
        map[y][x] += i.ToString();
        nextToCheck.Add((x, y));
    }
}

bool IsFullyEnclosed(int x, int y)
{
    var xLine = input[y];
    return IsEnclosedInXRange(xLine, 0, x) && IsEnclosedInXRange(xLine, x + 1, xLine.Length)
        && IsEnclosedInYRange(x, 0, y) && IsEnclosedInYRange(x, y + 1, input.Count);
}

bool IsEnclosedInXRange(string line, int start, int end)
{
    int numberOfWalls = 0;
    char? lastVerticalWas = null;

    for (int i = start; i < end; ++i)
    {
        switch (line[i])
        {
            case 'J':
                if (lastVerticalWas == 'F') { lastVerticalWas = null; break; }
                numberOfWalls++;
                lastVerticalWas = 'J';
                break;

            case '7':
                if (lastVerticalWas == 'L') { lastVerticalWas = null; break; }
                numberOfWalls++;
                lastVerticalWas = '7';
                break;

            case '|':
            case 'L':
            case 'F':
                numberOfWalls++;
                lastVerticalWas = line[i];
                break;

            case '-': break;

            default:
                lastVerticalWas = null;
                break;
        }
    }

    return numberOfWalls > 0 && numberOfWalls % 2 == 1;
}

bool IsEnclosedInYRange(int xLine, int start, int end)
{
    int numberOfWalls = 0;
    char? lastHorizontalWas = null;

    for (int i = start; i < end; ++i)
    {
        switch (input[i][xLine])
        {
            case 'L':
                if (lastHorizontalWas == '7') { lastHorizontalWas = null; break; }
                numberOfWalls++;
                lastHorizontalWas = 'L';
                break;

            case 'J':
                if (lastHorizontalWas == 'F') { lastHorizontalWas = null; break; }
                numberOfWalls++;
                lastHorizontalWas = 'J';
                break;

            case '-':
            case '7':
            case 'F':
                numberOfWalls++;
                lastHorizontalWas = input[i][xLine];
                break;

            case '|': break;

            default:
                lastHorizontalWas = null;
                break;
        }
    }

    return numberOfWalls > 0 && numberOfWalls % 2 == 1;
}
