namespace StudentApp.Helpers;

public static class InputHelper
{
    public static int GetInt(string message, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();
            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }
            Console.WriteLine($"⚠️ Please enter a valid number between {min} and {max}.");
        }
    }
    public static string GetString(string message, bool required = true, int maxLength = 100)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(input))
            {
                if (!required)
                    return "";
                Console.WriteLine("⚠️ This field is required.");
                continue;
            }
            if (input.Length > maxLength)
            {
                Console.WriteLine($"⚠️ Text too long! Max {maxLength} characters allowed.");
                continue;
            }
            return input;
        }

    }
}