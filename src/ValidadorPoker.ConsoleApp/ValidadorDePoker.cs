
using ValidadorPoker.ConsoleApp.Cards;

namespace ValidadorPoker.ConsoleApp;

public enum Winner
{
    Player1,
    Player2,
    Draw
}
public enum Classification
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush,
    RoyalFlush

}

public sealed record ComparisonResult(Winner Winner, string ClassP1, string ClassP2);

public static class PokerValidator
{
    public static ComparisonResult Compare(Card[] handP1, Card[] handP2)
    {
        var allCards = new List<Card>();
        allCards.AddRange(handP1);
        allCards.AddRange(handP2);
        if (!(allCards.Count == allCards.Distinct().Count()))
            throw new InvalidDataException();
        Winner winner = GetWinner(handP1, handP2, out Classification p1Class, out Classification p2Class);
        return new ComparisonResult(winner, Extensions.ToString(p1Class), Extensions.ToString(p2Class));
    }

    public static Winner GetWinner(Card[] handP1, Card[] handP2, out Classification p1Class, out Classification p2Class)
    {
        p1Class = GetClassification(handP1);
        p2Class = GetClassification(handP2);

        if (p1Class == p2Class)
        {
            int[] p1Values = [.. handP1.Select(h => h.CardValue)];
            int[] p2Values = [.. handP2.Select(h => h.CardValue)];
            int p1Max = p1Values.Max();
            int p2Max = p2Values.Max();
            var p1Groups = p1Values.GroupBy(x => x).ToList();
            var p2Groups = p2Values.GroupBy(x => x).ToList();
            switch (p1Class)
            {
                case Classification.RoyalFlush:
                    return Winner.Draw;
                case Classification.StraightFlush:
                    {
                        return p1Max == p2Max ? Winner.Draw : p1Max > p2Max ? Winner.Player1 : Winner.Player2;
                    }

                case Classification.FourOfAKind:
                    {
                        int p1FourValue = p1Groups.First(g => g.Count() == 4).Key;
                        int p2FourValue = p2Groups.First(g => g.Count() == 4).Key;
                        if (p1FourValue == p2FourValue)
                        {
                            int p1RemainingCard = p1Groups.First(y => y.Count() == 1).Key;
                            int p2RemainingCard = p2Groups.First(y => y.Count() == 1).Key;
                            return p1RemainingCard == p2RemainingCard ? Winner.Draw : p1RemainingCard > p2RemainingCard ? Winner.Player1 : Winner.Player2;
                        }
                        return p1FourValue > p2FourValue ? Winner.Player1 : Winner.Player2;
                    }

                case Classification.FullHouse:
                    {
                        int p1ThreeValue = p1Groups.First(g => g.Count() == 3).Key;
                        int p2ThreeValue = p2Groups.First(g => g.Count() == 3).Key;
                        if (p1ThreeValue == p2ThreeValue)
                        {
                            int p1PairValue = p1Groups.First(g => g.Count() == 2).Key;
                            int p2PairValue = p2Groups.First(g => g.Count() == 2).Key;
                            return p1PairValue == p2PairValue ? Winner.Draw : p1PairValue > p2PairValue ? Winner.Player1 : Winner.Player2;
                        }
                        return p1ThreeValue > p2ThreeValue ? Winner.Player1 : Winner.Player2;
                    }

                case Classification.Flush:
                    return CompareValues(p1Values, p2Values);
                case Classification.Straight:
                    {
                        return p1Max == p2Max ? Winner.Draw : p1Max > p2Max ? Winner.Player1 : Winner.Player2;
                    }

                case Classification.ThreeOfAKind:
                    {
                        int p1ThreeValue = p1Groups.First(g => g.Count() == 3).Key;
                        int p2ThreeValue = p2Groups.First(g => g.Count() == 3).Key;
                        if (p1ThreeValue != p2ThreeValue)
                            return p1ThreeValue > p2ThreeValue ? Winner.Player1 : Winner.Player2;

                        int[] p1RemainingCards = [.. p1Values.Where(v => v != p1ThreeValue).OrderBy(v => v)];
                        int[] p2RemainingCards = [.. p2Values.Where(v => v != p2ThreeValue).OrderBy(v => v)];
                        return CompareValues(p1RemainingCards, p2RemainingCards);
                    }

                case Classification.TwoPair:
                    {
                        int[] p1Pairs = [.. p1Groups.Where(g => g.Count() == 2).Select(g => g.Key)];
                        int[] p2Pairs = [.. p2Groups.Where(g => g.Count() == 2).Select(g => g.Key)];
                        int p1HighPair = Math.Max(p1Pairs[0], p1Pairs[1]);
                        int p2HighPair = Math.Max(p2Pairs[0], p2Pairs[1]);
                        if (p1HighPair != p2HighPair)
                            return p1HighPair > p2HighPair ? Winner.Player1 : Winner.Player2;

                        int p1LowPair = Math.Min(p1Pairs[0], p1Pairs[1]);
                        int p2LowPair = Math.Min(p2Pairs[0], p2Pairs[1]);
                        if (p1LowPair != p2LowPair)
                            return p1LowPair > p2LowPair ? Winner.Player1 : Winner.Player2;

                        int p1RemainingCard = p1Values.First(v => v != p1HighPair && v != p1LowPair);
                        int p2RemainingCard = p2Values.First(v => v != p2HighPair && v != p2LowPair);
                        return p1RemainingCard == p2RemainingCard ? Winner.Draw : p1RemainingCard > p2RemainingCard ? Winner.Player1 : Winner.Player2;
                    }

                case Classification.OnePair:
                    {
                        int p1PairValue = p1Groups.First(g => g.Count() == 2).Key;
                        int p2PairValue = p2Groups.First(g => g.Count() == 2).Key;
                        if (p1PairValue != p2PairValue)
                            return p1PairValue > p2PairValue ? Winner.Player1 : Winner.Player2;

                        int[] p1RemainingCards = [.. p1Values.Where(v => v != p1PairValue).OrderBy(v => v)];
                        int[] p2RemainingCards = [.. p2Values.Where(v => v != p2PairValue).OrderBy(v => v)];
                        return CompareValues(p1RemainingCards, p2RemainingCards);
                    }

                case Classification.HighCard:
                    return CompareValues(p1Values, p2Values);
                default:
                    return Winner.Draw;
            }
        }

        return p1Class > p2Class ? Winner.Player1 : Winner.Player2;

        static Winner CompareValues(int[] valuesP1, int[] valuesP2)
        {
            for (int i = valuesP1.Length - 1; i >= 0; i--)
            {
                if (valuesP1[i] == valuesP2[i])
                    continue;

                return valuesP1[i] > valuesP2[i] ? Winner.Player1 : Winner.Player2;
            }

            return Winner.Draw;
        }
    }
    public static Classification GetClassification(Card[] hand)
    {
        int[] values = [.. hand.Select(h => h.CardValue)];

        if (hand.All(h => h.CardSuit == hand[0].CardSuit))
        {
            if (values.Sum() == 10 + 11 + 12 + 13 + 14) // 10, J, Q, K, A
                return Classification.RoyalFlush;
            if (values.IsSequence())
                return Classification.StraightFlush;
            return Classification.Flush;
        }
        if (values.IsSequence())
            return Classification.Straight;
        if (values.GroupBy(x => x).Any(g => g.Count() == 4))
            return Classification.FourOfAKind;
        var groups = values.GroupBy(x => x).Select(y => y.Count());
        if (groups.Contains(3) && groups.Contains(2))
            return Classification.FullHouse;
        if (groups.Contains(3))
            return Classification.ThreeOfAKind;
        var pairs = values.GroupBy(x => x).Count(y => y.Count() == 2);
        if (pairs == 2)
            return Classification.TwoPair;
        if (pairs == 1)
            return Classification.OnePair;
        return Classification.HighCard;
    }

    public static Card[] ReadHand(string input)
    {
        string[] splitInput = input.Split(' ');
        if (splitInput.Length != 5 || splitInput.Any(s => s.Length != 2))
            throw new InvalidDataException();
        if (splitInput.Any(s => !Card.Values.ContainsKey(s[0]) || !Card.Suits.ContainsKey(s[1])))
            throw new InvalidDataException();
        Card[] cards = [.. splitInput.Select(s => new Card(Card.Values[s[0]], Card.Suits[s[1]])).OrderBy(c => c.CardValue)];
        if (!(cards.Length == cards.Distinct().Count()))
            throw new InvalidDataException();
        return cards;
    }
}
