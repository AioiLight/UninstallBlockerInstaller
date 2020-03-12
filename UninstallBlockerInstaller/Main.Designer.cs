namespace UninstallBlockerInstaller
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Button_Install = new System.Windows.Forms.Button();
            this.Label_Description = new System.Windows.Forms.Label();
            this.Button_Open = new System.Windows.Forms.Button();
            this.TextBox_Apk = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Button_Install
            // 
            resources.ApplyResources(this.Button_Install, "Button_Install");
            this.Button_Install.Name = "Button_Install";
            this.Button_Install.UseVisualStyleBackColor = true;
            this.Button_Install.Click += new System.EventHandler(this.Button_Install_Click);
            // 
            // Label_Description
            // 
            resources.ApplyResources(this.Label_Description, "Label_Description");
            this.Label_Description.Name = "Label_Description";
            // 
            // Button_Open
            // 
            resources.ApplyResources(this.Button_Open, "Button_Open");
            this.Button_Open.Name = "Button_Open";
            this.Button_Open.UseVisualStyleBackColor = true;
            // 
            // TextBox_Apk
            // 
            resources.ApplyResources(this.TextBox_Apk, "TextBox_Apk");
            this.TextBox_Apk.Name = "TextBox_Apk";
            this.TextBox_Apk.ReadOnly = true;
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextBox_Apk);
            this.Controls.Add(this.Button_Open);
            this.Controls.Add(this.Label_Description);
            this.Controls.Add(this.Button_Install);
            this.Name = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Install;
        private System.Windows.Forms.Label Label_Description;
        private System.Windows.Forms.Button Button_Open;
        private System.Windows.Forms.TextBox TextBox_Apk;
    }
}

