var boardInput = File.ReadAllLines("input.txt");
var partOne = 0;
var gears = new Dictionary<(int x, int y), List<int>>();

HashSet<(int x, int y)> gearsHit = new HashSet<(int x, int y)>();
for (int i = 0; i < boardInput.Length; i++)
{
    var currentNumber = 0;
    bool hitSymbol = false;

    for (int j = 0; j < boardInput[i].Length; j++)
    {
        bool wasNumber = boardInput[i][j] >= '0' && boardInput[i][j] <= '9';
        if (wasNumber)
        {
            currentNumber *= 10;
            currentNumber += int.Parse(boardInput[i][j].ToString());
            
            var symbolInfo = SymbolSurrounds(boardInput, i, j);
            hitSymbol |= symbolInfo.symbol;

            if (symbolInfo.gear != null)
            {
                gearsHit.Add(symbolInfo.gear.Value);
            }
        }

        if (currentNumber != 0 && (!wasNumber || j == boardInput[i].Length - 1))
        {
            if (hitSymbol)
            {                
                partOne += currentNumber;
                foreach (var item in gearsHit)
                {
                    var list = gears.TryGetValue(item, out var existent) ? existent : [];
                    list.Add(currentNumber);
                    gears[item] = list;
                }
            }

            currentNumber = 0;
            hitSymbol = false;
            gearsHit.Clear();
        }
    }
}

Console.WriteLine($"Part One: {partOne}");

var partTwo = gears.Where(x => x.Value.Count == 2).Select(x => x.Value[0] * x.Value[1]).Sum();
Console.WriteLine($"Part Two: {partTwo}");

(bool symbol, (int, int)? gear) SymbolSurrounds(string[] board, int i, int j)
{
    // Y-Axis
    for (int  i2 = i - 1;  i2 <= i + 1; i2++)
    {
        if (i2 < 0 || i2 >= board.Length) continue;

        // X-Axis
        for(int j2 = j - 1; j2 <= j + 1; j2++)
        {
            if (j2 < 0 || j2 >= board[i2].Length) continue;

            if (board[i2][j2] != '.' && (board[i2][j2] < '0' || board[i2][j2] > '9'))
            {
                return (true, board[i2][j2] == '*' ? (i2, j2) : null);
            }
        }
    }

    return (false, null);
}
