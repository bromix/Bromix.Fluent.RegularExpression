namespace Bromix.Fluent.RegularExpression;

/// <summary>
/// Represents a quantifier for a regular expression pattern.
/// </summary>
public sealed class Quantifier
{
    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents no quantifier.
    /// </summary>
    public static readonly Quantifier None = new(string.Empty);

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents zero or more occurrences of the previous element.
    /// </summary>
    public static readonly Quantifier ZeroOrMore = new("*");

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents zero or more occurrences of the previous element, matched lazily.
    /// </summary>
    public static readonly Quantifier ZeroOrMoreLazy = new("*?");

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents one or more occurrences of the previous element.
    /// </summary>
    public static readonly Quantifier OneOrMore = new("+");

    /// <summary>
    /// /// Gets a <see cref="Quantifier"/> instance that represents one or more occurrences of the previous element, matched lazily.
    /// </summary>
    public static readonly Quantifier OneOrMoreLazy = new("+?");

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents zero or one occurrences of the previous element.
    /// </summary>
    public static readonly Quantifier Optional = new("?");

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents exactly the specified number of occurrences of the previous element.
    /// </summary>
    /// <param name="count">The number of occurrences.</param>
    /// <returns>A <see cref="Quantifier"/> instance that represents the specified number of occurrences.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than or equal to 0.</exception>
    public static Quantifier Exactly(int count)
    {
        return count switch
        {
            <= 0 => throw new ArgumentOutOfRangeException(nameof(count), "Value must be greater than zero."),
            1 => None,
            _ => new Quantifier($"{{{count}}}")
        };
    }

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents at least the specified number of occurrences of the previous element.
    /// </summary>
    /// <param name="count">The minimum number of occurrences.</param>
    /// <returns>A <see cref="Quantifier"/> instance that represents at least the specified number of occurrences.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 0.</exception>
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

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents at most the specified number of occurrences of the previous element.
    /// </summary>
    /// <param name="count">The maximum number of occurrences.</param>
    /// <returns>A <see cref="Quantifier"/> instance that represents at most the specified number of occurrences.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than or equal to 0.</exception>
    public static Quantifier AtMost(int count)
    {
        return count switch
        {
            <= 0 => throw new ArgumentOutOfRangeException(nameof(count), "Value must be greater than zero."),
            1 => Optional,
            _ => new Quantifier($"{{,{count}}}"),
        };
    }

    /// <summary>
    /// Gets a <see cref="Quantifier"/> instance that represents a specific range of occurrences of the preceding element.
    /// </summary>
    /// <param name="min">The minimum number of occurrences of the preceding element. Must be non-negative.</param>
    /// <param name="max">The maximum number of occurrences of the preceding element. Must be non-negative and greater than or equal to <paramref name="min"/>.</param>
    /// <returns>A <see cref="Quantifier"/> that matches a specific range of occurrences of the preceding element.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="min"/> or <paramref name="max"/> is negative, or when <paramref name="max"/> is less than <paramref name="min"/>.</exception>
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

    /// <summary>
    /// Returns the quantifier for a regular expression pattern.
    /// </summary>
    /// <returns>The quantifier for a regular expression pattern.</returns>
    public override string ToString() => _value;
    private Quantifier(string value) => _value = value;
    private readonly string _value;
}