using MIgrationTools.Dialogs;
using MIgrationTools.Managers;
using MIgrationTools.Models;
using System.Windows;

namespace MIgrationTools
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {

            var dialgManager = new DialogManager();
            var options = new DialogOptions<MigrationsTools>("Migration Tools");
            await dialgManager.Init(options);

            await dialgManager.ShowDialogAsync();


        }
    }
}
