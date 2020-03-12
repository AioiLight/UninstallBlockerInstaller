using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;

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
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // ファイルの存在チェック
            if (!File.Exists(TextBox_Apk.Text))
            {
                // APKファイルが見つからない
                ErrorDialog.ShowError("", "", this);
                return;
            }

            if (!File.Exists(Path.Combine(appDir, "adb.exe")))
            {
                // adb.exeが見つからない
                ErrorDialog.ShowError("", "", this);
                return;
            }

            var adb = new AdbServer();

            var adbResult = adb.StartServer(Path.Combine(appDir, "adb.exe"), true);

            try
            {
                var device = AdbClient.Instance.GetDevices().First();

                // 端末が複数台接続されている
                if (AdbClient.Instance.GetDevices().Count > 1)
                {
                    // デバイスがどれひとつ接続されていない
                    ErrorDialog.ShowError("", "", this);
                    return;
                }

                // APKのインストール
                try
                {
                    var pkgManager = new PackageManager(device);
                    pkgManager.InstallPackage(TextBox_Apk.Text, false);
                }
                catch (Exception)
                {
                    // インストールに失敗した
                    ErrorDialog.ShowError("", "", this);
                    return;
                }

                // DeviceOwnerをセット
                var cmd = $"dpm set-device-owner {Constants.Package}/{Constants.Class}";
                var receiver = new ConsoleOutputReceiver();
                AdbClient.Instance.ExecuteRemoteCommand(cmd, device, receiver);

                if (receiver.ToString().Contains("Success:"))
                {
                    // デバイス管理者の設定に成功

                    return;
                }
                else
                {
                    // デバイス管理者の設定に失敗した
                    ErrorDialog.ShowError("", "", this);
                    return;
                }
            }
            catch (Exception)
            {
                // なんらかのエラー
                ErrorDialog.ShowError("", "", this);
                return;
            }
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