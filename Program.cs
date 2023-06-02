using System;
using System.Linq;

class Program
{
    static string[] multipliers = { "x15", "x16", "x17", "x18", "x19", "x20" };
    static string[,] grid = new string[3, 3];
    static bool[,] revealed = new bool[3, 3];
    static decimal betAmount = 0;
    static bool gameEnded = false;
    static bool escapePressed = false;

    static void Main()
    {
        while (true)
        {
            Console.Write("Enter the bet amount: ");
            string? betAmountString = Console.ReadLine();

            // Validate and parse the bet amount entered by the user
            if (betAmountString is not null && decimal.TryParse(betAmountString, out betAmount) && betAmount > 0)
            {
                // Bet amount is valid, proceed with the game
                InitializeGrid();
                DrawGrid();
                Console.WriteLine("Press [Space] to spin");

                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (!gameEnded)
                    {
                        if (keyInfo.Key == ConsoleKey.Spacebar)
                        {
                            // Reveal a random multiplier
                            RevealRandomMultiplier();
                        }
                        else if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            escapePressed = true;
                            break;
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        escapePressed = true;
                        break;
                    }
                }

                if (!escapePressed)
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }

                break;
            }
            else
            {
                Console.WriteLine("Invalid bet amount. Please enter a valid positive number.");
                continue;
            }
        }
    }
    static void InitializeGrid()
    {
        // Randomly select 3 multipliers from the available options
        Random random = new Random();
        string[] selectedMultipliers = multipliers.OrderBy(x => random.Next()).Take(3).ToArray();

        // Randomly assign the selected multipliers to the grid positions
        int[] positions = Enumerable.Range(0, 9).OrderBy(x => random.Next()).ToArray();
        int multiplierIndex = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            int row = positions[i] / 3;
            int col = positions[i] % 3;

            grid[row, col] = selectedMultipliers[multiplierIndex];
            revealed[row, col] = false;

            multiplierIndex = (multiplierIndex + 1) % 3;
        }
    }

    static void DrawGrid()
    {
        // Draw the grid with the revealed multipliers or slot numbers
        Console.WriteLine("----------------");

        int slotNumber = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (revealed[i, j])
                {
                    Console.Write($"| {grid[i, j]} ");
                }
                else
                {
                    Console.Write($"| {slotNumber} ");
                }
                slotNumber++;
            }
            Console.WriteLine("|");
            Console.WriteLine("----------------");
        }
    }

    static void RevealRandomMultiplier()
    {
        // Randomly select a position that has not been revealed
        Random random = new Random();
        int position;

        do
        {
            position = random.Next(1, 10);
        } while (IsPositionRevealed(position));

        // Calculate the row and column based on the position selected
        int row = (position - 1) / 3;
        int col = (position - 1) % 3;

        // Mark the selected position as revealed
        revealed[row, col] = true;

        // Clear the console and display the updated grid with the revealed multiplier
        Console.Clear();
        Console.WriteLine($"Initial bet amount: {betAmount}");
        DrawGrid();

        Console.WriteLine($"Multiplier {grid[row, col]} revealed at position {position}");

        // Check if the revealed multiplier appears twice in the grid
        if (IsMultiplierAppearedTwice(grid[row, col]))
        {
            // Calculate the total amount won based on the bet amount and multiplier value
            decimal totalAmount = betAmount * GetMultiplierValue(grid[row, col]);

            Console.WriteLine("");
            Console.WriteLine($"Player has won {totalAmount} of their initial bet amount.");
            Console.WriteLine("Press [Esc] to exit");

            gameEnded = true;
        }
        else
        {
            Console.WriteLine("Press [Space] to spin");
        }
    }

    static bool IsMultiplierAppearedTwice(string multiplier)
    {
        // Count the number of times the multiplier appears in the revealed positions
        int count = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (revealed[i, j] && grid[i, j] == multiplier)
                {
                    count++;
                }
            }
        }

        // Check if the multiplier appeared twice
        return count == 2;
    }

    static bool IsPositionRevealed(int position)
    {
        // Calculate the row and column based on the position
        int row = (position - 1) / 3;
        int col = (position - 1) % 3;

        // Check if the position is already revealed
        return revealed[row, col];
    }

    static decimal GetMultiplierValue(string multiplier)
    {
        // Get the corresponding multiplier value
        switch (multiplier)
        {
            case "x15":
                return 15;
            case "x16":
                return 16;
            case "x17":
                return 17;
            case "x18":
                return 18;
            case "x19":
                return 19;
            case "x20":
                return 20;
            default:
                return 1;
        }
    }
}