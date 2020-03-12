using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace UninstallBlockerInstaller
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            UpdateTextBox($@"{appDir}\{Constants.ApkFileName}.{Constants.ExtensionName}", TextBox_Apk);
        }

        /// <summary>
        /// テキストボックスにテキストを設定して、キャレットを終端に持ってくる。
        /// </summary>
        /// <param name="str">テキスト。</param>
        /// <param name="textBox">テキストボックス。</param>
        private void UpdateTextBox(string str, TextBox textBox)
        {
            textBox.Text = str;
            textBox.Select(textBox.Text.Length, 0);
        }

        private void Button_Install_Click(object sender, EventArgs e)
        {

        }

        private void Button_Open_Click(object sender, EventArgs e)
        {
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // ファイルを開くダイアログを作成
            var dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = appDir;
            dialog.Filters.Add(new CommonFileDialogFilter(Constants.ExtensionName.ToUpper(), Constants.ExtensionName));

            var dialogResult = dialog.ShowDialog();

            if (dialogResult == CommonFileDialogResult.Ok)
            {
                UpdateTextBox(dialog.FileName, TextBox_Apk);
            }
        }
    }
}
