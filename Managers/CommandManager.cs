using System.Diagnostics;
using System.Windows.Threading;

namespace MIgrationTools.Managers
{
    public static class CommandManager
    {

        public static async Task AddMigration(string migrationName, string migrationProject, string startupProject, string dbContext, string outputDir)
        {

            var command = $"dotnet ef migrations add {migrationName} --project {migrationProject} --startup-project {startupProject} --context {dbContext} --output-dir {outputDir}";


            await RunAsync(command);

        }

        private static async Task RunAsync(string command)
        {
            // Create process start info for cmd.exe
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true // Set to true if you don't want to display the command prompt window
            };

            var result = await VS.Commands.ExecuteAsync("View.Output");


            var outputWindow = await VS.Windows.GetOutputWindowPaneAsync(Community.VisualStudio.Toolkit.Windows.VSOutputWindowPane.General);

            await outputWindow.ActivateAsync();


            //foreach (var num in Enumerable.Range(0, 1000))
            //{
            //    await outputWindow.WriteLineAsync("Num : " + num);
            //}

            // Start the process
            Process process = Process.Start(psi);

            // Example: Send a command to the command prompt
            await process.StandardInput.WriteLineAsync(command);


            await Task.Run(async () =>
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = await process.StandardOutput.ReadLineAsync();
                    await outputWindow.WriteLineAsync(line);
                }
            });

            // Example: Read output and error streams
            // Example: Close input stream and wait for the command prompt to exit
            process.StandardInput.Close();
            process.CloseMainWindow();
        }

    }
}
