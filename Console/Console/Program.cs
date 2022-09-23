using System;
using User.Interaction;

namespace Console
{
    public class Console
    {
        static async Task Main()
        {
            System.Console.WriteLine("What would you like to do today?");

            UserInteraction user = new UserInteraction();
            await user.BeginUserInteraction();
        }
    }
}