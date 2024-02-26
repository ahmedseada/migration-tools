using Microsoft.VisualStudio.OLE.Interop;
using MIgrationTools.Managers;
using MIgrationTools.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MIgrationTools.Dialogs.Windows
{
    /// <summary>
    /// Interaction logic for AddMigration.xaml
    /// </summary>
    public partial class AddMigration : UserControl
    {
        IEnumerable<Project> _projects = Enumerable.Empty<Project>();
        Project _migrationProject;
        Project _startupProject;
        string _selectedDbContext=string.Empty;
        List<string> dbContexts = new List<string>();


        public AddMigration()
        {
            InitializeComponent();
            Dispatcher.InvokeAsync(async () =>
            {
                _projects = await VS.Solutions.GetAllProjectsAsync();

                foreach (var project in _projects)
                {
                    cmbProjects.Items.Add(project.Name);
                    cmbStartupProjects.Items.Add(project.Name);
                }


            });
        }

        private async void BtnAddMigration_Click(object sender, RoutedEventArgs e)
        {
            var migrationProject = _migrationProject.FullPath;
            var startupProject = _startupProject.FullPath;
            var outFolder = System.IO.Path.Combine(Directory.GetParent(migrationProject).FullName, txtFolder.Text);
            DialogManager.Close(Enums.DialogEnum.MainDialog);

            var dbContex = _selectedDbContext == string.Empty ? txtDbContext.Text : _selectedDbContext;

            await Task.Run(async () =>
            {
                await Dispatcher.InvokeAsync(async () =>
                {
                    await CommandManager.AddMigration(txtMigrationName.Text, migrationProject, startupProject, dbContex , outFolder);
                });
            });
        }

        private async void cmbProjects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string item = e.AddedItems[0] as string;
            _migrationProject = _projects.FirstOrDefault(p => p.Name == item);


            await Task.Run(async () =>
            {

                await Dispatcher.InvokeAsync(async () =>
                {
                    try
                    {
                        var dpath = Directory.GetParent(_migrationProject.FullPath);
                        var snapShots = Directory.GetFiles(dpath.FullName, "*.cs", SearchOption.AllDirectories).Where(p => p.EndsWith("ModelSnapshot.cs")).ToList();

                        foreach (var snapshot in snapShots)
                        {

                            var codeSnippet = string.Join("", File.ReadAllLines(snapshot));
                            Regex regex = new Regex(@"\[DbContext\(typeof\((\w+)\)\)\]");
                            Match match = regex.Match(codeSnippet);
                            if (match.Success)
                            {
                                dbContexts.Add(match.Groups[1].Value);
                            }
                        }

                        if (dbContexts.Any())
                        {
                            txtDbContext.Visibility = Visibility.Hidden;
                            cmbDbContexts.Visibility = Visibility.Visible;

                            dbContexts.ForEach(dbContext => { 
                            
                                cmbDbContexts.Items.Add(dbContext);
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync(ex.Message);
                        throw;
                    }
                });

            });
        }

        private void cmbStartupProjects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string item = e.AddedItems[0] as string;
            _startupProject = _projects.FirstOrDefault(p => p.Name == item);
        }

        private void cmbDbContexts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           _selectedDbContext = e.AddedItems[0] as string;
        }
    }
}


