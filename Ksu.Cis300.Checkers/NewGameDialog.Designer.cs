
namespace Ksu.Cis300.Checkers
{
    partial class uxNewGameDialog
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
            this.uxComputerBlack = new System.Windows.Forms.RadioButton();
            this.uxComputerWhite = new System.Windows.Forms.RadioButton();
            this.uxNoComputer = new System.Windows.Forms.RadioButton();
            this.uxLevelLabel = new System.Windows.Forms.Label();
            this.uxLevel = new System.Windows.Forms.NumericUpDown();
            this.uxOKButton = new System.Windows.Forms.Button();
            this.uxCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.uxLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // uxComputerBlack
            // 
            this.uxComputerBlack.AutoSize = true;
            this.uxComputerBlack.Location = new System.Drawing.Point(77, 19);
            this.uxComputerBlack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uxComputerBlack.Name = "uxComputerBlack";
            this.uxComputerBlack.Size = new System.Drawing.Size(187, 24);
            this.uxComputerBlack.TabIndex = 0;
            this.uxComputerBlack.TabStop = true;
            this.uxComputerBlack.Text = "Computer plays Black";
            this.uxComputerBlack.UseVisualStyleBackColor = true;
            // 
            // uxComputerWhite
            // 
            this.uxComputerWhite.AutoSize = true;
            this.uxComputerWhite.Location = new System.Drawing.Point(77, 55);
            this.uxComputerWhite.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uxComputerWhite.Name = "uxComputerWhite";
            this.uxComputerWhite.Size = new System.Drawing.Size(189, 24);
            this.uxComputerWhite.TabIndex = 1;
            this.uxComputerWhite.TabStop = true;
            this.uxComputerWhite.Text = "Computer plays White";
            this.uxComputerWhite.UseVisualStyleBackColor = true;
            // 
            // uxNoComputer
            // 
            this.uxNoComputer.AutoSize = true;
            this.uxNoComputer.Location = new System.Drawing.Point(77, 90);
            this.uxNoComputer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uxNoComputer.Name = "uxNoComputer";
            this.uxNoComputer.Size = new System.Drawing.Size(171, 24);
            this.uxNoComputer.TabIndex = 2;
            this.uxNoComputer.TabStop = true;
            this.uxNoComputer.Text = "No computer player";
            this.uxNoComputer.UseVisualStyleBackColor = true;
            // 
            // uxLevelLabel
            // 
            this.uxLevelLabel.AutoSize = true;
            this.uxLevelLabel.Location = new System.Drawing.Point(73, 135);
            this.uxLevelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.uxLevelLabel.Name = "uxLevelLabel";
            this.uxLevelLabel.Size = new System.Drawing.Size(50, 20);
            this.uxLevelLabel.TabIndex = 3;
            this.uxLevelLabel.Text = "Level:";
            // 
            // uxLevel
            // 
            this.uxLevel.Location = new System.Drawing.Point(135, 132);
            this.uxLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uxLevel.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.uxLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.uxLevel.Name = "uxLevel";
            this.uxLevel.Size = new System.Drawing.Size(117, 26);
            this.uxLevel.TabIndex = 4;
            this.uxLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.uxLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // uxOKButton
            // 
            this.uxOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uxOKButton.Location = new System.Drawing.Point(77, 181);
            this.uxOKButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uxOKButton.Name = "uxOKButton";
            this.uxOKButton.Size = new System.Drawing.Size(81, 35);
            this.uxOKButton.TabIndex = 5;
            this.uxOKButton.Text = "OK";
            this.uxOKButton.UseVisualStyleBackColor = true;
            // 
            // uxCancel
            // 
            this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uxCancel.Location = new System.Drawing.Point(191, 181);
            this.uxCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uxCancel.Name = "uxCancel";
            this.uxCancel.Size = new System.Drawing.Size(78, 35);
            this.uxCancel.TabIndex = 6;
            this.uxCancel.Text = "Cancel";
            this.uxCancel.UseVisualStyleBackColor = true;
            // 
            // uxNewGameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 249);
            this.Controls.Add(this.uxCancel);
            this.Controls.Add(this.uxOKButton);
            this.Controls.Add(this.uxLevel);
            this.Controls.Add(this.uxLevelLabel);
            this.Controls.Add(this.uxNoComputer);
            this.Controls.Add(this.uxComputerWhite);
            this.Controls.Add(this.uxComputerBlack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "uxNewGameDialog";
            this.Text = "New Game";
            ((System.ComponentModel.ISupportInitialize)(this.uxLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton uxComputerBlack;
        private System.Windows.Forms.RadioButton uxComputerWhite;
        private System.Windows.Forms.RadioButton uxNoComputer;
        private System.Windows.Forms.Label uxLevelLabel;
        private System.Windows.Forms.NumericUpDown uxLevel;
        private System.Windows.Forms.Button uxOKButton;
        private System.Windows.Forms.Button uxCancel;
    }
}