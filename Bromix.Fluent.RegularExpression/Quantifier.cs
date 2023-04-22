namespace Bromix.Fluent.RegularExpression;

public sealed class Quantifier
{
    public static readonly Quantifier None = new(string.Empty);
    public static readonly Quantifier ZeroOrMore = new("*");
    public static readonly Quantifier ZeroOrMoreLazy = new("*?");
    public static readonly Quantifier OneOrMore = new("+");
    public static readonly Quantifier OneOrMoreLazy = new("+?");
    public static readonly Quantifier Optional = new("?");

    public static Quantifier Exactly(int count)
    {
        return count switch
        {
            <= 0 => throw new ArgumentOutOfRangeException(nameof(count), "Value must be greater than zero."),
            1 => None,
            _ => new Quantifier($"{{{count}}}")
        };
    }

    public static Quantifier AtLeast(int count)
    {
        return count switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(count), "Value must be non-negative."),
            0 => ZeroOrMore,
            1 => OneOrMore,
            _ => new Quantifier($"{{{count},}}"),
        };
    }

    public static Quantifier AtMost(int count)
    {
        return count switch
        {
            <= 0 => throw new ArgumentOutOfRangeException(nameof(count), "Value must be greater than zero."),
            1 => Optional,
            _ => new Quantifier($"{{,{count}}}"),
        };
    }

    public static Quantifier Range(int min, int max)
    {
        if (min < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(min), "Value must be non-negative.");
        }

        if (max < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(max), "Value must be non-negative.");
        }

        if (max < min)
        {
            throw new ArgumentOutOfRangeException(nameof(max), "Value max cannot be less than value min.");
        }

        if (min == max)
        {
            return Exactly(min);
        }

        return min switch
        {
            0 when max == 1 => Optional,
            0 => AtMost(max),
            _ => new Quantifier($"{{{min},{max}}}")
        };
    }

    public override string ToString() => _value;
    private Quantifier(string value) => _value = value;
    private readonly string _value;
}