using Spectre.Console;
using System.Diagnostics;
using TarkovDumper.Implementations;

namespace TarkovDumper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            AnsiConsole.Profile.Width = 420;

            // Ask the user which processor to run
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which processor would you like to run?")
                    .AddChoices(new[] { "Lone Tarkov", "Lone Arena", "Evo" }));

            AnsiConsole.Status().Start("Starting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));

                Processor processor = null;
                try
                {
                    // Instantiate the selected processor
                    processor = choice switch
                    {
                        "Lone Tarkov" => new EFTProcessor(),
                        //"Lone Arena" => new ArenaProcessor(),
                        //"Evo" => new EvoProcessor(),
                        _ => throw new InvalidOperationException("Invalid processor selection.")
                    };

                    processor.Run(ctx);
                    GC.Collect();
                    Pause();
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine();

                    if (processor != null)
                        AnsiConsole.MarkupLine($"[bold yellow]Exception thrown while processing step -> {processor.LastStepName}[/]");

                    AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                    if (ex.StackTrace != null)
                    {
                        AnsiConsole.MarkupLine("[bold yellow]==========================Begin Stack Trace==========================[/]");
                        AnsiConsole.WriteLine(ex.StackTrace);
                        AnsiConsole.MarkupLine("[bold yellow]===========================End Stack Trace===========================[/]");
                    }
                }
                finally
                {
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                }
            });

            Pause();
        }

        private static void Pause()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}