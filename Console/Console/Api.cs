using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using User.Models;

namespace User.Api
{
    public class ApiInteraction
    {
        static readonly HttpClient client = new HttpClient();
        static readonly string urlBase = "https://localhost:7209";

        public async Task<List<Flashcard>> ViewCards()
        {
            List<Flashcard> allCards = new List<Flashcard>();
            var response = await client.GetAsync($"{urlBase}/card");

            string responseContent = await response.Content.ReadAsStringAsync();

            allCards = JsonSerializer.Deserialize<List<Flashcard>>(responseContent) ?? throw new NullReferenceException(nameof(allCards));

            return allCards;
        }

        public async Task<string> CreateNewCard(Flashcard newCard)
        {
            string serializedCard = JsonSerializer.Serialize(newCard);
            StringContent content = new StringContent(serializedCard, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{urlBase}/card", content);

            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        public async Task<string> DeleteCard(string word)
        {
            var response = await client.DeleteAsync($"{urlBase}/card/{word}");

            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}

