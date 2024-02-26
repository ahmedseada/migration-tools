using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MIgrationTools.Models
{
    public class DialogOptions<TControl> where TControl : UserControl, new()
    {
        public int Height { get; set; } = 500;
        public int Width { get; set; } = 1000;

        public string Title { get; set; }


        public TControl Control { get; set; }

        public DialogOptions()
        {
            Control = new TControl();

        }

        public DialogOptions(string title) : this()
        {
            Title = title;
        }
    }

    
}
