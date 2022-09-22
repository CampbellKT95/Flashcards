using System;
using Flashcards.Models;

namespace Flashcards.Data
{
    public interface IRepository
    {
        public Task<List<Flashcard>> ReadAllFlashcards();
        public Task InsertFlashcard(Flashcard flashcard);
        public Task DeleteFlashcard(string word);
    }
}


