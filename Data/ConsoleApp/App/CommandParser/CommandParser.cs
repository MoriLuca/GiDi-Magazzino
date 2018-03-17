using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp.App
{
    public class CommandParser : ICommandParser
    {

        private string inputString;
        public string InputString
        {
            get => inputString;
            set
            {
                inputString = value;
                this.CommandPack = ParseInput();
            }
        }
        private List<string> _args { get; set; }
        public ICommandPack CommandPack { get; set; }

        private ICommandPack ParseInput()
        {
            CommandPack cp = new CommandPack();
            _args = InputString.Split(" ").ToList();
            cp.Command = _args[0];
            cp.Args = _args.Skip(1).ToList();
            return cp;
        }
    }

    public class CommandPack : ICommandPack
    {
        public string Command { get; set; }
        public List<string> Args { get; set; }
    }

}
