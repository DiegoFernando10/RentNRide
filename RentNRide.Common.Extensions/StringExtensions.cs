namespace RentNRide.Common.Extensions;

public static class StringExtensions
{
    public static string GetNumbers(this string value)
    {
        var tmp = string.Empty;

        if (!string.IsNullOrEmpty(value))
        {
            for (var i = 0; i < value.Length; i++)
                if (char.IsDigit(value[i]))
                    tmp += value[i];
        }

        return tmp;
    }

}
