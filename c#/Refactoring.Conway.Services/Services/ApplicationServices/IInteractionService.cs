namespace Refactoring.Conway.Services.Services.ApplicationServices
{
    public interface IInteractionService
    {
        void DisplayMessage(string message, bool clearDisplay = false);

        void PrintCharacter(char character);

        int PromptIntegerQuestion(string question);

        void ReadKey();
    }
}
