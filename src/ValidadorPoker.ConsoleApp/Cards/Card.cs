namespace ValidadorPoker.ConsoleApp.Cards;

public record Card(int CardValue, CardSuit CardSuit)
{
    public static readonly Dictionary<char, int> Values = new()
    {
        ['2'] = 2,
        ['3'] = 3,
        ['4'] = 4,
        ['5'] = 5,
        ['6'] = 6,
        ['7'] = 7,
        ['8'] = 8,
        ['9'] = 9,
        ['D'] = 10,
        ['J'] = 11,
        ['Q'] = 12,
        ['K'] = 13,
        ['A'] = 14
    };

    public static readonly Dictionary<char, CardSuit> Suits = new()
    {
        ['O'] = CardSuit.Ouros,
        ['C'] = CardSuit.Copas,
        ['E'] = CardSuit.Espadas,
        ['P'] = CardSuit.Paus
    };
}
