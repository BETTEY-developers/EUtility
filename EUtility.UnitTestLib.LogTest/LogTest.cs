using System.Diagnostics;
using System.Text;

namespace EUtility.UnitTestLib.LogTest;

public enum LogType
{
    DontCare,Track, Debug, Info, Warn, Error, Fatal
}
public class LogTest
{
    public static ConsoleColor TrackLogColor { get; set; } = ConsoleColor.DarkGray;
    public static ConsoleColor DebugLogColor { get; set; } = ConsoleColor.Gray;
    public static ConsoleColor InfoLogColor { get; set; } = ConsoleColor.Blue;
    public static ConsoleColor WarnLogColor { get; set; } = ConsoleColor.Yellow;
    public static ConsoleColor ErrorLogColor { get; set; } = ConsoleColor.Red;
    public static ConsoleColor FatalLogColor { get; set; } = ConsoleColor.Magenta;
    public static string DateFormat { get; set; } = "yyyy/MM/dd hh:mm:ss:ff";
    public static LogType FilterType { get; set; } = LogType.DontCare;

    private static List<string> _loglines = new List<string>();

    /// <summary>
    /// 写一行日志
    /// </summary>
    /// <param name="logType">该日志类型</param>
    /// <param name="message">日志行主体</param>
    static public void WriteLog(LogType logType, string message)
    {
        if (logType <= FilterType)
            return;
        _loglines.Add($"[{Enum.GetName(typeof(LogType), logType)}] {DateTime.Now.ToString(DateFormat)}: {message}");
        Console.Write("[");
        switch(logType)
        {
            case LogType.Track:
                Console.ForegroundColor = TrackLogColor; break;
            case LogType.Debug:
                Console.ForegroundColor = DebugLogColor; break;
            case LogType.Info: 
                Console.ForegroundColor = InfoLogColor; break;
            case LogType.Warn:
                Console.ForegroundColor = WarnLogColor; break;
            case LogType.Error:
                Console.ForegroundColor = ErrorLogColor; break;
            case LogType.Fatal:
                Console.ForegroundColor = FatalLogColor; break;
        }
        Console.Write($"{Enum.GetName(typeof(LogType), logType),-5}");
        Console.ResetColor();
        Console.Write("] ");
        Console.Write($"[{DateTime.Now.ToString(DateFormat)}]");
        Console.Write(": ");
        if(logType >= LogType.Error) Console.ForegroundColor = ErrorLogColor;
        Console.Write(message);
        Console.WriteLine();
    }

    /// <summary>
    /// 保存日志到文件
    /// </summary>
    /// <param name="description">日志文件描述</param>
    static public void Save(string description = "")
    {
        string s = DateTime.Now.ToString(DateFormat);
        Path.GetInvalidFileNameChars().ToList().ForEach(x => s = s.Replace(x,'_'));
        StreamWriter sw = new StreamWriter($"{Process.GetCurrentProcess().ProcessName}_{(description == "" ? "" : $"{description}_")}{s}.txt");
        sw.Write(new StringBuilder().AppendJoin(Environment.NewLine, _loglines).ToString());
        sw.Close();
    }

    /// <summary>
    /// 保存日志到流
    /// </summary>
    /// <param name="stream">要保存的流</param>
    static void Save(Stream stream)
    {
        string s = DateTime.Now.ToString(DateFormat);
        Path.GetInvalidFileNameChars().ToList().ForEach(x => s = s.Replace(x, '_'));
        var v = Encoding.Default.GetBytes(new StringBuilder().AppendJoin(Environment.NewLine, _loglines).ToString());
        stream.Write(v,0,v.Length);
    }

    /// <summary>
    /// 清空日志信息
    /// </summary>
    static void Clear()
    {
        _loglines.Clear();
    }
}
