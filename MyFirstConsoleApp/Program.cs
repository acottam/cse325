// Hello World output
Console.WriteLine("Hello, World!");
Console.WriteLine($"The current time is {DateTime.Now}");

// Days Until Christmas output
Console.WriteLine($"There are {GetDaysUntilChristmas()} days until the next Christmas.");

// Get Days Until Christmas method
static int GetDaysUntilChristmas()
{
    DateTime today = DateTime.Today;
    DateTime nextChristmas = new DateTime(today.Year, 12, 25);
    if (nextChristmas < today)
    {
        nextChristmas = nextChristmas.AddYears(1);
    }
    TimeSpan difference = nextChristmas - today;
    return difference.Days;
}
