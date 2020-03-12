using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace UninstallBlockerInstaller
{
    internal class ErrorDialog
    {
        internal static void ShowError(string instruction, string desc, Form form)
        {
            var dialog = new TaskDialog();
            dialog.Caption = form.Text;
            dialog.InstructionText = instruction;
            dialog.Text = desc;
            dialog.Icon = TaskDialogStandardIcon.Error;
            dialog.OwnerWindowHandle = form.Handle;
            dialog.Show();
        }
    }
}
