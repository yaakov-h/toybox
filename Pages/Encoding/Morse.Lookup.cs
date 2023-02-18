using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Toybox.Pages;

partial class MorseModel
{
    public static ImmutableDictionary<char, string> InternationalMorseCode { get; } = new Dictionary<char, string>()
    {
        ['A'] = ".-",
        ['B'] = "-...",
        ['C'] = "-.-.",
        ['D'] = "-..",
        ['E'] = ".",
        ['F'] = "..-.",
        ['G'] = "--.",
        ['H'] = "....",
        ['I'] = "..",
        ['J'] = ".---",
        ['K'] = "-.-",
        ['L'] = ".-..",
        ['M'] = "--",
        ['N'] = "-.",
        ['O'] = "---",
        ['P'] = ".--.",
        ['Q'] = "--.-",
        ['R'] = ".-.",
        ['S'] = "...",
        ['T'] = "-",
        ['U'] = "..-",
        ['V'] = "...-",
        ['W'] = ".--",
        ['X'] = "-..-",
        ['Y'] = "-.--",
        ['Z'] = "--.-",
        ['1'] = ".----",
        ['2'] = "..---",
        ['3'] = "...--",
        ['4'] = "....-",
        ['5'] = ".....",
        ['6'] = "-....",
        ['7'] = "--...",
        ['8'] = "---..",
        ['9'] = "----.",
        ['0'] = "-----",
    }.ToImmutableDictionary(new CharInvariantInsensitiveComparer());

    sealed class CharInvariantInsensitiveComparer : IEqualityComparer<char>
    {
        public bool Equals(char x, char y)
            => Normalize(x) == Normalize(y);

        public int GetHashCode([DisallowNull] char obj)
            => Normalize(obj).GetHashCode();

        static char Normalize(char c)
            => char.ToUpper(c, CultureInfo.InvariantCulture);
    }
}