using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;

namespace dotnet_kaos
{
    public static class CommandExtensions
    {
        public static T WithPipe<T>(this T thisValue, Action<T> action) where T : Command
        {
            action?.Invoke(thisValue);
            return thisValue;
        }

        public static T WithHandler<T>(this T thisValue, ICommandHandler commandHandler) where T : Command
        {
            thisValue.Handler = commandHandler;
            return thisValue;
        }

        public static T WithArgument<T>(this T thisValue, Argument argument) where T : System.CommandLine.Command
        {
            thisValue.AddArgument(argument);
            return thisValue;
        }
        public static T WithOption<T>(this T thisValue, Option option) where T : System.CommandLine.Command
        {
            thisValue.AddOption(option);
            return thisValue;
        }

        public static T WithOption<T>(this T thisValue, string alias, Argument argument, string description = null, bool isHidden = false) where T : System.CommandLine.Command
        {
            thisValue.AddOption(new Option(alias, description, argument, isHidden));
            return thisValue;
        }

        public static T WithAlias<T>(this T thisValue, string alias) where T : Symbol
        {
            thisValue.AddAlias(alias);
            return thisValue;
        }
    }
}
