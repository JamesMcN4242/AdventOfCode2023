var input = File.ReadAllLines("input.txt").Select(str => str.Remove(0, str.IndexOf(':') + 1).Trim()).ToList();
(var times, var distances) = (input[0].Split(' ').Where(str => !string.IsNullOrEmpty(str)).Select(int.Parse).ToList(),
  input[1].Split(' ').Where(str => !string.IsNullOrEmpty(str)).Select(int.Parse).ToList());

long multipliedWins = 1;
for (int i = 0; i < times.Count; i++)
{
  multipliedWins *= WinRange(times[i], distances[i]);
}

Console.WriteLine($"Part One: {multipliedWins}");

// Part Two - Surprisingly no change required since it still solves in ~130ms.
var time = long.Parse(input[0].Replace(" ", ""));
var dist = long.Parse(input[1].Replace(" ", ""));
Console.WriteLine($"Part Two: {WinRange(time, dist)}");

long WinRange(long time, long dist)
{
  int firstWin = -1;
  for (int secondHeld = 1; secondHeld < time; ++secondHeld)
  {
    bool winning = IsWinning(time, dist, secondHeld);
    if (winning)
    {
      if (firstWin == -1)
        firstWin = secondHeld;
    }
    else if (firstWin != -1)
    {
      return secondHeld - firstWin;
    }
  }

  return 0;
}

bool IsWinning(long time, long dist, long pointInTime) => (time - pointInTime) * pointInTime > dist;
