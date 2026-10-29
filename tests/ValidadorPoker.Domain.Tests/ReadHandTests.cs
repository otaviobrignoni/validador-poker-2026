using ValidadorPoker.ConsoleApp;
using ValidadorPoker.ConsoleApp.Cards;

namespace ValidadorPoker.Domain.Tests;

public class ReadHandTests
{
    [Test]
    public async Task ReturnsAllEnteredCardsRegardlessOfOrder()
    {
        var hand = PokerValidator.ReadHand("AC 2P DE 7O JC");

        await Assert.That(hand.Length).IsEqualTo(5);
        await Assert.That(hand.Count(card => card.CardValue == 14 && card.CardSuit == CardSuit.Copas)).IsEqualTo(1);
        await Assert.That(hand.Count(card => card.CardValue == 2 && card.CardSuit == CardSuit.Paus)).IsEqualTo(1);
        await Assert.That(hand.Count(card => card.CardValue == 10 && card.CardSuit == CardSuit.Espadas)).IsEqualTo(1);
        await Assert.That(hand.Count(card => card.CardValue == 7 && card.CardSuit == CardSuit.Ouros)).IsEqualTo(1);
        await Assert.That(hand.Count(card => card.CardValue == 11 && card.CardSuit == CardSuit.Copas)).IsEqualTo(1);
    }
}
