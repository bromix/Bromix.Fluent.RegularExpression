[![Nuget](https://img.shields.io/nuget/v/Bromix.Fluent.RegularExpression)](https://www.nuget.org/packages/Bromix.Fluent.RegularExpression/) [![Nuget](https://img.shields.io/nuget/dt/Bromix.Fluent.RegularExpression)](https://www.nuget.org/packages/Bromix.Fluent.RegularExpression/) [![GitHub](https://img.shields.io/github/license/bromix/Bromix.Fluent.RegularExpression)](https://github.com/bromix/Bromix.Fluent.RegularExpression/blob/main/LICENSE)

# Fluent RegularExpression

The `Bromix.Fluent.RegularExpression` package provides a fluent API for building regular expression patterns in C#. This
can be useful in situations where regular expressions are complex or difficult to maintain, or when they
need to be generated dynamically based on external data inputs.

## Installation via dotnet CLI

1. Open a command prompt or terminal
2. Navigate to your project directory
3. Run the following command:

```
dotnet add package Bromix.Fluent.RegularExpression
```

## Usage

Here are some simple examples to get a sense of how it works. For more detailed information, you can use the [wiki](https://github.com/bromix/Bromix.Fluent.RegularExpression/wiki).

### Match Year

The pattern could be ```\d{4}``` and can be written with Fluent Regular Expression like this.

```csharp
using Bromix.Fluent.RegularExpression;

var pattern = Pattern
    .With()
    .Digit(Quantifier.Exactly(4))
    .ToString();

var regex = new Regex(pattern);
var match = regex.Match("2023");
Console.WriteLine(match);
```

### Match Mail

The pattern could be ```^[a-zA-Z0-9._%\-+]+@[a-zA-Z0-9._]+.[a-zA-Z]{2,}$``` and can be written with Fluent Regular
Expression like this.

```csharp
using Bromix.Fluent.RegularExpression;

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
```