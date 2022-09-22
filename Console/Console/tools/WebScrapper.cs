using System;
using System.Net.Http;

namespace User.Tools
{
    public class WebScrapper
    {
        private readonly string jishoUrl = "https://jisho.org/search/";

        public async Task ScrapSentence(string word)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync($"{jishoUrl}/{word}%23sentences");

            string htmlContent = await response.Content.ReadAsStringAsync();

            // find first three instances of japanese_sentence, which indicates the beginning of a jp sentence
            int[] allSentenceStarts = new int[3];
            int lastStartPosition = 0;

            for (int i = 0; i < 3; i++)
            {
                // search for the next sentence beginning from the last found sentence
                int sentenceStart = htmlContent.IndexOf("japanese_sentence", lastStartPosition + 1);
                lastStartPosition = sentenceStart;

                allSentenceStarts[i] = sentenceStart;
            }

            // find the first three instances of english_sentence, which indicates the beginning of an en sentence (and thus the end of the coorsponding jp sentence)
            int[] allSentenceEnds = new int[3];
            int lastEndPosition = 0;
            for (int i = 0; i < 3; i++)
            {
                int sentenceEnd = htmlContent.IndexOf("english_sentence", lastEndPosition + 1);
                lastEndPosition = sentenceEnd;

                allSentenceEnds[i] = sentenceEnd;
            }

            // break into smaller snippts
            string[] allSentences = new string[3];
            for (int i = 0; i < 3; i++)
            {
                int snippetLength = allSentenceEnds[i] - allSentenceStarts[i];
                string snippet = htmlContent.Substring(allSentenceStarts[i], snippetLength);

                allSentences[i] = snippet;
            }

            System.Console.WriteLine(String.Join(",", allSentences));

            // all kanji are fixed in a <span class="unlinked">. Parse these out into an array
        }
    }
}

