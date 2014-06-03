namespace FaceBookNuker
{
    partial class SSCVN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSCVN));
            this.btnGenName = new System.Windows.Forms.Button();
            this.lblNo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGenName
            // 
            this.btnGenName.Location = new System.Drawing.Point(78, 84);
            this.btnGenName.Name = "btnGenName";
            this.btnGenName.Size = new System.Drawing.Size(101, 38);
            this.btnGenName.TabIndex = 0;
            this.btnGenName.Text = "Gen Name";
            this.btnGenName.UseVisualStyleBackColor = true;
            this.btnGenName.Visible = false;
            this.btnGenName.Click += new System.EventHandler(this.btnGenName_Click);
            // 
            // lblNo
            // 
            this.lblNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblNo.ForeColor = System.Drawing.Color.Red;
            this.lblNo.Location = new System.Drawing.Point(12, 9);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(167, 42);
            this.lblNo.TabIndex = 1;
            this.lblNo.Text = "22222222";
            this.lblNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SSCVN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 62);
            this.Controls.Add(this.lblNo);
            this.Controls.Add(this.btnGenName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SSCVN";
            this.ShowInTaskbar = false;
            this.Text = "Count Up";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGenName;
        private System.Windows.Forms.Label lblNo;
    }
}