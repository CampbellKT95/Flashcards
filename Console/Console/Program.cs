﻿using System;
using User.Interaction;

namespace Console
{
    public class Console
    {
        static void Main()
        {
            UserInteraction user = new UserInteraction();

            user.Handle_User_input();
        }
    }
}