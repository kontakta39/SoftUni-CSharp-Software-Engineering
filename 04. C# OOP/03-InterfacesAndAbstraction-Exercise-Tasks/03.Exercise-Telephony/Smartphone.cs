using Telephony.Models.Interfaces;

namespace Telephony
{
    public class Smartphone : ICallable, IBrowsable
    {
        public string Call(string phoneNumber)
        {
            bool ifContains = phoneNumber.All(c => char.IsDigit(c));

            if (ifContains)
            {
                return $"Calling... {phoneNumber}";
            }

            else
            {
                throw new ArgumentException("Invalid number!");
            }
        }

        public string Browse(string url)
        {
            bool ifContains = url.All(c => !char.IsDigit(c));

            if (ifContains)
            {
                return $"Browsing: {url}!";
            }

            else
            {
                throw new ArgumentException("Invalid URL!");
            }
        }
    }
}
