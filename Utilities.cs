using System.Globalization;

public static class Utilities
{
    public static string ReadString(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please input a valid name:");
            } else
            {
                return input;
            }
        }
    }

    public static DateTime ReadDate(string date)
    {
        while (true)
        {
            DateTime d;
            if (string.IsNullOrEmpty(date))
            {
                d = DateTime.Now;
                return d;
            } else
            if(DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            {
                return d;
            } else
            {
                Console.WriteLine("Please enter the date in a valid format (yyyy-mm-dd)");
                date = Console.ReadLine();
            }
        }
    }

    public static int ReadInt(string choice)
    {
        int ParsedInt;
        while (true)
        {
            if(int.TryParse(choice, out ParsedInt))
            {
                return ParsedInt;
            } else
            {
                Console.WriteLine("Please enter a valid option:");
                choice = Console.ReadLine();
            }
        }
    }
}