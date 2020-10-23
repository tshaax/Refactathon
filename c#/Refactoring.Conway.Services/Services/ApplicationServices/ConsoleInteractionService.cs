using System;

namespace Refactoring.Conway.Services.Services.ApplicationServices
{
    public class ConsoleInteractionService : IInteractionService
    {
        public void DisplayMessage(string message, bool clearDisplay = false)
        {
            if (clearDisplay)
                Console.Clear();

            Console.WriteLine(message);
        }

        public void PrintCharacter(char character)
        {
            Console.Write(character);
        }

        public int PromptIntegerQuestion(string question)
        {
            int answer;
            string inputAnswer;
            do
            {
                Console.WriteLine(question);
                inputAnswer = Console.ReadLine();
            }
            while (!int.TryParse(inputAnswer, out answer));
            return answer;
        }

        public void ReadKey()
        {
            Console.ReadKey();
        }
    }
}