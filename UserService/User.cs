﻿namespace MondoAspNetMvcSample.UserService
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FriendlyName { get; set; }

        public string Password { get; set; }

        public string AccessToken { get; set; }
    }
}