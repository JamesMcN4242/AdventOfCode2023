var input = File.ReadAllLines("input.txt"); 
Console.WriteLine($"Part One: {RunEnergisationFromPoint(CreateMap(), -1, 0, Direction.Right)}");

int max = 0;
int mapWidth = input[0].Length;
for (int i = 0; i < input.Length; i++)
{
  max = Math.Max(max, RunEnergisationFromPoint(CreateMap(), -1, i, Direction.Right));
  max = Math.Max(max, RunEnergisationFromPoint(CreateMap(), mapWidth, i, Direction.Left));
}

for (int i = 0; i < mapWidth; i++)
{ 
  max = Math.Max(max, RunEnergisationFromPoint(CreateMap(), i, -1, Direction.Down));
  max = Math.Max(max, RunEnergisationFromPoint(CreateMap(), i, input.Length, Direction.Up));
}
Console.WriteLine($"Part Two: {max}");

int RunEnergisationFromPoint(MapTiles[][] map, int x, int y, Direction dir)
{
  Queue<(int x, int y, Direction dir)> toProcess = new Queue<(int x, int y, Direction dir)>();
  toProcess.Enqueue((x, y, dir));

  while (toProcess.Count > 0)
  {
    var info = toProcess.Dequeue();
    var targetPos = GetNextPosition(info.x, info.y, info.dir);
    if (targetPos.x < 0 || targetPos.x >= map[0].Length || targetPos.y < 0 || targetPos.y >= map.Length
      || map[targetPos.y][targetPos.x].HasFlag(DirToMapTileVal(info.dir)))
    {
      continue;
    }

    var targetTile = map[targetPos.y][targetPos.x];
    if (targetTile.HasFlag(MapTiles.Empty))
    {
      toProcess.Enqueue((targetPos.x, targetPos.y, info.dir));
      map[targetPos.y][targetPos.x] |= DirToMapTileVal(info.dir);
    }
    else if (targetTile.HasFlag(MapTiles.LineSplitterVertical))
    {
      map[targetPos.y][targetPos.x] |= DirToMapTileVal(info.dir);
      if (info.dir == Direction.Up || info.dir == Direction.Down)
      {
        toProcess.Enqueue((targetPos.x, targetPos.y, info.dir));
      }
      else
      {
        toProcess.Enqueue((targetPos.x, targetPos.y, Direction.Up));
        toProcess.Enqueue((targetPos.x, targetPos.y, Direction.Down));
      }
    }
    else if (targetTile.HasFlag(MapTiles.LineSplitterHorizontal))
    {
      map[targetPos.y][targetPos.x] |= DirToMapTileVal(info.dir);
      if (info.dir == Direction.Left || info.dir == Direction.Right)
      {
        toProcess.Enqueue((targetPos.x, targetPos.y, info.dir));
      }
      else
      {
        toProcess.Enqueue((targetPos.x, targetPos.y, Direction.Left));
        toProcess.Enqueue((targetPos.x, targetPos.y, Direction.Right));
      }
    }
    else if (targetTile.HasFlag(MapTiles.MirrorLeftToDown))
    {
      map[targetPos.y][targetPos.x] |= DirToMapTileVal(info.dir);
      var newDir = info.dir switch
      {
        Direction.Up => Direction.Left,
        Direction.Down => Direction.Right,
        Direction.Left => Direction.Up,
        Direction.Right => Direction.Down,
      };
      toProcess.Enqueue((targetPos.x, targetPos.y, newDir));
    }
    else if (targetTile.HasFlag(MapTiles.MirrorLeftToUp))
    {
      map[targetPos.y][targetPos.x] |= DirToMapTileVal(info.dir);
      var newDir = info.dir switch
      {
        Direction.Up => Direction.Right,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Down,
        Direction.Right => Direction.Up,
      };
      toProcess.Enqueue((targetPos.x, targetPos.y, newDir));
    }
    else
    {
      toProcess.Enqueue((targetPos.x, targetPos.y, info.dir));
      map[targetPos.y][targetPos.x] |= DirToMapTileVal(info.dir);
    }
  }

  return map.Sum(row => row.Count(col => col.HasFlag(MapTiles.LightRight) || col.HasFlag(MapTiles.LightLeft) || col.HasFlag(MapTiles.LightUp) || col.HasFlag(MapTiles.LightDown)));
}

MapTiles[][] CreateMap()
{
  var map = new MapTiles[input.Length][];
  for (var i = 0; i < input.Length; i++)
  {
    map[i] = new MapTiles[input[i].Length];
    for (var j = 0; j < input[i].Length; j++)
    {
      map[i][j] = input[i][j] switch
      {
        '.' => MapTiles.Empty,
        '|' => MapTiles.LineSplitterVertical,
        '-' => MapTiles.LineSplitterHorizontal,
        '/' => MapTiles.MirrorLeftToUp,
        '\\' => MapTiles.MirrorLeftToDown,
        '>' => MapTiles.LightRight,
        '<' => MapTiles.LightLeft,
        '^' => MapTiles.LightUp,
        'v' => MapTiles.LightDown,
        _ => throw new Exception("Unknown tile")
      };
    }
  }

  return map;
}

(int x, int y) GetNextPosition(int x, int y, Direction dir)
{
  switch(dir)
  {
    case Direction.Up:
      return (x, y - 1);

    case Direction.Down:
      return (x, y + 1);

    case Direction.Left:
      return (x - 1, y);

    case Direction.Right:
      return (x + 1, y);
  }

  return (-1, -1);
}

MapTiles DirToMapTileVal(Direction dir) => dir switch
{
  Direction.Up => MapTiles.LightUp,
  Direction.Down => MapTiles.LightDown,
  Direction.Left => MapTiles.LightLeft,
  Direction.Right => MapTiles.LightRight,
  _ => throw new Exception("Unknown direction")
};

void RenderMap(MapTiles[][] map)
{
  for (var i = 0; i < map.Length; i++)
  {
    for (var j = 0; j < map[i].Length; j++)
    {
      Console.Write(map[i][j] switch
      {
        MapTiles.Empty => '.',
        MapTiles.LineSplitterVertical => '|',
        MapTiles.LineSplitterHorizontal => '-',
        MapTiles.MirrorLeftToUp => '/',
        MapTiles.MirrorLeftToDown => '\\',
        MapTiles.LightRight => '>',
        MapTiles.LightLeft => '<',
        MapTiles.LightUp => '^',
        MapTiles.LightDown => 'v',
        _ => 'X' // Multiple in this case
      });
    }

    Console.WriteLine();
  }

  Console.WriteLine();
  Console.WriteLine();
}

[Flags]
enum MapTiles
{
  Empty = 1 << 0,
  LineSplitterVertical = 1 << 1,
  LineSplitterHorizontal = 1 << 2,
  MirrorLeftToDown = 1 << 3,
  MirrorLeftToUp = 1 << 4,

  LightRight = 1 << 5,
  LightLeft = 1 << 6,
  LightUp = 1 << 7,
  LightDown = 1 << 8
}

enum Direction
{
  Up,
  Down,
  Left,
  Right
}
