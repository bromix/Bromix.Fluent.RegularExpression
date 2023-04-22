using FluentAssertions;

namespace Bromix.Fluent.RegularExpression.Tests;

public sealed class PatternTests
{
    [Theory]
    [InlineData("(", @"\(")]
    [InlineData(")", @"\)")]
    [InlineData("[", @"\[")]
    [InlineData("]", @"\]")]
    [InlineData("+", @"\+")]
    [InlineData("Hello", @"Hello")]
    public void Pattern_With_Literal(string input, string expected)
    {
        Pattern.With().Literal(input).ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("(", @"\(?")]
    [InlineData(")", @"\)?")]
    [InlineData("+", @"\+?")]
    [InlineData("Hello", @"(Hello)?")]
    public void Pattern_With_Literal_And_Quantifier_Optional(string input, string expected)
    {
        Pattern.With().Literal(Quantifier.Optional, input).ToString().Should().Be(expected);
    }

    [Fact]
    public void Pattern_With_StartOfLine()
    {
        Pattern.With().StartOfLine().ToString().Should().Be("^");
    }

    [Fact]
    public void Pattern_With_EndOfLine()
    {
        Pattern.With().EndOfLine().ToString().Should().Be("$");
    }

    [Fact]
    public void Pattern_With_Digit()
    {
        Pattern.With().Digit().ToString().Should().Be(@"\d");
    }

    [Fact]
    public void Pattern_With_Digit_And_Quantifier()
    {
        Pattern.With().Digit(Quantifier.Exactly(4)).ToString().Should().Be(@"\d{4}");
    }
    
    [Fact]
    public void Pattern_With_Word()
    {
        Pattern.With().Word().ToString().Should().Be(@"\w");
    }

    [Fact]
    public void Pattern_With_Word_And_Quantifier()
    {
        Pattern.With().Word(Quantifier.Exactly(4)).ToString().Should().Be(@"\w{4}");
    }

    [Fact]
    public void OneOf()
    {
        var group = Pattern.With()
            .OneOf(
                Quantifier.Optional,
                p => p.CharacterClass(Quantifier.AtMost(10), cc => cc.Range('0', '9')),
                p => p.CharacterClass(Quantifier.AtMost(10), cc => cc.Range('a', 'z')),
                p => p.CharacterClass(Quantifier.AtMost(10), cc => cc.Range('A', 'Z')))
            .ToString();
    }

    [Fact]
    public void NamedGroup()
    {
        var group = Pattern.With()
            .Group("Text", g => g
                .CharacterClass(cc => cc
                    .Range('A', 'Z')))
            .ToString();
    }
}