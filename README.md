# WordsToNumbers
A .NET library for converting numeric words into numbers.

## Installation

New versions of the library are published to NuGet.

```
Install-Package WordsToNumbers
```

## Usage

The library contains an interface, `IWordToNumberStrategy`, and the following implementations of that interface:

| Implementation Class | Description |
| --- | ------------- |
| `SimpleReplacementStrategy` | A simple replacement strategy that handles most number phrases up to (but not including) one million. |

Use it by mapping it in your service container:

```csharp
public void ConfigureServices(IServiceCollection services)
{
   services.AddScoped<IWordToNumberStrategy, SimpleReplacementStrategy>();
}
```

or just construct the strategy class yourself and use it:

```csharp
var converter = new SimpleReplacementStrategy();
converter.ConvertWordsToNumbers(words);
```

## Number Support

To see what kind of phrases this library supports, check out the [test cases](WordsToNumbers.Tests/SimpleReplacementStrategyTests.cs).  Bug reports on phrases that don't work are welcome!
