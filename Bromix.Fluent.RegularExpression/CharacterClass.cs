using System.Text;

namespace Bromix.Fluent.RegularExpression;

/// <summary>
/// Represents a character class in a regular expression pattern.
/// </summary>
public sealed class CharacterClass
{
    internal CharacterClass(Quantifier? quantifier = null) => _quantifier = quantifier ?? Quantifier.None;

    /// <summary>
    /// Adds a range of characters to the character class.
    /// </summary>
    /// <param name="start">The start character of the range.</param>
    /// <param name="end">The end character of the range.</param>
    /// <returns>The current <see cref="CharacterClass"/> instance.</returns>
    public CharacterClass Range(char start, char end)
    {
        _stringBuilder.Append($"{start}-{end}");
        return this;
    }

    /// <summary>
    /// Adds a set of characters to the character class.
    /// </summary>
    /// <param name="first">The first character to add.</param>
    /// <param name="second">The second character to add.</param>
    /// <param name="additional">Additional characters to add.</param>
    /// <returns>The current <see cref="CharacterClass"/> instance.</returns>
    public CharacterClass OneOf(char first, char second, params char[] additional)
    {
        var chars = new[] { first, second }.Concat(additional);
        foreach (var c in chars)
        {
            if (CharsToEscape.Contains(c))
            {
                _stringBuilder.Append($@"\{c}");
            }
            else
            {
                _stringBuilder.Append(c);
            }
        }

        return this;
    }

    /// <summary>
    /// Returns the character class as a string, including any specified <see cref="Quantifier"/>.
    /// </summary>
    /// <returns>A string representation of the character class.</returns>
    public override string ToString()
    {
        return $"[{_stringBuilder}]{_quantifier}";
    }

    private const string CharsToEscape = @"\^-[]";
    private readonly StringBuilder _stringBuilder = new();
    private readonly Quantifier _quantifier;
}