using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.Controls
{
    public static class DataGridViewExtensions
    {
        public static void MakeDoubleBuffered(this DataGridView datagridView)
        {
            // this improves dataGridView's scrolling performance
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null,
                datagridView,
                new object[] {true}
                );
        }
    }
}
