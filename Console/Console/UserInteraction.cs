using System;
using User.Models;
using User.Api;

namespace User.Interaction
{
    public class UserInteraction
    {
        static readonly ApiInteraction api = new ApiInteraction();

        private bool continuation = true;

        public void BeginUserInteraction()
        {
            System.Console.WriteLine("What would you like to do today?");

            while (continuation)
            {
                HandleUserInput();
            }
        }

        private async Task HandleUserInput()
        {
            string user_response = DisplayOptions();

            switch (user_response)
            {
                case "1":
                    List<Flashcard> allCards = await api.ViewCards();

                    foreach (Flashcard card in allCards)
                    {
                        System.Console.WriteLine($"\n{card.Word,3} {"|",10} {card.Furigana,25} {"|",10} {card.Definition,25} {"|",10} {card.Example,25} {"|",10} {card.Difficulty,25} {"|",10} {card.Last_Reviewed,25}\n");
                    }
                    break;

                case "2":
                    System.Console.WriteLine("\nCreating new card...\n");
                    Flashcard newCard = PopulateCard();

                    string createResult = await api.CreateNewCard(newCard);

                    System.Console.WriteLine($"Create Result: {createResult}");
                    break;

                case "3":
                    System.Console.WriteLine("\n Type a word you would like to delete");
                    string wordToDelete = System.Console.ReadLine() ?? throw new NullReferenceException(nameof(wordToDelete));

                    string deleteResult = await api.DeleteCard(wordToDelete);

                    System.Console.WriteLine($"Delete result: {deleteResult}");
                    break;

                case "0":
                    System.Console.WriteLine("Terminating...");
                    continuation = false;
                    break;

                default:
                    System.Console.WriteLine("Unrecognized command");
                    break;
            }
        }

        private string DisplayOptions()
        {
            System.Console.WriteLine("[1] View all cards");
            System.Console.WriteLine("[2] Add a new card");
            System.Console.WriteLine("[3] Delete a card");
            System.Console.WriteLine("[0] End");

            string user_response = System.Console.ReadLine() ?? throw new NullReferenceException(nameof(user_response));

            return user_response;
        }

        private Flashcard PopulateCard()
        {
            System.Console.Write("Word:");
            string word = System.Console.ReadLine() ?? throw new NullReferenceException(nameof(word));

            System.Console.Write("\nFurigana:");
            string? furigana = System.Console.ReadLine();

            System.Console.Write("\nDefinition:");
            string definition = System.Console.ReadLine() ?? throw new NullReferenceException(nameof(definition));

            System.Console.Write("\nExample:");
            string? example = System.Console.ReadLine();

            string defaultLastReview = DateTime.Today.ToString("d"); 
            Flashcard card = new Flashcard(word, furigana, definition, example, 1, defaultLastReview);

            return card;
        }
    }
}

