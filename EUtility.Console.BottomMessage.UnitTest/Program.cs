namespace EUtility.ConsoleEx.Message.UnitTest;

using EUtility.ConsoleEx.Message;
using EUtility.StringEx.StringExtension;
using EUtility.UnitTestLib.LogTest;
using System.Collections.Generic;
using System.Text;

// ***** Bottom message unit test *****
internal class Program
{

    static void Use_Default_Two_Unit_Formatter_Outputer_Create_Message()
    {
        LogTest.WriteLog(LogType.Track, "***** Use_Default_Two_Unit_Formatter_Outputer_Create_Message *****");

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit object ***");
        Message.MessageUnit messageUnit = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit1\" ****"); return "Testunit1"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test\" ****"); return "Just Test"; })()
        };

        LogTest.WriteLog(LogType.Debug, "messageUnit:");
        LogTest.WriteLog(LogType.Debug,UnitTestLib.ObjectProperty.OutputObjectProperty<Message.MessageUnit>.FormatObjectPropertyValue(messageUnit));

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit2 object ***");
        Message.MessageUnit messageUnit2 = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit2\" ****"); return "Testunit2"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test2\" ****"); return "Just Test2"; })()
        };

        LogTest.WriteLog(LogType.Debug, "messageUnit2:");
        LogTest.WriteLog(LogType.Debug, UnitTestLib.ObjectProperty.OutputObjectProperty<Message.MessageUnit>.FormatObjectPropertyValue(messageUnit2));

        LogTest.WriteLog(LogType.Track, "***   Init messageOutputer object ***");
        Message.MessageOutputer messageOutputer = new()
        {
            new Func<Message.MessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit object to messageOutputer object ****"); return messageUnit; })(),
            new Func<Message.MessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit2 object to messageOutputer object ****"); return messageUnit2; })()
        };

        LogTest.WriteLog(LogType.Debug, "messageOutputer:");
        LogTest.WriteLog(LogType.Debug, Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<Message.MessageOutputer>.FormatObjectPropertyValue(messageOutputer));

        LogTest.WriteLog(LogType.Info, "*** Call messageOutputer.Write ***");
        messageOutputer.Write();

    }

    class CMessageUnit : Message.IMessageUnit
    {
        string _title = string.Empty;
        string _description = string.Empty;

        public string Title 
        {
            get 
            {
                int r = new Random().Next();
                LogTest.WriteLog(LogType.Track, "** Call CMessageUnit.Title_get **");
                LogTest.WriteLog(LogType.Debug, "*** result: " + _title + r.ToString() + " ***");
                return _title + r.ToString();
            }
            set
            {
                LogTest.WriteLog(LogType.Track, "** Call CMessageUnit.Title_set **");
                LogTest.WriteLog(LogType.Debug, "*** value: " + value + " ***");
                _title = value;
            }
        }
        public string Description
        {
            get
            {
                int r = new Random().Next();
                LogTest.WriteLog(LogType.Track, "** Call CMessageUnit.Description_get **");
                LogTest.WriteLog(LogType.Debug, "*** result: " + _description + r.ToString() + " ***");
                return _description + r.ToString();
            }
            set
            {
                LogTest.WriteLog(LogType.Track, "** Call CMessageUnit.Description_set **");
                LogTest.WriteLog(LogType.Debug, "*** value: " + value + " ***");
                _description = value;
            }
        }
    }
    static void Use_Custom_Two_Unit_Default_Formatter_Outputer_Create_Message()
    {
        LogTest.WriteLog(LogType.Track, "***** Use_Custom_Two_Unit_Default_Formatter_Outputer_Create_Message *****");

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit object ***");
        CMessageUnit messageUnit = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit1\" ****"); return "Testunit1"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test\" ****"); return "Just Test"; })()
        };

        LogTest.WriteLog(LogType.Debug, "messageUnit:");
        LogTest.WriteLog(LogType.Debug, UnitTestLib.ObjectProperty.OutputObjectProperty<CMessageUnit>.FormatObjectPropertyValue(messageUnit));

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit2 object ***");
        CMessageUnit messageUnit2 = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit2\" ****"); return "Testunit2"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test2\" ****"); return "Just Test2"; })()
        };
        LogTest.WriteLog(LogType.Track, "***   Init messageOutputer object ***");
        Message.MessageOutputer messageOutputer = new()
        {
            new Func<CMessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit object to messageOutputer object ****"); return messageUnit; })(),
            new Func<CMessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit2 object to messageOutputer object ****"); return messageUnit2; })()
        };

        LogTest.WriteLog(LogType.Debug, "messageOutputer:");
        LogTest.WriteLog(LogType.Debug, Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<Message.MessageOutputer>.FormatObjectPropertyValue(messageOutputer));

        LogTest.WriteLog(LogType.Info, "*** Call messageOutputer.Write ***");
        messageOutputer.Write();
    }


    class CMessageFormatter : Message.IMessageFormatter
    {
        string IMessageFormatter.FormatMessage(ICollection<IMessageUnit> messageunits)
        {
            LogTest.WriteLog(LogType.Track, "** Call CMessageFormatter.FormatMessage **");
            List<string> sl = new();
            LogTest.WriteLog(LogType.Info, "* Enum messageunits argument collection *");
            foreach (var item in messageunits)
            {
                LogTest.WriteLog(LogType.Info, Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<IMessageUnit>.FormatObjectPropertyValue(item));

                StringBuilder sb = new();
                sb.Append(item.Title);
                sb.Append(':');
                sb.Append(item.Description);
                sl.Add(sb.ToString());
            }
            LogTest.WriteLog(LogType.Debug, "** result: " + string.Join(' ', sl) + " **");
            return string.Join(' ', sl);
        }
    }

    static void Use_Custom_Two_Unit_Custom_Formatter_Default_Outputer_Create_Message()
    {
        LogTest.WriteLog(LogType.Track, "***** Use_Custom_Two_Unit_Default_Formatter_Outputer_Create_Message *****");

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit object ***");
        CMessageUnit messageUnit = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit1\" ****"); return "Testunit1"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test\" ****"); return "Just Test"; })()
        };

        LogTest.WriteLog(LogType.Debug, "messageUnit:");
        LogTest.WriteLog(LogType.Debug, UnitTestLib.ObjectProperty.OutputObjectProperty<CMessageUnit>.FormatObjectPropertyValue(messageUnit));

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit2 object ***");
        CMessageUnit messageUnit2 = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit2\" ****"); return "Testunit2"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test2\" ****"); return "Just Test2"; })()
        };
        LogTest.WriteLog(LogType.Track, "***   Init messageOutputer object ***");
        Message.MessageOutputer messageOutputer = new()
        {
            new Func<CMessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit object to messageOutputer object ****"); return messageUnit; })(),
            new Func<CMessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit2 object to messageOutputer object ****"); return messageUnit2; })()
        };

        LogTest.WriteLog(LogType.Track, "***   Init messageformatter object ***");
        CMessageFormatter messageFormatter = new();
        LogTest.WriteLog(LogType.Debug, "messageFormatter:");
        LogTest.WriteLog(LogType.Debug,Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<CMessageFormatter>.FormatObjectPropertyValue(messageFormatter));

        LogTest.WriteLog(LogType.Debug, "messageOutputer:");
        LogTest.WriteLog(LogType.Debug, Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<Message.MessageOutputer>.FormatObjectPropertyValue(messageOutputer));

        LogTest.WriteLog(LogType.Info, "*** Call messageOutputer.Write(IMessageFormatter) ***");
        messageOutputer.Write(messageFormatter);
    }

    class CMessageOutputer : Message.MessageOutputer
    {
        public override void Write()
        {
            Write(_messageFormater);
        }
        public override void Write(IMessageFormatter messageFormater)
        {
            (int cx, int cy) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 0);
            string ms = messageFormater.FormatMessage(_messageUnits);
            Console.Write(ms + new string(' ', Console.BufferWidth - ms.GetStringInConsoleGridWidth()));
            Console.SetCursorPosition(cx, cy);
        }
    }

    static void Use_Custom_Two_Unit_Custom_Formatter_Outputer_Create_Message()
    {
        LogTest.WriteLog(LogType.Track, "***** Use_Custom_Two_Unit_Default_Formatter_Outputer_Create_Message *****");

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit object ***");
        CMessageUnit messageUnit = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit1\" ****"); return "Testunit1"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test\" ****"); return "Just Test"; })()
        };

        LogTest.WriteLog(LogType.Debug, "messageUnit:");
        LogTest.WriteLog(LogType.Debug, UnitTestLib.ObjectProperty.OutputObjectProperty<CMessageUnit>.FormatObjectPropertyValue(messageUnit));

        LogTest.WriteLog(LogType.Track, "***   Init messageUnit2 object ***");
        CMessageUnit messageUnit2 = new()
        {
            Title = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Title value: \"Testunit2\" ****"); return "Testunit2"; })(),
            Description = new Func<string>(() => { LogTest.WriteLog(LogType.Track, "**** Set Description value: \"Just Test2\" ****"); return "Just Test2"; })()
        };
        LogTest.WriteLog(LogType.Track, "***   Init messageOutputer object ***");
        CMessageOutputer messageOutputer = new()
        {
            new Func<CMessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit object to messageOutputer object ****"); return messageUnit; })(),
            new Func<CMessageUnit>(()=>{ LogTest.WriteLog(LogType.Track,"**** Add messageUnit2 object to messageOutputer object ****"); return messageUnit2; })()
        };

        LogTest.WriteLog(LogType.Track, "***   Init messageformatter object ***");
        CMessageFormatter messageFormatter = new();
        LogTest.WriteLog(LogType.Debug, "messageFormatter:");
        LogTest.WriteLog(LogType.Debug, Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<CMessageFormatter>.FormatObjectPropertyValue(messageFormatter));

        LogTest.WriteLog(LogType.Debug, "messageOutputer:");
        LogTest.WriteLog(LogType.Debug, Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<Message.MessageOutputer>.FormatObjectPropertyValue(messageOutputer));

        LogTest.WriteLog(LogType.Info, "*** Call messageOutputer.Write(IMessageFormatter) ***");
        messageOutputer.Write(messageFormatter);
    }

    static void Main(string[] args)
    {
        try
        {
            Use_Default_Two_Unit_Formatter_Outputer_Create_Message();
            Console.ReadLine();
            Use_Custom_Two_Unit_Default_Formatter_Outputer_Create_Message();
            Console.ReadLine();
            Use_Custom_Two_Unit_Custom_Formatter_Default_Outputer_Create_Message();
            Console.ReadLine();
            Use_Custom_Two_Unit_Custom_Formatter_Outputer_Create_Message();
            Console.ReadLine();
        }
        catch(Exception e)
        {
            LogTest.WriteLog(LogType.Fatal, Environment.NewLine + UnitTestLib.ObjectProperty.OutputObjectProperty<Exception>.FormatObjectPropertyValue(e));
        }
        finally
        {
            LogTest.Save();
        }
    }
}