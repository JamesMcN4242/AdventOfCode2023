var input = File.ReadAllText("input.txt").Replace("\r", "").Split("\n\n");
long sum = 0;
List<long> previousScores = new List<long>();

for (int i = 0; i < input.Length; i++)
{
    string? line = input[i];
    var rows = line.Split('\n');
    long score = GetVerticalReflection(rows);
    score = score > 0 ? score : GetHorizontalMirror(rows) * 100;
    previousScores.Add(score);
    sum += score;
}

Console.WriteLine($"Part One: {sum}");

// Aight this section doesn't work, I really don't understand what the problem is asking for. We'll come back to it later maybe, or I'll enjoy life. Either one.
sum = 0;
for (int i1 = 0; i1 < input.Length; i1++)
{
    string line = input[i1];
    long score = 0;
    var rows = line.Split('\n');

    for (int i = 0; i < rows.Length; i++)
    {
        string before = rows[i];

        for (int j = 0; j < rows[i].Length; j++)
        {
            var charArr = before.ToCharArray();
            charArr[j] = charArr[j] == '#' ? '.' : '#';
            rows[i] = new string(charArr);

            score = GetVerticalReflection(rows);
            score = score != 0 && previousScores[i1] != score ? score : GetHorizontalMirror(rows) * 100;

            if (score != 0 && previousScores[i1] != score)
                break;
        }

        if (score != 0 && previousScores[i1] != score) break;
        rows[i] = before;
    }

    sum += score;
}
Console.WriteLine($"Part Two: {sum}");

int GetVerticalReflection(string[] rows)
{
    List<string> reverseRows = new List<string>(rows.Select(str => str[0].ToString()));
    for (int horizontalIndex = 1; horizontalIndex < rows[0].Length; ++horizontalIndex)
    {
        bool valid = true;
        for (int j = 0; j < rows.Length; ++j)
        {
            for (int k = 0; k < reverseRows[j].Length && k + horizontalIndex < rows[j].Length && valid; ++k)
            {
                if (reverseRows[j][k] != rows[j][k + horizontalIndex])
                {
                    valid = false;
                    break;
                }
            }

            reverseRows[j] = rows[j][horizontalIndex] + reverseRows[j];
        }

        if (valid)
        {
            return horizontalIndex;
        }
    }

    return 0;
}

long GetHorizontalMirror(string[] rows)
{
    List<string> reverseColumns = new List<string>(rows.Length);
    for(int i = 0; i < rows[0].Length; ++i)
    {
        reverseColumns.Add(rows[0][i].ToString());
    }

    for (int verticalIndex = 1; verticalIndex < rows.Length; ++verticalIndex)
    {
        bool valid = true;

        for (int offset = 0; offset + verticalIndex < rows.Length; ++offset)
        {
            for (int xIndex = 0; xIndex < reverseColumns.Count && offset < reverseColumns[xIndex].Length; ++xIndex)
            {
                if (valid && reverseColumns[xIndex][offset] != rows[offset + verticalIndex][xIndex])
                {
                    valid = false;
                }
            }
        }

        for (int xIndex = 0; xIndex < reverseColumns.Count; ++xIndex)
        {
            reverseColumns[xIndex] = rows[verticalIndex][xIndex].ToString() + reverseColumns[xIndex];
        }

        if (valid)
        {
            return verticalIndex;
        }
    }

    return 0;
}
