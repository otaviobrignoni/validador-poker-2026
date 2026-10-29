using ValidadorPoker.ConsoleApp;

namespace ValidadorPoker.Domain.Tests;

public class HandComparisonTests
{
    [Test]
    [Arguments("5C 5P 6E 7P KO", "2P 3E 8E 8O DO", Winner.Player2, "Um Par", "Um Par")]
    [Arguments("5O 8P 9E JE AP", "2P 5P 7O 8E QC", Winner.Player1, "Carta Alta", "Carta Alta")]
    [Arguments("2O 9P AE AC AP", "3O 6O 7O DO QO", Winner.Player2, "Trinca", "Flush")]
    [Arguments("4O 6E 9C QC QP", "3O 6O 7C QO QE", Winner.Player1, "Um Par", "Um Par")]
    [Arguments("2C 2O 4P 4O 4E", "3P 3O 3E 9E 9O", Winner.Player1, "Full House", "Full House")]
    [Arguments("3O 4C 5E 6P 7O", "2C 2E 7P 7C 7E", Winner.Player2, "Sequência", "Full House")]
    [Arguments("3O 4C 5E 6P 7O", "2C 4E 7P 7C 7E", Winner.Player1, "Sequência", "Trinca")]
    [Arguments("9O 9C 9E 9P 2O", "2C 2E 7P 7O 7C", Winner.Player1, "Quadra", "Full House")]
    [Arguments("5E 6E 7E 8E 9E", "4O 4C 4P 4E 2O", Winner.Player1, "Straight Flush", "Quadra")]
    [Arguments("DC JC QC KC AC", "5E 6E 7E 8E 9E", Winner.Player1, "Royal Flush", "Straight Flush")]
    [Arguments("4O 4C 8E 8P KO", "4E 4P 8O 8C QO", Winner.Player1, "Dois Pares", "Dois Pares")]
    [Arguments("2O 5C 8E JP AO", "2P 5E 8C JO AC", Winner.Draw, "Carta Alta", "Carta Alta")]
    [Arguments("2C 2P 4E 6O 8C", "3C 5P 7E 9O AC", Winner.Player1, "Um Par", "Carta Alta")]
    [Arguments("2C 2P 3E 3O 4C", "AC AP KE QO JC", Winner.Player1, "Dois Pares", "Um Par")]
    [Arguments("2C 2P 2E 3O 4C", "AC AP KE KO QC", Winner.Player1, "Trinca", "Dois Pares")]
    [Arguments("2C 3P 4E 5O 6C", "AO JO 9O 7O 3O", Winner.Player2, "Sequência", "Flush")]
    [Arguments("2C 2P 2E 3O 3C", "AO JO 9O 7O 4O", Winner.Player1, "Full House", "Flush")]
    public async Task ComparesAndSwapsPlayers(string hand1, string hand2, Winner winner, string class1, string class2)
    {
        var result = PokerValidator.Compare(PokerValidator.ReadHand(hand1), PokerValidator.ReadHand(hand2));
        await Assert.That(result.Winner).IsEqualTo(winner);
        await Assert.That(result.ClassP1).IsEqualTo(class1);
        await Assert.That(result.ClassP2).IsEqualTo(class2);

        var reversed = PokerValidator.Compare(PokerValidator.ReadHand(hand2), PokerValidator.ReadHand(hand1));
        var reversedWinner = winner switch
        {
            Winner.Player1 => Winner.Player2,
            Winner.Player2 => Winner.Player1,
            _ => Winner.Draw
        };
        await Assert.That(reversed.Winner).IsEqualTo(reversedWinner);
        await Assert.That(reversed.ClassP1).IsEqualTo(class2);
        await Assert.That(reversed.ClassP2).IsEqualTo(class1);
    }
}
