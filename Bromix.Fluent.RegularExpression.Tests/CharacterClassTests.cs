using FluentAssertions;

namespace Bromix.Fluent.RegularExpression.Tests;

public class CharacterClassTests
{
    [Fact]
    public void CharacterClass_With_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]");
    }

    [Fact]
    public void CharacterClass_With_Range_a_zA_Z()
    {
        var result = Pattern.With()
            .CharacterClass(cc => cc
                .Range('a', 'z')
                .Range('A', 'Z'))
            .ToString();
        result.Should().Be("[a-zA-Z]");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_None_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.None, cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_ZeroOrMore_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.ZeroOrMore, cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]*");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_ZeroOrMoreLazy_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.ZeroOrMoreLazy, cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]*?");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_OneOrMore_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.OneOrMore, cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]+");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_OneOrMoreLazy_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.OneOrMoreLazy, cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]+?");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_Optional_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.Optional, cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]?");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_Exactly_4_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.Exactly(4), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]{4}");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_AtLeast_0_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.AtLeast(0), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]*");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_AtLeast_1_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.AtLeast(1), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]+");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_AtLeast_2_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.AtLeast(2), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]{2,}");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_AtMost_1_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.AtMost(1), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]?");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_AtMost_2_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.AtMost(2), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]{,2}");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_Range_0_1_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.Range(0, 1), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]?");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_Range_0_2_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.Range(0, 2), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]{,2}");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_Range_1_2_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.Range(1, 2), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]{1,2}");
    }

    [Fact]
    public void CharacterClass_With_Quantifier_Range_1_1_And_Range_a_z()
    {
        var result = Pattern.With()
            .CharacterClass(Quantifier.Range(1, 1), cc => cc
                .Range('a', 'z'))
            .ToString();
        result.Should().Be("[a-z]");
    }
    
    [Fact]
    public void CharacterClass_With_OneOf()
    {
        var result = Pattern.With()
            .CharacterClass(cc => cc.OneOf('a', '-', '1', '^', '[', ']', '.'))
            .ToString();
        result.Should().Be(@"[a\-1\^\[\].]");
    }

    [Fact]
    public void CharacterClass_With_Params_Of_OneOf()
    {
        var result = Pattern.With()
            .OneOf('a', '-', '1', '^', '[', ']', '.')
            .ToString();
        result.Should().Be(@"[a\-1\^\[\].]");
    }
    
    [Fact]
    public void CharacterClass_With_Quantifier_Params_Of_OneOf()
    {
        var result = Pattern.With()
            .OneOf(Quantifier.OneOrMore,  'a', '-', '1', '^', '[', ']', '.')
            .ToString();
        result.Should().Be(@"[a\-1\^\[\].]+");
    }
}