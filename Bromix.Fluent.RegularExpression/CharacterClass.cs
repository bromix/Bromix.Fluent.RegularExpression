using System.Text;

namespace Bromix.Fluent.RegularExpression;

public sealed class CharacterClass
{
    internal CharacterClass(Quantifier? quantifier = null) => _quantifier = quantifier ?? Quantifier.None;

    public CharacterClass Range(char start, char end)
    {
        _stringBuilder.Append($"{start}-{end}");
        return this;
    }

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

    public override string ToString()
    {
        return $"[{_stringBuilder}]{_quantifier}";
    }

    private const string CharsToEscape = @"\^-[]";
    private readonly StringBuilder _stringBuilder = new();
    private readonly Quantifier _quantifier;
}