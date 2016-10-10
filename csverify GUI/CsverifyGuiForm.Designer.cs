namespace csverify_GUI
{
    partial class CsverifyGuiForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CsverifyGuiForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbVerbose = new System.Windows.Forms.RadioButton();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.rbQuiet = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbLineNumbers = new System.Windows.Forms.CheckBox();
            this.cbOnlyAnnotated = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbCarbon = new System.Windows.Forms.RadioButton();
            this.rbSilicon = new System.Windows.Forms.RadioButton();
            this.rbNoVerifier = new System.Windows.Forms.RadioButton();
            this.bRun = new System.Windows.Forms.Button();
            this.lbCodeFiles = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bRemoveSelected = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tbCommandLine = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbVerbose);
            this.groupBox1.Controls.Add(this.rbNormal);
            this.groupBox1.Controls.Add(this.rbQuiet);
            this.groupBox1.Location = new System.Drawing.Point(12, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(125, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Verbosity ";
            // 
            // rbVerbose
            // 
            this.rbVerbose.AutoSize = true;
            this.rbVerbose.Location = new System.Drawing.Point(17, 65);
            this.rbVerbose.Name = "rbVerbose";
            this.rbVerbose.Size = new System.Drawing.Size(64, 17);
            this.rbVerbose.TabIndex = 2;
            this.rbVerbose.Text = "Verbose";
            this.rbVerbose.UseVisualStyleBackColor = true;
            this.rbVerbose.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.Checked = true;
            this.rbNormal.Location = new System.Drawing.Point(17, 42);
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Size = new System.Drawing.Size(58, 17);
            this.rbNormal.TabIndex = 1;
            this.rbNormal.TabStop = true;
            this.rbNormal.Text = "Normal";
            this.rbNormal.UseVisualStyleBackColor = true;
            this.rbNormal.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // rbQuiet
            // 
            this.rbQuiet.AutoSize = true;
            this.rbQuiet.Location = new System.Drawing.Point(17, 19);
            this.rbQuiet.Name = "rbQuiet";
            this.rbQuiet.Size = new System.Drawing.Size(50, 17);
            this.rbQuiet.TabIndex = 0;
            this.rbQuiet.Text = "Quiet";
            this.rbQuiet.UseVisualStyleBackColor = true;
            this.rbQuiet.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbLineNumbers);
            this.groupBox2.Controls.Add(this.cbOnlyAnnotated);
            this.groupBox2.Location = new System.Drawing.Point(12, 188);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 77);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Other options ";
            // 
            // cbLineNumbers
            // 
            this.cbLineNumbers.AutoSize = true;
            this.cbLineNumbers.Checked = true;
            this.cbLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLineNumbers.Location = new System.Drawing.Point(7, 43);
            this.cbLineNumbers.Name = "cbLineNumbers";
            this.cbLineNumbers.Size = new System.Drawing.Size(109, 17);
            this.cbLineNumbers.TabIndex = 1;
            this.cbLineNumbers.Text = "Print line numbers";
            this.cbLineNumbers.UseVisualStyleBackColor = true;
            this.cbLineNumbers.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // cbOnlyAnnotated
            // 
            this.cbOnlyAnnotated.AutoSize = true;
            this.cbOnlyAnnotated.Location = new System.Drawing.Point(7, 20);
            this.cbOnlyAnnotated.Name = "cbOnlyAnnotated";
            this.cbOnlyAnnotated.Size = new System.Drawing.Size(152, 17);
            this.cbOnlyAnnotated.TabIndex = 0;
            this.cbOnlyAnnotated.Text = "Verify only annotated items";
            this.cbOnlyAnnotated.UseVisualStyleBackColor = true;
            this.cbOnlyAnnotated.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbCarbon);
            this.groupBox3.Controls.Add(this.rbSilicon);
            this.groupBox3.Controls.Add(this.rbNoVerifier);
            this.groupBox3.Location = new System.Drawing.Point(143, 86);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(125, 96);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Verifier Backend ";
            // 
            // rbCarbon
            // 
            this.rbCarbon.AutoSize = true;
            this.rbCarbon.Location = new System.Drawing.Point(17, 65);
            this.rbCarbon.Name = "rbCarbon";
            this.rbCarbon.Size = new System.Drawing.Size(59, 17);
            this.rbCarbon.TabIndex = 2;
            this.rbCarbon.Text = "Carbon";
            this.rbCarbon.UseVisualStyleBackColor = true;
            this.rbCarbon.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // rbSilicon
            // 
            this.rbSilicon.AutoSize = true;
            this.rbSilicon.Checked = true;
            this.rbSilicon.Location = new System.Drawing.Point(17, 42);
            this.rbSilicon.Name = "rbSilicon";
            this.rbSilicon.Size = new System.Drawing.Size(56, 17);
            this.rbSilicon.TabIndex = 1;
            this.rbSilicon.TabStop = true;
            this.rbSilicon.Text = "Silicon";
            this.rbSilicon.UseVisualStyleBackColor = true;
            this.rbSilicon.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // rbNoVerifier
            // 
            this.rbNoVerifier.AutoSize = true;
            this.rbNoVerifier.Location = new System.Drawing.Point(17, 19);
            this.rbNoVerifier.Name = "rbNoVerifier";
            this.rbNoVerifier.Size = new System.Drawing.Size(51, 17);
            this.rbNoVerifier.TabIndex = 0;
            this.rbNoVerifier.Text = "None";
            this.rbNoVerifier.UseVisualStyleBackColor = true;
            this.rbNoVerifier.CheckedChanged += new System.EventHandler(this.cbOnlyAnnotated_CheckedChanged);
            // 
            // bRun
            // 
            this.bRun.Location = new System.Drawing.Point(12, 312);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(724, 75);
            this.bRun.TabIndex = 4;
            this.bRun.Text = "Run verification";
            this.bRun.UseVisualStyleBackColor = true;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // lbCodeFiles
            // 
            this.lbCodeFiles.FormattingEnabled = true;
            this.lbCodeFiles.Location = new System.Drawing.Point(275, 56);
            this.lbCodeFiles.Name = "lbCodeFiles";
            this.lbCodeFiles.Size = new System.Drawing.Size(461, 169);
            this.lbCodeFiles.TabIndex = 5;
            this.lbCodeFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbCodeFiles_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Code Files:";
            // 
            // bRemoveSelected
            // 
            this.bRemoveSelected.Location = new System.Drawing.Point(604, 227);
            this.bRemoveSelected.Name = "bRemoveSelected";
            this.bRemoveSelected.Size = new System.Drawing.Size(132, 23);
            this.bRemoveSelected.TabIndex = 7;
            this.bRemoveSelected.Text = "Remove Selected File";
            this.bRemoveSelected.UseVisualStyleBackColor = true;
            this.bRemoveSelected.Click += new System.EventHandler(this.bRemoveSelected_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 26);
            this.label2.TabIndex = 8;
            this.label2.Text = "A checked item means \"full verification.\"\r\nAn unchecked item means \"ignore method" +
    " bodies.\"";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(661, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Add...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "C# code files|*.cs|All files|*.*";
            this.openFileDialog1.ShowHelp = true;
            this.openFileDialog1.Title = "Add C# code file";
            // 
            // tbCommandLine
            // 
            this.tbCommandLine.Location = new System.Drawing.Point(12, 286);
            this.tbCommandLine.Name = "tbCommandLine";
            this.tbCommandLine.Size = new System.Drawing.Size(724, 20);
            this.tbCommandLine.TabIndex = 10;
            // 
            // CsverifyGuiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 399);
            this.Controls.Add(this.tbCommandLine);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bRemoveSelected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbCodeFiles);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CsverifyGuiForm";
            this.Text = "Soothsharp Verifier GUI";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVerbose;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.RadioButton rbQuiet;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbOnlyAnnotated;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbCarbon;
        private System.Windows.Forms.RadioButton rbSilicon;
        private System.Windows.Forms.RadioButton rbNoVerifier;
        private System.Windows.Forms.CheckBox cbLineNumbers;
        private System.Windows.Forms.Button bRun;
        private System.Windows.Forms.CheckedListBox lbCodeFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bRemoveSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbCommandLine;
    }
}

