using FluentAssertions;

namespace Bromix.Fluent.RegularExpression.Tests;

public sealed class QuantifierTests
{
    [Fact]
    public void Quantifier_None()
    {
        Quantifier.None.ToString().Should().BeEmpty();
    }

    [Fact]
    public void Quantifier_Optional()
    {
        Quantifier.Optional.ToString().Should().Be("?");
    }

    [Fact]
    public void Quantifier_ZeroOrMore()
    {
        Quantifier.ZeroOrMore.ToString().Should().Be("*");
    }

    [Fact]
    public void Quantifier_ZeroOrMoreLazy()
    {
        Quantifier.ZeroOrMoreLazy.ToString().Should().Be("*?");
    }

    [Fact]
    public void Quantifier_OneOrMore()
    {
        Quantifier.OneOrMore.ToString().Should().Be("+");
    }

    [Fact]
    public void Quantifier_OneOrMoreLazy()
    {
        Quantifier.OneOrMoreLazy.ToString().Should().Be("+?");
    }

    [Theory]
    // ZeroOrMore
    [InlineData(0, "*")]
    // OneOrMore
    [InlineData(1, "+")]
    [InlineData(2, "{2,}")]
    [InlineData(3, "{3,}")]
    public void Quantifier_AtLeast(int count, string expected)
    {
        Quantifier.AtLeast(count).ToString().Should().Be(expected);
    }

    [Fact]
    public void Quantifier_AtLeast_Throws()
    {
        var result = () => Quantifier.AtLeast(-1);
        result.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value must be non-negative. (Parameter 'count')");
    }

    [Theory]
    // Optional
    [InlineData(1, "?")]
    [InlineData(2, "{,2}")]
    [InlineData(3, "{,3}")]
    public void Quantifier_AtMost(int count, string expected)
    {
        Quantifier.AtMost(count).ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Quantifier_AtMost_Throws(int count)
    {
        var result = () => Quantifier.AtMost(count);
        result.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value must be greater than zero. (Parameter 'count')");
    }

    [Theory]
    [InlineData(1, "")]
    [InlineData(2, "{2}")]
    [InlineData(3, "{3}")]
    public void Quantifier_Exactly(int count, string expected)
    {
        Quantifier.Exactly(count).ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Quantifier_Exactly_Throws(int count)
    {
        var result = () => Quantifier.Exactly(count);
        result.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value must be greater than zero. (Parameter 'count')");
    }

    [Theory]
    // Optional
    [InlineData(0, 1, "?")]
    // Exactly(1)
    [InlineData(1, 1, "")]
    // Exactly(1)
    [InlineData(5, 5, "{5}")]
    // AtMost(4)
    [InlineData(0, 4, "{,4}")]
    [InlineData(1, 2, "{1,2}")]
    public void Quantifier_Range(int min, int max, string expected)
    {
        Quantifier.Range(min, max).ToString().Should().Be(expected);
    }
    
    [Fact]
    public void Quantifier_Range_Negative_Min_Throws()
    {
        var result = () => Quantifier.Range(-1, 0);
        result.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value must be non-negative. (Parameter 'min')");
    }
    
    [Fact]
    public void Quantifier_Range_Negative_Max_Throws()
    {
        var result = () => Quantifier.Range(0, -1);
        result.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value must be non-negative. (Parameter 'max')");
    }
    
    [Fact]
    public void Quantifier_Range_Max_Less_Than_Min_Throws()
    {
        var result = () => Quantifier.Range(5, 4);
        result.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value max cannot be less than value min. (Parameter 'max')");
    }
    
    [Fact]
    public void Quantifier_Range_Min_And_Max_Is_0()
    {
        var result = () => Quantifier.Range(0, 0);
        result.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value must be greater than zero. (Parameter 'count')");
    }
}