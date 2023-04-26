using System.Text.RegularExpressions;
using Bromix.Fluent.RegularExpression;

namespace Examples;

internal static class Patterns
{
    internal static void MatchYear()
    {
        var pattern = Pattern
            .With()
            .Digit(Quantifier.Exactly(4))
            .ToString();

        var regex = new Regex(pattern);
        var match = regex.Match("2023");
        Console.WriteLine(match);
    }

    internal static void MatchMail(string input)
    {
        var pattern = Pattern
            .With()
            .StartOfLine()
            .CharacterClass(Quantifier.OneOrMore, cc => cc
                .Range('a', 'z')
                .Range('A', 'Z')
                .Range('0', '9')
                .OneOf('.', '_', '%', '-', '+'))
            .Literal("@")
            .CharacterClass(Quantifier.OneOrMore, cc => cc
                .Range('a', 'z')
                .Range('A', 'Z')
                .Range('0', '9')
                .OneOf('.', '_'))
            .Literal(".")
            .CharacterClass(Quantifier.AtLeast(2), cc => cc
                .Range('a', 'z')
                .Range('A', 'Z'))
            .EndOfLine()
            .ToString();

        var regex = new Regex(pattern);
        var match = regex.Match(input);
        Console.WriteLine(match.Success);
    }
}