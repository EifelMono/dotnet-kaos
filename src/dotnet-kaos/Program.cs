using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent;
using EifelMono.Fluent.Flow;

namespace dotnet_kaos
{
    class Program
    {
        static async Task<int> Main(string[] args)
            => await args.CommandBuilder("root")
                .Command("command1")
                    .Command("command1.1")
                        .Option<int>("--int-a", default, "c# name => inta")
                        .OnCommand((inta) =>
                        {
                            Console.WriteLine($"command1.1 {inta}");
                        })
                    .Command("command1.2")
                        .Option<int>("--int-a", default, "c# name => inta")
                        .OnCommand((inta) =>
                        {
                            Console.WriteLine($"command1.2 {inta}");
                        })
                    .Alias("-c1")
                    .Option<int>("--int-a", default, "c# name => inta")
                    .Option<string>("--string-b", default, "c# name => stringb")
                    .Option<double>("--double-c", default, "c# name => doublec")
                    .OnCommand((inta, stringb, doublec) =>
                    {
                        Console.WriteLine($"command1 {inta} {stringb} {doublec}");
                    })
                .Command("command2")
                    .Alias("-c2")
                    .Option<DayOfWeek>("--dow", default, "c# name => dow")
                    .OnCommand((dow) =>
                    {
                        Console.WriteLine($"command2 {dow}");
                    })
                .RunAsync(() =>
                {
                    Console.WriteLine("RootCommand");
                });



        static async Task<int> MainX(string[] args)
        {
            Console.CancelKeyPress += (s, e) =>
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            };

            var rootCommand = new RootCommand("dotnet kaos")
            {
                TestCommand(),
                ToolCommand(),
            }
            .WithHandler(CommandHandler.Create(() =>
            {
                System.Diagnostics.Process p1 = null;
                System.Diagnostics.Process p2 = null;
                Console.CancelKeyPress += (s, e) =>
                {
                    p1?.Kill();
                    p2?.Kill();
                };
                p1 = System.Diagnostics.Process.Start(@".\..\..\..\..\dotnet-kaos.Server\bin\debug\netcoreapp3.0\dotnet-kaos.Server.exe");
                p2 = fluent.OS.OpenUrl(@"http://localhost:5000");
            }));
            return await rootCommand.InvokeAsync(args);
        }

        static Command TestCommand()
            => new Command("test")
                {
                    new Command("sub1")
                    {
                        new Command("sub1.1")
                            .WithOption("--int-a", new Argument<int>(4711), "--int-a")
                            .WithHandler(CommandHandler.Create<int>((inta)=> {
                                Console.WriteLine($"sub 1.1 {inta}");
                            })),
                        new Command("sub1.2")
                            .WithHandler(CommandHandler.Create(()=> {
                                Console.WriteLine($"sub 1.2");
                            })),
                        new Command("sub1.3")
                            .WithOption("--int-c", new Argument<int>(1), "--int-c")
                            .WithOption("--dow", new Argument<DayOfWeek>(DayOfWeek.Monday), "--dow")
                            .WithHandler(CommandHandler.Create<int,DayOfWeek>((intc, dow)=> {
                                Console.WriteLine($"sub 1.3 {intc} {dow}");
                            })),

                    },
                    new Command("sub2")
                    .WithHandler(CommandHandler.Create(()=>
                    {
                        Console.WriteLine($"sub 2");
                    }))
                };

        static Command ToolCommand()
            => new Command("tool")
                {
                    new Command("update")
                        .WithAlias("karl")
                        .WithOption("--int-option", new Argument<int>(1), "Info --int-option")
                        .WithHandler(CommandHandler.Create<int>((intoption) =>
                        {
                            Console.WriteLine($"in update {intoption}");
                        })),
                    new Command("list")
                        .WithArgument(new Argument<string>("test"))
                        .WithHandler(CommandHandler.Create<string>((test) =>
                        {
                            Console.WriteLine($"in list {test}");
                        }))
                };
    }
}
