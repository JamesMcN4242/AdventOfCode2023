var inputLines = File.ReadAllLines("input.txt");

var firstAndLastDigitsInLines = inputLines.Select(line => line.Split(' ').Select(x => x.First(IsIntChar).ToString() + x.Last(IsIntChar).ToString()));
var partOneSol = firstAndLastDigitsInLines.Sum(x => x.Select(int.Parse).Sum());
Console.WriteLine($"Part One: {partOneSol}");

var partTwoInput = Array.ConvertAll(inputLines, SubAndCullStrings);
var partTwoSections = partTwoInput.Select(str => (str[0].ToString() + str[^1].ToString()));
var partTwoSol = partTwoSections.Sum(x => int.Parse(x));
Console.WriteLine($"Part Two: {partTwoSol}");

bool IsIntChar(char c)
{
    return c >= '0' && c <= '9';
}

// It's only day one and I already feel lazy enough to do this as opposed to just two for statements since I'd need to _slightly_ think...
// That's a really bad sign of lack of motivation.
string SubAndCullStrings(string str)
{
  bool startsWithANumber = false;
  while(!startsWithANumber)
  {
    startsWithANumber = str.StartsWith("one") || str.StartsWith("two") || str.StartsWith("three") 
      || str.StartsWith("four") || str.StartsWith("five") || str.StartsWith("six") 
      || str.StartsWith("seven") || str.StartsWith("eight") || str.StartsWith("nine")
      || str.StartsWith('1') || str.StartsWith('2') || str.StartsWith('3')
      || str.StartsWith('4') || str.StartsWith('5') || str.StartsWith('6')
      || str.StartsWith('7') || str.StartsWith('8') || str.StartsWith('9');

    if(!startsWithANumber)
    {
      str = str.Substring(1);
    }
  }

  switch(str[0])
  {
    case 'o':
      str = "1" + str;
      break;

    case 't':
      str = str.StartsWith("two") ? "2" + str : "3" + str;
      break;

    case 'f':
      str = str.StartsWith("four") ? "4" + str : "5" + str;
      break;

    case 's':
      str = str.StartsWith("six") ? "6" + str : "7" + str;
      break;

    case 'e':
      str = "8" + str;
      break;

    case 'n':
      str = "9" + str;
      break;
  }
  
  bool endsWithANumber = false;
  while(!endsWithANumber)
  {
    endsWithANumber = str.EndsWith("one") || str.EndsWith("two") || str.EndsWith("three")
  || str.EndsWith("four") || str.EndsWith("five") || str.EndsWith("six")
  || str.EndsWith("seven") || str.EndsWith("eight") || str.EndsWith("nine")
  || str.EndsWith('1') || str.EndsWith('2') || str.EndsWith('3')
  || str.EndsWith('4') || str.EndsWith('5') || str.EndsWith('6')
  || str.EndsWith('7') || str.EndsWith('8') || str.EndsWith('9');

    if (!endsWithANumber)
    {
      str = str.Substring(0, str.Length - 1);
    }
  }

  switch (str[str.Length - 1])
  {
    case 'e':
      str = str.EndsWith("one") ? str + "1" : str.EndsWith("three") ? str + "3" : str.EndsWith("five") ? str + 5 : str + "9";
      break;

    case 'o':
      str = str + "2";
      break;

    case 'r':
      str = str + "4";
      break;

    case 'x':
      str = str + "6";
      break;

    case 'n':
      str = str + "7";
      break;

    case 't':
      str = str + "8";
      break;
  }

  return str;
}
