using System.ComponentModel;
using System.Diagnostics;

namespace ValidadorPoker.ConsoleApp;

public static class Extensions
{
    public static bool IsSequence(this int[] values) => Enumerable.Range(1, values.Length - 1).All(i => values[i] == values[i - 1] + 1);
    public static string ToString(this Classification classification)
    {
        return classification switch
        {
          Classification.HighCard => "Carta Alta",
          Classification.OnePair => "Um Par",
          Classification.TwoPair => "Dois Pares",
          Classification.ThreeOfAKind => "Trinca",
          Classification.Straight => "Sequência",
          Classification.Flush => "Flush",
          Classification.FullHouse => "Full House",
          Classification.FourOfAKind => "Quadra",
          Classification.StraightFlush => "Straight Flush",
          Classification.RoyalFlush => "Royal Flush",
          _ => throw new UnreachableException()
        };
    }


}
