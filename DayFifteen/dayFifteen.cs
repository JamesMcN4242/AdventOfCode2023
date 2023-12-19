var input = File.ReadAllText("input.txt").Replace("\n", "").Replace("\r", "").Split(',');
long overAllSum = 0;
Dictionary<int, List<string>> lenses = new Dictionary<int, List<string>>(256);

foreach (var set in input)
{
  int sum = 0;
  bool lenseSumFinalised = false;
  int lenseSum = 0;
  string label = "";


  foreach (char c in set)
  {
    if (c == '-' || c == '=')
    {
      lenseSum = sum;
      lenseSumFinalised = true;
    }

    if (!lenseSumFinalised) label += c;


    sum = ((sum + c) * 17) % 256;
  }

  overAllSum += sum;
  
  List<string> lenseList = lenses.ContainsKey(lenseSum) ? lenses[lenseSum] : new List<string>();
  if (set.Contains('='))
  {
    int index = lenseList.FindIndex(str => str.StartsWith(label));
    if (index == -1)
      lenseList.Add(set);
    else
      lenseList[index] = set;
  }
  else
  {
    int index = lenseList.FindIndex(str => str.StartsWith(label));
    if (index != -1) lenseList.RemoveAt(index);
  }

  lenses[lenseSum] = lenseList;
}

Console.WriteLine($"Part One: {overAllSum}");

overAllSum = 0;
foreach(var lense in lenses)
{
  long focalLength = lense.Key + 1;

  for (int i = 0; i < lense.Value.Count; i++)
  {
    string set = lense.Value[i];
    int value = int.Parse(set.Substring(set.IndexOf('=') + 1));
    overAllSum += value * (i + 1) * focalLength;
  }
}

Console.WriteLine($"Part Two: {overAllSum}");
