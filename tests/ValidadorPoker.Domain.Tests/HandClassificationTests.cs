using ValidadorPoker.ConsoleApp;

namespace ValidadorPoker.Domain.Tests;

public class HandClassificationTests
{
    [Test]
    [Arguments("2C 5P 8E JO AC", "Carta Alta")]
    [Arguments("2C 2P 8E JO AC", "Um Par")]
    [Arguments("2C 2P 8E 8P AC", "Dois Pares")]
    [Arguments("2C 2P 2E JO AC", "Trinca")]
    [Arguments("2C 3P 4E 5O 6C", "Sequência")]
    [Arguments("2C 5C 8C JC AC", "Flush")]
    [Arguments("2C 2P 2E JO JC", "Full House")]
    [Arguments("2C 2P 2E 2O AC", "Quadra")]
    [Arguments("2C 3C 4C 5C 6C", "Straight Flush")]
    [Arguments("DC JC QC KC AC", "Royal Flush")]
    [Arguments("AC 2P 3E 4O 5C", "Carta Alta")]
    [Arguments("AC 2C 3C 4C 5C", "Flush")]
    public async Task IdentifiesClassification(string hand, string classification)
    {
        var result = PokerValidator.Compare(PokerValidator.ReadHand(hand), PokerValidator.ReadHand("7O 9O DO QO KO"));
        await Assert.That(result.ClassP1).IsEqualTo(classification);
        await Assert.That(result.ClassP2).IsEqualTo("Flush");
    }
}
