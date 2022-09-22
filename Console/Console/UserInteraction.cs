using System;

namespace Console.UserInteraction
{
    public class UserInteraction
    {
        public void Handle_User_input()
        {
            System.Console.WriteLine("What would you like to do today?");

            bool continuation = true;
            while (continuation)
            {
                string user_response = Display_Options();

                switch (user_response)
                {
                    case "1":
                        System.Console.WriteLine("Call view all");
                        break;
                    case "2":
                        System.Console.WriteLine("Call add new card");
                        break;
                    case "3":
                        System.Console.WriteLine("Call delete card");
                        break;
                    case "0":
                        System.Console.WriteLine("Terminating...");
                        continuation = false;
                        break;
                };
            }
        }

        public string Display_Options()
        {
            System.Console.WriteLine("[1] View all cards");
            System.Console.WriteLine("[2] Add a new card");
            System.Console.WriteLine("[3] Delete a card");
            System.Console.WriteLine("[0 End");

            string user_response = System.Console.ReadLine() ?? throw new NullReferenceException(nameof(user_response));

            return user_response;
        }
    }
}

