var input = File.ReadAllLines("input.txt").Select(str => str.ToCharArray()).ToList();

// Runs the first north operation
PartOne();

// Reset the full input
input = File.ReadAllLines("input.txt").Select(str => str.ToCharArray()).ToList();
PartTwo();

void PartOne()
{
  RunNorth();
  Console.WriteLine($"Part One: {GetTotalLoad()}");
}

void PartTwo()
{
  int piecesMoved = 0;
  Dictionary<int, string[]> cache = new();
  Dictionary<int, ulong> lastMovedOn = new();

  for(ulong i = 0; i < 1000000000; i++)
  {
    int previousMoved = piecesMoved;
    int movedThisTime = RunFullCycle();

    // If we don't have that many iterations left we might as well run through them all to ensure we don't try skipping stuff again and getting further back.
    if (1000000000 - i < 50) continue;

    // Check if a cache already exists, replace if not the same, return response if it is.
    if (!cache.ContainsKey(movedThisTime))
    {
      cache[movedThisTime] = input.Select(line => new string(line)).ToArray();
      lastMovedOn[movedThisTime] = i;
    }
    else
    {
      string[] previousCached = cache[movedThisTime];
      bool allMatch = true;
      for (int j = 0; j < input.Count; j++)
      {
        if (previousCached[j] != new string(input[j]))
        {
          allMatch = false;
          break;
        }
      }

      if (allMatch)
      {
        i += (1000000000 - i) / (i - lastMovedOn[movedThisTime]) * (i - lastMovedOn[movedThisTime]);
      }
      else
      {
        // Change the cache value to the new one, and continue.
        cache[movedThisTime] = input.Select(line => new string(line)).ToArray();
        lastMovedOn[movedThisTime] = i;
      }
    }

    piecesMoved = movedThisTime;
  }

  Console.WriteLine($"Part Two: {GetTotalLoad()}");
}

int RunFullCycle()
{
  return RunNorth() + RunWest() + RunSouth() + RunEast();
};

int RunNorth()
{
  int moved = 0;
  bool movement = true;

  while (movement)
  {
    movement = false;

    for (int i = 1; i < input.Count; i++)
    {
      for (int j = 0; j < input[i].Length; j++)
      {
        if (input[i][j] == 'O' && input[i - 1][j] == '.')
        {
          ++moved;
          input[i - 1][j] = 'O';
          input[i][j] = '.';
          movement = true;
        }
      }
    }
  }

  return moved;
}

int RunSouth()
{
  int moved = 0;
  bool movement = true;

  while (movement)
  {
    movement = false;

    for (int i = input.Count - 2; i >= 0; i--)
    {
      for (int j = 0; j < input[i].Length; j++)
      {
        if (input[i][j] == 'O' && input[i + 1][j] == '.')
        {
          ++moved;
          input[i + 1][j] = 'O';
          input[i][j] = '.';
          movement = true;
        }
      }
    }
  }

  return moved;
}

int RunWest()
{
  int moved = 0;
  bool movement = true;

  while (movement)
  {
    movement = false;

    for (int i = 0; i < input.Count; ++i)
    {
      for (int j = 1; j < input[i].Length; j++)
      {
        if (input[i][j] == 'O' && input[i][j - 1] == '.')
        {
          ++moved;
          input[i][j - 1] = 'O';
          input[i][j] = '.';
          movement = true;
        }
      }
    }
  }

  return moved;
}

int RunEast()
{
  int moved = 0;
  bool movement = true;

  while (movement)
  {
    movement = false;

    for (int i = 0; i < input.Count; ++i)
    {
      for (int j = input[i].Length - 2; j >= 0; j--)
      {
        if (input[i][j] == 'O' && input[i][j + 1] == '.')
        {
          ++moved;
          input[i][j + 1] = 'O';
          input[i][j] = '.';
          movement = true;
        }
      }
    }
  }

  return moved;
}

long GetTotalLoad()
{
  long sum = 0;

  for (int i = 0; i < input.Count; i++)
  {
    foreach (char c in input[i])
    {
      if (c == 'O')
      {
        sum += input.Count - i;
      }
    }
  }

  return sum;
}
