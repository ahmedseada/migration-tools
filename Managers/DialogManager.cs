using Microsoft.VisualStudio.PlatformUI;
using MIgrationTools.Enums;
using MIgrationTools.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MIgrationTools.Managers
{

    public class DialogManager : DialogWindow
    {

        static IDictionary<DialogEnum, DialogWindow> Dialogs = new Dictionary<DialogEnum, DialogWindow>();

        public async Task Init<TControl>(DialogOptions<TControl> options, DialogEnum dialogEnum = DialogEnum.MainDialog)
            where TControl : UserControl, new()
        {

            this.Title = options.Title;
            this.Width = options.Width; //400
            this.Height = options.Height; //500
            this.Content = options.Control;

            Dialogs.Add(dialogEnum, this);
        }

        public static DialogWindow Get(DialogEnum dialogEnum) => Dialogs.FirstOrDefault(d => d.Key == dialogEnum).Value;
        public static void Close(DialogEnum dialogEnum)
        {
            var dialog = Dialogs.FirstOrDefault(d => d.Key == dialogEnum).Value;
            if (dialog is not null)
            {
                dialog.Close();
                Dialogs.Remove(dialogEnum);
            }
        }

        public static void Add(DialogEnum dialogEnum, DialogWindow dialog)
        {
            Dialogs.Add(dialogEnum, dialog);
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DialogManager.Close(DialogEnum.MainDialog);
        }

    }
}
