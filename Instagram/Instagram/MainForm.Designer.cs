namespace Instagram
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.pageWorking = new System.Windows.Forms.TabPage();
            this.pageManager = new System.Windows.Forms.TabPage();
            this.btnSignUp = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tabMain.SuspendLayout();
            this.pageManager.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.pageWorking);
            this.tabMain.Controls.Add(this.pageManager);
            this.tabMain.ItemSize = new System.Drawing.Size(82, 28);
            this.tabMain.Location = new System.Drawing.Point(0, 4);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(751, 397);
            this.tabMain.TabIndex = 0;
            // 
            // pageWorking
            // 
            this.pageWorking.Location = new System.Drawing.Point(4, 32);
            this.pageWorking.Name = "pageWorking";
            this.pageWorking.Padding = new System.Windows.Forms.Padding(3);
            this.pageWorking.Size = new System.Drawing.Size(743, 361);
            this.pageWorking.TabIndex = 0;
            this.pageWorking.Text = "     Working     ";
            this.pageWorking.UseVisualStyleBackColor = true;
            // 
            // pageManager
            // 
            this.pageManager.Controls.Add(this.btnSignUp);
            this.pageManager.Location = new System.Drawing.Point(4, 22);
            this.pageManager.Name = "pageManager";
            this.pageManager.Padding = new System.Windows.Forms.Padding(3);
            this.pageManager.Size = new System.Drawing.Size(743, 371);
            this.pageManager.TabIndex = 1;
            this.pageManager.Text = "     Manager     ";
            this.pageManager.UseVisualStyleBackColor = true;
            // 
            // btnSignUp
            // 
            this.btnSignUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSignUp.Location = new System.Drawing.Point(8, 6);
            this.btnSignUp.Name = "btnSignUp";
            this.btnSignUp.Size = new System.Drawing.Size(160, 90);
            this.btnSignUp.TabIndex = 0;
            this.btnSignUp.Text = "Sign Up";
            this.btnSignUp.UseVisualStyleBackColor = true;
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 404);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(751, 22);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 426);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IG Nuker";
            this.tabMain.ResumeLayout(false);
            this.pageManager.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage pageWorking;
        private System.Windows.Forms.TabPage pageManager;
        private System.Windows.Forms.Button btnSignUp;
        private System.Windows.Forms.StatusStrip statusBar;
    }
}

