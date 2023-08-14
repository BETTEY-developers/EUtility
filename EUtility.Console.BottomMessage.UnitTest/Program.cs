namespace EUtility.Console.Message.UnitTest;
using SConsole = System.Console;

// ***** Bottom message unit test *****
internal class Program
{
    static void Use_Default_Two_Unit_Formatter_Outputer_Create_Message()
    {
        SConsole.WriteLine()
        Message.MessageUnit messageUnit = new()
        {
            Title = "Testunit1",
            Description = "Just Test"
        };
        Message.MessageUnit messageUnit2 = new()
        {
            Title = "Testunit2",
            Description = "Just Test"
        };

        Message.MessageOutputer messageOutputer = new()
        {
            messageUnit,
            messageUnit2
        };

        messageOutputer.Write();

    }
    static void Main(string[] args)
    {
        SConsole.WriteLine("***** Bottom message unit test *****");
        
    }
}