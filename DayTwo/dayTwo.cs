var gameInputs = File.ReadAllLines("input.txt").Select(line => line.Split(':'));

int scorePartOne = 0;
int scorePartTwo = 0;

foreach (var gameInput in gameInputs)
{
    var result = IsGameRoundPossible(gameInput[1]);

    if (result.possible)
        scorePartOne += int.Parse(gameInput[0].Remove(0, "Game ".Length));

    scorePartTwo += result.minimalPower;
}

Console.WriteLine($"Part One: {scorePartOne}");
Console.WriteLine($"Part Two: {scorePartTwo}");

(bool possible, int minimalPower) IsGameRoundPossible(string gameInput)
{
    bool possible = true;
    int minimumRed = 0;
    int minimumGreen = 0;
    int minimumBlue = 0;

    var rounds = gameInput.Replace(" ", "").Split(";");
    foreach (var round in rounds)
    {
        var colours = round.Split(',');
        foreach (var colour in colours)
        {
            if (colour.EndsWith("red"))
            {
                int numberPresent = int.Parse(colour.Replace("red", ""));
                possible &= numberPresent <= 12;
                minimumRed = minimumRed < numberPresent ? numberPresent : minimumRed;
            }
            else if (colour.EndsWith("green"))
            {
                int numberPresent = int.Parse(colour.Replace("green", ""));
                possible &= numberPresent <= 13;
                minimumGreen = minimumGreen < numberPresent ? numberPresent : minimumGreen;
            }
            else if (colour.EndsWith("blue"))
            {
                int numberPresent = int.Parse(colour.Replace("blue", ""));
                possible &= numberPresent <= 14;
                minimumBlue = minimumBlue < numberPresent ? numberPresent : minimumBlue;
            }
        }
    }

    return (possible, minimumRed * minimumGreen * minimumBlue);
}
