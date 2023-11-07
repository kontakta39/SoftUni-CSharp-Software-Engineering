using Telephony.Models.Interfaces;

namespace Telephony
{
    public class StationaryPhone : ICallable
    {
        public string Call(string phoneNumber)
        {
            bool ifContains = phoneNumber.All(Char.IsDigit);

            if (ifContains)
            {
                return $"Dialing... {phoneNumber}";
            }

            else
            {
                throw new ArgumentException("Invalid number!");
            }
        }
    }
}