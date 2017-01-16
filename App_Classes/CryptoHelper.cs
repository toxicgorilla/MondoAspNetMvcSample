namespace MondoAspNetMvcSample.App_Classes
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptoHelper
    {
        public static string GenerateRandomString(int length)
        {
            const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            var sb = new StringBuilder();
            using (var provider = new RNGCryptoServiceProvider())
            {
                while (sb.Length != length)
                {
                    var oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    var character = (char)oneByte[0];
                    if (Valid.Contains(character))
                    {
                        sb.Append(character);
                    }
                }
            }

            return sb.ToString();
        }
    }
}