var input = File.ReadAllLines("input.txt").Select(str => str.ToCharArray()).ToList();
// Runs the first north operation
PartOne();

// PartTwo should run N, W, S, E operations, and presumably cache the solutions in a bit mask like way to check if the configuration has been done already?
// Or maybe count the moves in the *last* NWSE formation, and when they match we could cache the state to see if that one is the same next time it occurs?
// That way we can get a remainder value of the runs still to be calculated to do the final N, W, S, E switches as required.
// For now I'm still a day behind so I'll do part one there and come back.

void PartOne()
{
    bool movement = true;
    while(movement)
    {
        movement = false;

        for (int i = 1; i < input.Count; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'O' && input[i - 1][j] == '.')
                {
                    input[i - 1][j] = 'O';
                    input[i][j] = '.';
                    movement = true;
                }
            }
        }
    }

    long sum = 0;
    for (int i = 0; i < input.Count; i++)
    {
        foreach (char c in input[i])
        {
            if (c == 'O')
            {
                sum += input.Count - i;
            }
        }
    }

    Console.WriteLine($"Part One: {sum}");
}
