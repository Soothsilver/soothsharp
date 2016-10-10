using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csverify_GUI
{
    public partial class CsverifyGuiForm : Form
    {
        public CsverifyGuiForm()
        {
            InitializeComponent();
        }

        private void bRemoveSelected_Click(object sender, EventArgs e)
        {
            if (this.lbCodeFiles.SelectedIndex >= 0 && this.lbCodeFiles.SelectedIndex < this.lbCodeFiles.Items.Count)
            {
                this.lbCodeFiles.Items.RemoveAt(this.lbCodeFiles.SelectedIndex);
            }

            UpdateCommandLine();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.lbCodeFiles.Items.Add(this.openFileDialog1.FileName);
                this.lbCodeFiles.SetItemChecked(this.lbCodeFiles.Items.Count - 1, true);
            }
            UpdateCommandLine();
        }

        private void bRun_Click(object sender, EventArgs e)
        {
            string commandName = "csverify.exe";
            string arguments = this.tbCommandLine.Text;
            Process p = new Process();
            p.StartInfo.FileName = commandName;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.WorkingDirectory = Application.StartupPath;
            p.Start();

        }

        private void cbOnlyAnnotated_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCommandLine();
        }

        private void UpdateCommandLine()
        {
            List<string> arguments = new List<string>();
            if (this.rbVerbose.Checked) arguments.Add("--verbose");
            if (this.rbQuiet.Checked) arguments.Add("--quiet");
            if (this.rbCarbon.Checked) arguments.Add("--carbon");
            if (this.rbSilicon.Checked) arguments.Add("--silicon");
            if (this.rbNoVerifier.Checked) arguments.Add("-c-");
            if (this.rbNoVerifier.Checked) arguments.Add("-s-");
            if (this.cbOnlyAnnotated.Checked) arguments.Add("--only-annotated");
            if (!this.cbLineNumbers.Checked) arguments.Add("--line-numbers-");
            arguments.Add("--wait");
            for (int i = 0; i < this.lbCodeFiles.Items.Count; i++)
            {
                string item = this.lbCodeFiles.Items[i].ToString();
                if  (this.lbCodeFiles.CheckedIndices.Contains(i))
                {
                    arguments.Add(item);
                }
                else
                {
                    arguments.Add("--assume=" + item);
                }
            }

            this.tbCommandLine.Text = string.Join(" ", arguments.Select(str => str.Contains(" ") ? "\"" + str + "\"" : str));
        }

        private void lbCodeFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateCommandLine();
        }
    }
}
