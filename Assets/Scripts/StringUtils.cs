

public static class StringUtils
{
    public static string SplitPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var result = new System.Text.StringBuilder();
        result.Append(input[0]);

        for (int i = 1; i < input.Length; i++)
        {
            char current = input[i];
            char previous = input[i - 1];

            bool currentIsUpper = char.IsUpper(current);
            bool previousIsUpper = char.IsUpper(previous);

            if (currentIsUpper && char.IsLower(previous))
            {
                result.Append(' ');
            }
            else if (currentIsUpper && previousIsUpper && i + 1 < input.Length && char.IsLower(input[i + 1]))
            {
                result.Append(' ');
            }

            result.Append(current);
        }

        return result.ToString();
    }
}
