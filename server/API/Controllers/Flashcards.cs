using Microsoft.AspNetCore.Mvc;
using Flashcards.Data;
using Flashcards.Models;
using System.Text.Json;

namespace Flashcards.API
{
    [ApiController]
    [Route("[controller]")]
    public class FlashcardsController : ControllerBase
    {
        private readonly ILogger<FlashcardsController> _logger;
        private readonly IRepository _repo;

        public FlashcardsController(ILogger<FlashcardsController> logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("/card")]
        public async Task<ActionResult<List<Flashcard>>> ReadAllCards()
        {
            ContentResult response = new ContentResult();

            List<Flashcard> allCards = new List<Flashcard>();
            try
            {
                allCards = await _repo.ReadAllFlashcards();
                _logger.LogInformation("All flashcards retrieved. Count: {count}", allCards.Count);

                response = new ContentResult()
                {
                    StatusCode = 200,
                    ContentType = "application/json",
                    Content = JsonSerializer.Serialize(allCards)
                };
            }
            catch(Exception ex)
            {
                _logger.LogError("Fetch all flashcards failed: {error}", ex.Message);
                response.StatusCode = 500;
            }

            return response;
        }

        [HttpPost("/card")]
        public async Task<ActionResult> InsertNewCard([FromBody] Flashcard newCard)
        {
            ContentResult response = new ContentResult();

            try
            {
                await _repo.InsertFlashcard(newCard);
                _logger.LogInformation("Inserting new card...");
                response = new ContentResult()
                {
                    StatusCode = 200,
                    ContentType = "application/json",
                    Content = "Successfully added new card"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Insert new card failed: {error}", ex.Message);
                response.StatusCode = 500;
            }

            return response;
        }

        [HttpDelete("/card/{word}")]
        public async Task<ActionResult> DeleteCard(string word)
        {
            ContentResult response = new ContentResult();

            try
            {
                await _repo.DeleteFlashcard(word);
                _logger.LogInformation("Card {word} successfully deleted", word);
                response = new ContentResult()
                {
                    StatusCode = 200,
                    ContentType = "application/json",
                    Content = "Successfully deleted card"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Deletion failed: {error}", ex.Message);
                response.StatusCode = 500;
            }

            return response;
        }
    }
}

