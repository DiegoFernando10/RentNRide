using NanoidDotNet;

namespace RentNRide.Common.Id;

public class IdGenerator
{
    public const string FullAlphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_-";
    public const string OnlyNumberAlphabet = "0123456789";
    public const string OnlyLetterAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string LetterAndNumberAlphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static async Task<string> NewSync(int size = 6, string alphabet = LetterAndNumberAlphabet)
    {
        return await Nanoid.GenerateAsync(alphabet, size);
    }
}
