using System.Text;

namespace Bromix.Fluent.RegularExpression;

/// <summary>
/// A <see cref="Pattern"/> object that represents the regular expression pattern.
/// </summary>
public sealed class Pattern
{
    /// <summary>
    /// Create a new <see cref="Pattern"/> object.
    /// </summary>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public static Pattern With() => new();

    private Pattern()
    {
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the first, second or and any additional characters provided. 
    /// </summary>
    /// <param name="first">The first possible character to match.</param>
    /// <param name="second">The second possible character to match.</param>
    /// <param name="additional">Any additional character to match.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern OneOf(char first, char second, params char[] additional)
    {
        return OneOf(Quantifier.None, first, second, additional);
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the first, second or and any additional characters provided.
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the character class (e.g. '*', '+', '?', '{3}', '{2,4}', etc.).</param>
    /// <param name="first">The first possible character to match.</param>
    /// <param name="second">The second possible character to match.</param>
    /// <param name="additional">Any additional character to match.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern OneOf(Quantifier quantifier, char first, char second, params char[] additional)
    {
        CharacterClass cc = new(quantifier);
        cc.OneOf(first, second, additional);
        _stringBuilder.Append(cc);
        return this;
    }

    public Pattern OneOf(string first, string second, params string[] additional)
    {
        return OneOf(Quantifier.None, first, second, additional);
    }

    public Pattern OneOf(Quantifier quantifier, string first, string second, params string[] additional)
    {
        var firstPattern = With().Literal(first);
        var secondPattern = With().Literal(second);
        _stringBuilder.AppendFormat("({0}|{1}", firstPattern, secondPattern);

        foreach (var choice in additional)
        {
            var additionalPattern = With().Literal(choice);
            _stringBuilder.AppendFormat("|{0}", additionalPattern);
        }

        _stringBuilder
            .Append(")")
            .Append(quantifier);
        return this;
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the character class specified by the given <paramref name="characterClass"/> delegate.
    /// </summary>
    /// <param name="characterClass"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern CharacterClass(Action<CharacterClass> characterClass)
    {
        return CharacterClass(Quantifier.None, characterClass);
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the character class specified by the given <paramref name="characterClass"/> delegate. 
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the character class (e.g. '*', '+', '?', '{3}', '{2,4}', etc.).</param>
    /// <param name="characterClass"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern CharacterClass(Quantifier quantifier, Action<CharacterClass> characterClass)
    {
        CharacterClass cc = new(quantifier);
        characterClass.Invoke(cc);
        _stringBuilder.Append(cc);
        return this;
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the group specified by the given <paramref name="group"/> delegate.
    /// </summary>
    /// <param name="group"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Group(Action<Pattern> group)
    {
        return Group(Quantifier.None, group);
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the group specified by the given <paramref name="group"/> delegate.
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the group.</param>
    /// <param name="group"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Group(Quantifier quantifier, Action<Pattern> group)
    {
        Pattern g = new();
        group.Invoke(g);
        _stringBuilder.Append($"({g}){quantifier}");
        return this;
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the named group specified by the given <paramref name="group"/> delegate.
    /// </summary>
    /// <param name="name">The name to assign to the capturing group.</param>
    /// <param name="group"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Group(string name, Action<Pattern> group)
    {
        return Group(Quantifier.None, name, group);
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches the named group specified by the given <paramref name="group"/> delegate.
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the named group.</param>
    /// <param name="name">The name to assign to the capturing group.</param>
    /// <param name="group"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Group(Quantifier quantifier, string name, Action<Pattern> group)
    {
        Pattern g = new();
        group.Invoke(g);
        _stringBuilder.Append($"(?<{name}>{g}){quantifier}");
        return this;
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches one of patterns specified by the given <paramref name="first"/>, <paramref name="second"/>, and <paramref name="additional"/> <see cref="Action{T}"/> delegates.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="additional"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern OneOf(Action<Pattern> first, Action<Pattern> second, params Action<Pattern>[] additional)
    {
        return OneOf(Quantifier.None, first, second, additional);
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches one of patterns specified by the given <paramref name="first"/>, <paramref name="second"/>, and <paramref name="additional"/> <see cref="Action{T}"/> delegates. 
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the group.</param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="additional"></param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern OneOf(Quantifier quantifier, Action<Pattern> first, Action<Pattern> second,
        params Action<Pattern>[] additional)
    {
        var firstPattern = With();
        first.Invoke(firstPattern);
        var secondPattern = With();
        second.Invoke(secondPattern);
        _stringBuilder.AppendFormat("({0}|{1}", firstPattern, secondPattern);

        foreach (var choice in additional)
        {
            var additionalPattern = With();
            choice.Invoke(additionalPattern);
            _stringBuilder.AppendFormat("|{0}", additionalPattern);
        }

        _stringBuilder
            .Append(")")
            .Append(quantifier);
        return this;
    }

    /// <summary>
    /// Appends a literal string to the current <see cref="Pattern"/> object.
    /// Any characters in the string that need to be escaped in a regular expression pattern will be automatically escaped.
    /// </summary>
    /// <param name="literal">The literal string to append to the pattern.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Literal(string literal) => Literal(Quantifier.None, literal);

    /// <summary>
    /// Appends a literal character to the current <see cref="Pattern"/> object.
    /// Characters that need to be escaped in a regular expression pattern will be automatically escaped.
    /// </summary>
    /// <param name="literal">The literal character to append to the pattern.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Literal(char literal) => Literal(Quantifier.None, literal);

    /// <summary>
    /// Appends a literal string to the current <see cref="Pattern"/> object.
    /// Any characters in the string that need to be escaped in a regular expression pattern will be automatically escaped.
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the literal.</param>
    /// <param name="literal">The literal string to append to the pattern.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Literal(Quantifier quantifier, string literal)
    {
        var group = literal.Length > 1 && quantifier != Quantifier.None;
        if (group)
        {
            _stringBuilder.Append("(");
        }

        foreach (var c in literal)
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

        if (group)
        {
            _stringBuilder.Append(")");
        }

        _stringBuilder.Append(quantifier);

        return this;
    }

    /// <summary>
    /// Appends a literal character to the current <see cref="Pattern"/> object.
    /// Characters that need to be escaped in a regular expression pattern will be automatically escaped.
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the literal.</param>
    /// <param name="literal">The literal string to append to the pattern.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Literal(Quantifier quantifier, char literal)
    {
        Literal(quantifier, literal.ToString());
        return this;
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches any digit character.
    /// </summary>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Digit() => Digit(Quantifier.None);

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches any digit character.
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the digit.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Digit(Quantifier quantifier)
    {
        _stringBuilder.Append($@"\d{quantifier}");
        return this;
    }

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches any word character.
    /// </summary>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Word() => Word(Quantifier.None);

    /// <summary>
    /// Returns a <see cref="Pattern"/> that matches any word character. 
    /// </summary>
    /// <param name="quantifier">The <see cref="Quantifier"/> to use for the word.</param>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern Word(Quantifier quantifier)
    {
        _stringBuilder.Append($@"\w{quantifier}");
        return this;
    }

    /// <summary>
    /// Appends the "^" anchor to the current <see cref="Pattern"/> object to match the start of a line.
    /// </summary>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern StartOfLine()
    {
        _stringBuilder.Append("^");
        return this;
    }

    /// <summary>
    /// Appends the "$" anchor to the current <see cref="Pattern"/> object to match the end of a line.
    /// </summary>
    /// <returns>A <see cref="Pattern"/> object that represents the regular expression pattern.</returns>
    public Pattern EndOfLine()
    {
        _stringBuilder.Append("$");
        return this;
    }

    /// <summary>
    /// Returns the regular expression pattern.
    /// </summary>
    /// <returns>The regular expression pattern.</returns>
    public override string ToString()
    {
        return _stringBuilder.ToString();
    }

    private const string CharsToEscape = @"()[]+.";
    private readonly StringBuilder _stringBuilder = new();
}