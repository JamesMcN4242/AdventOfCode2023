var input = File.ReadAllLines("input.txt").Select(str => str.Split(' '));
var rankedSet = new List<(int rank, string cards, int bid)>(input.Count());
PartOne();
PartTwo();

void PartOne()
{
  foreach (var inp in input)
  {
    var cards = inp[0];
    var bid = int.Parse(inp[1]);
    var grouped = cards.GroupBy(c => c).Select(g => (g.Key, g.Count())).ToList();
    EstablishRank(grouped, cards, bid);
  }

  Console.WriteLine($"Part One: {CalculateScore(true)}");
}

void PartTwo()
{
  rankedSet.Clear();

  foreach (var inp in input)
  {
    var cards = inp[0];
    var bid = int.Parse(inp[1]);
    var grouped = cards.GroupBy(c => c).Select(g => (g.Key, g.Count())).ToList();
    var jokers = grouped.Find(x => x.Item1 == 'J');

    if (jokers != default && grouped.Count > 1)
    {
      grouped.Remove(jokers);
      grouped.Sort((x, y) => y.Item2.CompareTo(x.Item2));

      var item = grouped[0];
      item.Item2 += jokers.Item2;
      grouped[0] = item;
    }

    EstablishRank(grouped, cards, bid);
  }

  Console.WriteLine($"Part Two: {CalculateScore(false)}");
}

int CompareHandValues(string cardsOne, string cardsTwo, bool partOne)
{
  for (int i = 0; i < cardsOne.Length; ++i)
  {
    if (cardsOne[i] == cardsTwo[i]) continue;

    if (!partOne)
    {
      if (cardsOne[i] == 'J') return 1;
      if (cardsTwo[i] == 'J') return -1;
    }

    if (cardsOne[i] > '1' && cardsOne[i] <= '9')
    {
      if (cardsTwo[i] > '1' && cardsTwo[i] <= '9') return Math.Sign(cardsOne[i] - cardsTwo[i]) * -1;
      return 1;
    }
    else if (cardsTwo[i] > '1' && cardsTwo[i] <= '9') return -1;
    else
    {
      // Ten card
      if (cardsOne[i] == 'T') return 1;
      if (cardsTwo[i] == 'T') return -1;

      // Face cards
      return Enum.Parse<FaceCards>(cardsOne[i].ToString()) > Enum.Parse<FaceCards>(cardsTwo[i].ToString()) ? -1 : 1;
    }
  }

  return 0;
}

void EstablishRank(List<(char, int)> grouped, string cards, int bid)
{
  switch (grouped.Count)
  {
    case 1:
      rankedSet.Add((7, cards, bid));
      break;

    case 2:
      rankedSet.Add((grouped[0].Item2 == 1 || grouped[0].Item2 == 4 ? 6 : 5, cards, bid));
      break;

    case 3:
      rankedSet.Add((grouped.Any(x => x.Item2 == 3) ? 4 : 3, cards, bid));
      break;

    case 4:
      rankedSet.Add((2, cards, bid));
      break;

    case 5:
      rankedSet.Add((1, cards, bid));
      break;
  }
}

int CalculateScore(bool partOne)
{
  rankedSet.Sort((x, y) => x.rank == y.rank ? CompareHandValues(x.cards, y.cards, partOne) : y.rank.CompareTo(x.rank));

  int sum = 0;
  for (int i = 1; i <= rankedSet.Count; ++i)
  {
    sum += rankedSet[^i].bid * i;
  }
  return sum;
}

enum FaceCards
{
  J = 0,
  Q,
  K,
  A
}
