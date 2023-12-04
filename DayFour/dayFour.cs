var input = File.ReadAllLines("input.txt").Select(str => str.Remove(0, str.IndexOf(':') + 1).Split('|'));
var gameSplit = input.Select(SplitInputs);

int score = 0;
var cardsHeld = Enumerable.Repeat(1, gameSplit.Count()).ToList();
for (int i = 0; i < gameSplit.Count(); i++)
{
  (var winning, var heldNumbers) = gameSplit.ElementAt(i);

  var matched = winning.Where(heldNumbers.Contains).Count();
  for(int j = 1; j <= matched; j++)
  {
    cardsHeld[i + j] += cardsHeld[i];
  }

  score += matched > 0 ? (int)Math.Pow(2, matched - 1) : 0;
}

Console.WriteLine($"Part One: {score}");

var cardNumberHeld = cardsHeld.Sum();
Console.WriteLine($"Part Two: {cardNumberHeld}");

(List<int> winning, HashSet<int> heldNumbers) SplitInputs(string[] inputs)
{
  var winning = inputs[0].Split(' ').Where(str => !string.IsNullOrEmpty(str)).Select(int.Parse).ToList();
  var heldNumbers = inputs[1].Split(' ').Where(str => !string.IsNullOrEmpty(str)).Select(int.Parse).ToHashSet();
  return (winning, heldNumbers);
}
