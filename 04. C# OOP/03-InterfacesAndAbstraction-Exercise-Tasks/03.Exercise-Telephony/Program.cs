//3 Exercise - Telephony
using Telephony.Core;
using Telephony.Core.Interfaces;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}