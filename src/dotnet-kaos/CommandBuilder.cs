using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace dotnet_kaos
{
    public static class ArgsExtension
    {
        public static CommandBuilder CommandBuilder(this string[] thisValue, string name = null)
        {
            var result = new CommandBuilder
            {
                Args = thisValue,
                Command = new RootCommand(name),
                Parent = null
            };
            return result;
        }
    }
    public static class CommandBuilderExtensions
    {
        public static Task<int> RunAsync(this CommandBuilder thisValue, Action<string[]> action = null)
        {
            var rootCommand = thisValue.Command as RootCommand;
            if (action is object)
                rootCommand.Handler = CommandHandler.Create(() => action(thisValue.Args));
            return rootCommand.InvokeAsync(thisValue.Args);
        }

        public static Task<int> RunAsync(this CommandBuilder thisValue, Action action = null)
        {
            var rootCommand = thisValue.Command as RootCommand;
            if (action is object)
                rootCommand.Handler = CommandHandler.Create(action);
            return rootCommand.InvokeAsync(thisValue.Args);
        }

        public static CommandBuilder Command(this CommandBuilder thisValue, string name, string description = null)
        {
            var result = new CommandBuilder
            {
                Command = new Command(name, description),
                Parent = thisValue
            };
            thisValue.Command.AddCommand(result.Command);
            return result;
        }

        public static CommandBuilder Alias(this CommandBuilder thisValue, string name)
        {
            thisValue.Command.AddAlias(name);
            return thisValue;
        }

        public static CommandBuilderOption<T> Option<T>(this CommandBuilder thisValue, string name, T defaultValue = default, string description = null, bool isHidden = false)
        {
            thisValue.Command.AddOption(new Option(name, description, new Argument<T>(defaultValue), isHidden));
            return new CommandBuilderOption<T>(thisValue);
        }

        public static CommandBuilder EndCommand(this CommandBuilder thisValue)
        {
            return thisValue.Parent;
        }

    }

    public class CommandBuilder
    {
        public CommandBuilder(CommandBuilder commandBuilder = null)
        {
            if (commandBuilder is object)
            {
                Args = commandBuilder.Args;
                Parent = commandBuilder.Parent;
                Command = commandBuilder.Command;
            }
        }
        public string[] Args { get; set; } = null;
        public CommandBuilder Parent { get; set; } = null;
        public Command Command { get; set; }
    }

    public interface IOnCommand
    {

    }

    public class CommandBuilderOption<T1> : CommandBuilder, IOnCommand
    {
        public CommandBuilderOption(CommandBuilder commandBuilder) : base(commandBuilder) { }

        public CommandBuilderOption<T1, T2> Option<T2>(string name, T2 defaultValue = default, string description = null, bool isHidden = false)
        {
            Command.AddOption(new Option(name, description, new Argument<T2>(defaultValue), isHidden));
            return new CommandBuilderOption<T1, T2>(this);
        }

        public CommandBuilder OnCommand(Action<T1> action)
        {
            Command.Handler = CommandHandler.Create(action);
            return Parent;
        }
    }

    public class CommandBuilderOption<T1, T2> : CommandBuilder, IOnCommand
    {
        public CommandBuilderOption(CommandBuilder commandBuilder) : base(commandBuilder) { }

        public CommandBuilderOption<T1, T2, T3> Option<T3>(string name, T3 defaultValue = default, string description = null, bool isHidden = false)
        {
            Command.AddOption(new Option(name, description, new Argument<T3>(defaultValue), isHidden));
            return new CommandBuilderOption<T1, T2, T3>(this);
        }
        public CommandBuilder OnCommand(Action<T1, T2> action)
        {
            Command.Handler = CommandHandler.Create(action);
            return Parent;
        }
    }

    public class CommandBuilderOption<T1, T2, T3> : CommandBuilder, IOnCommand
    {
        public CommandBuilderOption(CommandBuilder commandBuilder) : base(commandBuilder) { }

        public CommandBuilderOption<T1, T2, T3, T4> Option<T4>(string name, T4 defaultValue = default, string description = null, bool isHidden = false)
        {
            Command.AddOption(new Option(name, description, new Argument<T4>(defaultValue), isHidden));
            return new CommandBuilderOption<T1, T2, T3, T4>(this);
        }
        public CommandBuilder OnCommand(Action<T1, T2, T3> action)
        {
            Command.Handler = CommandHandler.Create(action);
            return Parent;
        }
    }

    public class CommandBuilderOption<T1, T2, T3, T4> : CommandBuilder, IOnCommand
    {
        public CommandBuilderOption(CommandBuilder commandBuilder) : base(commandBuilder) { }

        public CommandBuilderOption<T1, T2, T3, T4, T5> Option<T5>(string name, T5 defaultValue = default, string description = null, bool isHidden = false)
        {
            Command.AddOption(new Option(name, description, new Argument<T5>(defaultValue), isHidden));
            return new CommandBuilderOption<T1, T2, T3, T4, T5>(this);
        }
        public CommandBuilder OnCommand(Action<T1, T2, T3, T4> action)
        {
            Command.Handler = CommandHandler.Create(action);
            return Parent;
        }
    }

    public class CommandBuilderOption<T1, T2, T3, T4, T5> : CommandBuilder, IOnCommand
    {
        public CommandBuilderOption(CommandBuilder commandBuilder) : base(commandBuilder) { }

        public CommandBuilder OnCommand(Action<T1, T2, T3, T4, T5> action)
        {
            Command.Handler = CommandHandler.Create(action);
            return Parent;
        }
    }
}
