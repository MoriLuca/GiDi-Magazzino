using System.Collections.Generic;

namespace ConsoleApp.App
{
    public interface ICommandParser
    {
        string InputString { get; set; }
        ICommandPack CommandPack { get; set; }
    }

    public interface ICommandPack
    {
        string Command { get; set; }
        List<string> Args { get; set; }
    }
}
