using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using User.Models;
using User.Tools;

namespace User.Api
{
    public class ApiInteraction
    {
        static readonly HttpClient client = new HttpClient();
        static readonly WebScrapper scrapper = new WebScrapper();
        static readonly string urlBase = "https://localhost:7209";

        public async Task<List<Flashcard>> ViewCards()
        {
            List<Flashcard> allCards = new List<Flashcard>();

            // currently failing at response
            var response = await client.GetAsync($"{urlBase}/card");

            string responseContent = await response.Content.ReadAsStringAsync();

            allCards = JsonSerializer.Deserialize<List<Flashcard>>(responseContent) ?? throw new NullReferenceException(nameof(allCards));

            return allCards;
        }

        public async Task<string> CreateNewCard(Flashcard newCard)
        {
            //string serializedCard = JsonSerializer.Serialize(newCard);
            //StringContent content = new StringContent(serializedCard, Encoding.UTF8, "application/json");

            //var response = await client.PostAsync($"{urlBase}/card", content);

            //string responseContent = await response.Content.ReadAsStringAsync();

            //return responseContent;


            // testing scrapper only
            await scrapper.ScrapSentence("魚");
            return "test";
        }

        public async Task<string> DeleteCard(string word)
        {
            var response = await client.DeleteAsync($"{urlBase}/card/{word}");

            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}

