using ValidadorPoker.ConsoleApp.Cards;

namespace ValidadorPoker.ConsoleApp;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Aperte ENTER para sortear duas mãos de poker...");
        if (Console.ReadLine() is null)
            return;

        List<string> deck = [.. Card.Values.Keys.SelectMany(value => Card.Suits.Keys.Select(suit => $"{value}{suit}"))];

        var drawnCards = new string[10];
        for (int i = 0; i < drawnCards.Length; i++)
        {
            int index = Random.Shared.Next(deck.Count);
            drawnCards[i] = deck[index];
            deck.RemoveAt(index);
        }

        string hand1 = string.Join(' ', drawnCards[..5]);
        string hand2 = string.Join(' ', drawnCards[5..]);
        var result = PokerValidator.Compare(PokerValidator.ReadHand(hand1), PokerValidator.ReadHand(hand2));

        Console.WriteLine($"Jogador 1: {hand1} — {result.ClassP1}");
        Console.WriteLine($"Jogador 2: {hand2} — {result.ClassP2}");
        string winner = result.Winner switch
        {
            Winner.Player1 => "Jogador 1",
            Winner.Player2 => "Jogador 2",
            _ => "Empate"
        };
        Console.WriteLine($"Resultado: {winner}");
    }
}
