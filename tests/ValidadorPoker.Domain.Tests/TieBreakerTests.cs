using ValidadorPoker.ConsoleApp;

namespace ValidadorPoker.Domain.Tests;

public class TieBreakerTests
{
    [Test]
    // High card: each position in descending order.
    [Arguments("AC KP 9E 5O 3C", "KO QP 9C 5E 3O", "Carta Alta")]
    [Arguments("AC KP 9E 5O 3C", "AO QP 9C 5E 3O", "Carta Alta")]
    [Arguments("AC KP 9E 5O 3C", "AO KO 8C 5E 3O", "Carta Alta")]
    [Arguments("AC KP 9E 5O 3C", "AO KO 9C 4E 3O", "Carta Alta")]
    [Arguments("AC KP 9E 5O 3C", "AO KO 9C 5E 2O", "Carta Alta")]
    // Pair: pair value and three kickers.
    [Arguments("8C 8P 3E 5O 7C", "6C 6P AE KO QE", "Um Par")]
    [Arguments("8C 8P AE 5O 3C", "8O 8E KC 5E 3O", "Um Par")]
    [Arguments("8C 8P AE KO 3C", "8O 8E AC QE 3O", "Um Par")]
    [Arguments("8C 8P AE KO 4C", "8O 8E AC KE 3O", "Um Par")]
    // Two pairs: higher pair, lower pair and kicker.
    [Arguments("9C 9P 2E 2O 3C", "8C 8P 7E 7O AC", "Dois Pares")]
    [Arguments("9C 9P 4E 4O 3C", "9O 9E 2C 2P AC", "Dois Pares")]
    [Arguments("9C 9P 4E 4O AC", "9O 9E 4C 4P KO", "Dois Pares")]
    // Equal three-of-a-kind values would require a shared card.
    [Arguments("9C 9P 9E 2O 3C", "8C 8P 8E AO KC", "Trinca")]
    [Arguments("4C 5P 6E 7O 8C", "3C 4P 5E 6O 7C", "Sequência")]
    // Flush: differences at each position.
    [Arguments("AC KC 9C 5C 3C", "KO QO 9O 5O 3O", "Flush")]
    [Arguments("AC KC 9C 5C 3C", "AO QO 9O 5O 3O", "Flush")]
    [Arguments("AC KC 9C 5C 3C", "AO KO 8O 5O 3O", "Flush")]
    [Arguments("AC KC 9C 5C 3C", "AO KO 9O 4O 3O", "Flush")]
    [Arguments("AC KC 9C 5C 3C", "AO KO 9O 5O 2O", "Flush")]
    [Arguments("4C 4P 4E 2O 2C", "3C 3P 3E AO AC", "Full House")]
    [Arguments("9C 9P 9E 9O 2C", "8C 8P 8E 8O AC", "Quadra")]
    [Arguments("4C 5C 6C 7C 8C", "3O 4O 5O 6O 7O", "Straight Flush")]
    public async Task FirstDifferentCriterionDecides(string hand1, string hand2, string classification)
    {
        var result = PokerValidator.Compare(PokerValidator.ReadHand(hand1), PokerValidator.ReadHand(hand2));
        await Assert.That(result.Winner).IsEqualTo(Winner.Player1);
        await Assert.That(result.ClassP1).IsEqualTo(classification);
        await Assert.That(result.ClassP2).IsEqualTo(classification);
    }

    [Test]
    [Arguments("AC KP 9E 5O 3C", "AO KC 9P 5E 3O", "Carta Alta")]
    [Arguments("8C 8P AE KO 3C", "8O 8E AC KP 3O", "Um Par")]
    [Arguments("9C 9P 4E 4O AC", "9O 9E 4C 4P AO", "Dois Pares")]
    [Arguments("4C 5P 6E 7O 8C", "8O 7C 6P 5E 4O", "Sequência")]
    [Arguments("AC KC 9C 5C 3C", "AO KO 9O 5O 3O", "Flush")]
    [Arguments("4C 5C 6C 7C 8C", "4O 5O 6O 7O 8O", "Straight Flush")]
    [Arguments("DC JC QC KC AC", "DO JO QO KO AO", "Royal Flush")]
    public async Task SuitsDoNotBreakTies(string hand1, string hand2, string classification)
    {
        var first = PokerValidator.ReadHand(hand1);
        var second = PokerValidator.ReadHand(hand2);
        var result = PokerValidator.Compare(first, second);
        await Assert.That(result.Winner).IsEqualTo(Winner.Draw);
        await Assert.That(result.ClassP1).IsEqualTo(classification);
        await Assert.That(result.ClassP2).IsEqualTo(classification);
    }
}
