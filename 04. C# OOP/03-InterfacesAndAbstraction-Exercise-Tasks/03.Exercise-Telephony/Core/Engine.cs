using Telephony.Core.Interfaces;
using Telephony.Models.Interfaces;

namespace Telephony.Core;

public class Engine : IEngine
{
    public void Run()
    {
        string[] phoneNumbers = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);

        string[] urls = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);

        ICallable phone;

        foreach (string phoneNumber in phoneNumbers) 
        {
            if (phoneNumber.Length == 10)
            {
                phone = new Smartphone();
            }

            else
            {
                phone = new StationaryPhone();
                
            }

            try
            {
                Console.WriteLine(phone.Call(phoneNumber));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        foreach (var item in urls)
        {
            IBrowsable url = new Smartphone();

            try
            {
                Console.WriteLine(url.Browse(item));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}