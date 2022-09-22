using System;
using Flashcards.Models;
using Npgsql;

namespace Flashcards.Data
{
    public class Repository : IRepository
    {
        private readonly string connectionString;
        private readonly ILogger<Repository> _logger;

        public Repository(string connectionString, ILogger<Repository> logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<Flashcard>> ReadAllFlashcards()
        {
            List<Flashcard> allCards = new List<Flashcard>();

            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            string cmdText = "SELECT * FROM flashcards";
            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText, connection);

            using var reader = await cmd.ExecuteReaderAsync();

            Flashcard card;
            while (reader.Read())
            {
                card = new Flashcard(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetString(5));

                allCards.Add(card);
            }

            await connection.CloseAsync();

            _logger.LogInformation("Fetched all cards. Count: {count}", allCards.Count);

            return allCards;
        }

        public async Task InsertFlashcard(Flashcard newCard)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            string cmdText = "INSERT INTO flashcards (Word, Furigana, Definition, Example, Difficulty, Last_Reviewed) VALUES (@Word, @Furigana, @Definition, @Example, @Difficulty, @Last_Reviewed)";
            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@Word", newCard.Word);
            cmd.Parameters.AddWithValue("@Furigana", newCard.Furigana);
            cmd.Parameters.AddWithValue("@Definition", newCard.Definition);
            cmd.Parameters.AddWithValue("@Example", newCard.Example);
            cmd.Parameters.AddWithValue("@Difficulty", newCard.Difficulty);
            cmd.Parameters.AddWithValue("@Last_Reviewed", newCard.Last_Reviewed);

            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            _logger.LogInformation("Inserted new card");
        }

        public async Task DeleteFlashcard(string word)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            string cmdText = "DELETE FROM flashcards WHERE Word = @Word";
            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@Word", word);

            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            _logger.LogInformation("Card deleted");
        }
    }
}

