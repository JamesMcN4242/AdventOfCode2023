var input = File.ReadAllText("input.txt").Replace("\r", "").Split("\n\n");
var mappings = CreateMappings();
var seeds = input[0].Replace("seeds: ", "").Split(" ").Select(long.Parse).ToList();
PartOne();
PartTwo();

Dictionary<(InputType, InputType), List<Mapping>> CreateMappings()
{
  var mappings = new Dictionary<(InputType, InputType), List<Mapping>>();
  for (int i = 1; i < input.Length; i++)
  {
    var line = input[i];
    var parts = line.Split("\n");

    var fromString = parts[0].Substring(0, parts[0].IndexOf("-"));
    var from = Enum.Parse<InputType>(fromString, true);

    var toString = parts[0].Substring(parts[0].LastIndexOf("-") + 1, parts[0].IndexOf(' ') - parts[0].LastIndexOf("-"));
    var to = Enum.Parse<InputType>(toString, true);

    List<Mapping> mappingList = new List<Mapping>(parts.Length - 1);
    for (int j = 1; j < parts.Length; j++)
    {
      var numbers = parts[j].Split(" ").Select(long.Parse);
      var mapping = new Mapping
      {
        FromStart = numbers.ElementAt(1),
        FromEnd = numbers.ElementAt(1) + numbers.ElementAt(2) - 1,
        ToStart = numbers.ElementAt(0),
        ToEnd = numbers.ElementAt(0) + numbers.ElementAt(2) - 1
      };

      mappingList.Add(mapping);
    }

    mappings.Add((from, to), mappingList);
  }

  return mappings;
}

void PartOne()
{
  var minLocation = long.MaxValue;
  foreach (var seed in seeds)
  {
    minLocation = Math.Min(minLocation, GetLocationValue(seed));
  }

  Console.WriteLine($"Part One: {minLocation}");
}

void PartTwo()
{
  var minLocation = long.MaxValue;

  for (int i = 0; i < seeds.Count; i+=2)
  {
    for (int j = 0; j < seeds[i+1]; j++)
    {
      minLocation = Math.Min(minLocation, GetLocationValue(seeds[i] + j));
    }
  }

  Console.WriteLine($"Part Two: {minLocation}");
}

long GetLocationValue(long seed)
{
  var fromValue = seed;
  var from = InputType.Seed;

  while (from != InputType.Location)
  {
    var mapping = mappings[(from, from + 1)];
    var toUseMapping = mapping.FirstOrDefault(m => m.GetEndValue(fromValue).HasValue);
    fromValue = toUseMapping != null ? toUseMapping.GetEndValue(fromValue)!.Value : fromValue;
    ++from;
  }

  return fromValue;
}

// WIP: For now part two is just brute forced
//long GetBatchedLocationValue(long start, long end, InputType from)
//{
//  List<(long, long)> sections = new List<(long, long)>();
//  var map = mappings[(from, from + 1)];
//
//  while(start < end)
//  {
//    var mapForSection = map.FirstOrDefault(m => m.FromStart >= start && m.FromEnd <= end);
//    if (mapForSection != null)
//    {
//      var rangeCovered = mapForSection.GetRangeCoveredFromStart(start);
//      start = mapForSection.FromEnd > end ? end : mapForSection.FromEnd;
//      sections.Add((mapForSection.ToStart + rangeCovered, mapForSection.GetRangeCoveredFromStart(start) + mapForSection.ToStart));
//      start = mapForSection.FromEnd + 1;
//    }
//  }
//
//  return fromValue;
//}

enum InputType
{
  Seed,
  Soil,
  Fertilizer,
  Water,
  Light,
  Temperature,
  Humidity,
  Location
}

class Mapping
{
  public long FromStart;
  public long FromEnd;
  public long ToStart;
  public long ToEnd;

  public long? GetEndValue(long start)
  {
    if (start >= FromStart && start <= FromEnd)
    {
      return ToStart + (start - FromStart);
    }

    return null;
  }

  public long GetRangeCoveredFromStart(long start)
  {
    return start - FromStart;
  }
}
