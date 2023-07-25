using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSharper.Helpers.Extensions
{
    public static class ListBoxExtension
    {
        public static ScrollViewer GetScrollViewer(this ListBox listBox)
        {
            Border border = (Border)VisualTreeHelper.GetChild(listBox, 0);
            return (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
        }
    }
}
