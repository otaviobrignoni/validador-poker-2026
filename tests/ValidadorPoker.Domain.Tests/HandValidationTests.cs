using ValidadorPoker.ConsoleApp;

namespace ValidadorPoker.Domain.Tests;

public class HandValidationTests
{
    [Test]
    [Arguments("")]
    [Arguments("5C 5P 6E 7P")]
    [Arguments("5C 5P 6E 7P KO AC")]
    [Arguments("5C 5C 6E 7P KO")]
    public async Task RejectsInvalidHandContents(string input)
    {
        await AssertRejected(() => PokerValidator.ReadHand(input));
    }

    [Test]
    [Arguments("1C 5P 6E 7P KO")]
    [Arguments("5C 5P 6E 7S KO")]
    [Arguments("10O 5P 6E 7P KO")]
    [Arguments("5 5P 6E 7P KO")]
    [Arguments("5CC 5P 6E 7P KO")]
    public async Task RejectsInvalidTextRepresentations(string input)
    {
        await AssertRejected(() => PokerValidator.ReadHand(input));
    }

    [Test]
    [Arguments("3O 4C 5E 6P 7O", "2C 2E 7P 7O 7C")]
    [Arguments("5E 6E 7E 8E 9E", "9O 9C 9E 9P 2O")]
    [Arguments("2C 3P 4E 5O 6C", "2C 8P 9E JO AC")]
    public async Task RejectsSharedCards(string input1, string input2)
    {
        var hand1 = PokerValidator.ReadHand(input1);
        var hand2 = PokerValidator.ReadHand(input2);
        await AssertRejected(() => PokerValidator.Compare(hand1, hand2));
        await AssertRejected(() => PokerValidator.Compare(hand2, hand1));
    }

    private static async Task AssertRejected(Action action)
    {
        var rejected = false;
        try
        {
            action();
        }
        catch (InvalidDataException)
        {
            rejected = true;
        }

        await Assert.That(rejected).IsTrue();
    }
}
