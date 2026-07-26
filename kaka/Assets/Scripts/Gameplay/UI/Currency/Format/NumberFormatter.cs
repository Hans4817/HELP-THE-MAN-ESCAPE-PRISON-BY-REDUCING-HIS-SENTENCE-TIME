public static class NumberFormatter
{
    private static readonly string[] Suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

    public static string Format(double value)
    {
        if (value < 1000) return value.ToString("F0");

        int index = 0;
        while (value >= 1000 && index < Suffixes.Length - 1)
        {
            value /= 1000;
            index++;
        }

        return $"{value:F2}{Suffixes[index]}"; // e.g., 1.50M or 2.34B
    }
}