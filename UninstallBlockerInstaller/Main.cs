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
            var prop = Properties.Resources.ResourceManager;

            // ファイルの存在チェック
            if (!File.Exists(TextBox_Apk.Text))
            {
                // APKファイルが見つからない
                ErrorDialog.ShowError(
                    string.Format(
                        prop.GetString("ApkNotFound_Inst"),
                        Path.GetFileName(TextBox_Apk.Text)),
                    string.Format(
                        prop.GetString("ApkNotFound_Desc"),
                        Path.GetFileName(TextBox_Apk.Text)),
                    this);
                return;
            }

            if (!File.Exists(Path.Combine(appDir, "adb.exe")))
            {
                // adb.exeが見つからない
                ErrorDialog.ShowError(
                    prop.GetString("AdbNotFound_Inst"),
                    prop.GetString("AdbNotFound_Desc"),
                    this);
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
                    ErrorDialog.ShowError(
                    prop.GetString("TooManyDevice_Inst"),
                    prop.GetString("TooManyDevice_Desc"),
                    this);
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
                    ErrorDialog.ShowError(
                    prop.GetString("ApkInstallFailed_Inst"),
                    prop.GetString("ApkInstallFailed_Desc"),
                    this);
                    return;
                }

                // DeviceOwnerをセット
                var cmd = $"dpm set-device-owner {Constants.Package}/{Constants.Class}";
                var receiver = new ConsoleOutputReceiver();
                AdbClient.Instance.ExecuteRemoteCommand(cmd, device, receiver);

                if (receiver.ToString().Contains("Success:"))
                {
                    // デバイス管理者の設定に成功
                    var dialog = new TaskDialog();
                    dialog.Caption = Properties.Resources.Complete_Desc;
                    dialog.Text = Text;
                    dialog.OwnerWindowHandle = Handle;
                    dialog.Icon = TaskDialogStandardIcon.Information;

                    dialog.Show();
                    return;
                }
                else
                {
                    // デバイス管理者の設定に失敗した
                    ErrorDialog.ShowError(
                    prop.GetString("DeviceOwnerFailed_Inst"),
                    prop.GetString("DeviceOwnerFailed_Desc"),
                    this);
                    return;
                }
            }
            catch (Exception)
            {
                // なんらかのエラー
                ErrorDialog.ShowError(
                    prop.GetString("Error_Inst"),
                    prop.GetString("Error_Desc"),
                    this);
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

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // adb.exeをキルする
            try
            {
                AdbClient.Instance.KillAdb();
            }
            catch (Exception)
            {

            }
        }
    }
}