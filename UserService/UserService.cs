namespace MondoAspNetMvcSample.UserService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService
    {
        private static readonly List<User> Users = new List<User> { new User { Id = 1, Username = "ubersteve", Password = "pwd", FriendlyName = "Steve", AccessToken = null } };

        public User GetUser(int id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }

        public User TryLogin(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public void UpdateUser(User user)
        {
            var oldUser = Users.FirstOrDefault(u => u.Id == user.Id);
            if (oldUser == null)
            {
                throw new Exception("User does not exist!");
            }

            oldUser.Username = user.Username;
            oldUser.Password = user.Password;
            oldUser.FriendlyName = user.FriendlyName;
            oldUser.AccessToken = user.AccessToken;
        }
    }
}