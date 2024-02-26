using MIgrationTools.Dialogs.Windows;
using MIgrationTools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MIgrationTools.Dialogs
{
    /// <summary>
    /// Interaction logic for MigrationsTools.xaml
    /// </summary>
    public partial class MigrationsTools : UserControl
    {
        private Index _indexPage;

        public MigrationsTools()
        {
            InitializeComponent();
            _indexPage = new Index();
            Grid.SetColumn(_indexPage, 1);
            viewHolder.Children.Add(_indexPage);

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var dialog = DialogManager.Get(Enums.DialogEnum.MainDialog);
            if (dialog != null)
            {
                dialog.Close();
                DialogManager.Close(Enums.DialogEnum.MainDialog);
            }
        }

        private void BtnAddMigration_Click(object sender, RoutedEventArgs e)
        {
            viewHolder.Children.Remove(_indexPage);
            var currentTool = new AddMigration();
            Grid.SetColumn(currentTool, 1);
            viewHolder.Children.Add(currentTool);
        }
    }
}
