var input = File.ReadAllText("input.txt").Replace("\n", "").Replace("\r", "").Split(',');
PartOne();

void PartOne()
{
    long overAllSum = 0;

    foreach (var set in input)
    {
        long sum = 0;

        foreach(char c in set)
        {
            sum = ((sum + c) * 17) % 256;
        }

        overAllSum += sum;
    }

    Console.WriteLine($"Part One: {overAllSum}");
}
