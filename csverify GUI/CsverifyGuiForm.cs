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

namespace Soothsharp.Gui
{
    /// <summary>
    /// This graphical interface form allows users to run csverify.exe on files without
    /// needing to know the command line optinos.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class CsverifyGuiForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsverifyGuiForm"/> class.
        /// </summary>
        public CsverifyGuiForm()
        {
            InitializeComponent();
        }

        private void bRemoveSelected_Click(object sender, EventArgs e)
        {
            if (lbCodeFiles.SelectedIndex >= 0 && lbCodeFiles.SelectedIndex < lbCodeFiles.Items.Count)
            {
                lbCodeFiles.Items.RemoveAt(lbCodeFiles.SelectedIndex);
            }

            UpdateCommandLine();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lbCodeFiles.Items.Add(openFileDialog1.FileName);
                lbCodeFiles.SetItemChecked(lbCodeFiles.Items.Count - 1, true);
            }
            UpdateCommandLine();
        }

        private void bRun_Click(object sender, EventArgs e)
        {
            string commandName = "csverify.exe";
            string arguments = tbCommandLine.Text;
            Process p = new Process
            {
                StartInfo =
                {
                    FileName = commandName,
                    Arguments = arguments,
                    WorkingDirectory = Application.StartupPath
                }
            };
            p.Start();

        }

        private void cbOnlyAnnotated_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCommandLine();
        }

        private void UpdateCommandLine()
        {
            List<string> arguments = new List<string>();
            if (rbVerbose.Checked) arguments.Add("--verbose");
            if (rbQuiet.Checked) arguments.Add("--quiet");
            if (rbCarbon.Checked) arguments.Add("--carbon");
            if (rbSilicon.Checked) arguments.Add("--silicon");
            if (rbNoVerifier.Checked) arguments.Add("-c-");
            if (rbNoVerifier.Checked) arguments.Add("-s-");
            if (cbOnlyAnnotated.Checked) arguments.Add("--only-annotated");
            if (!cbLineNumbers.Checked) arguments.Add("--line-numbers-");
            arguments.Add("--wait");
            for (int i = 0; i < lbCodeFiles.Items.Count; i++)
            {
                string item = lbCodeFiles.Items[i].ToString();
                if  (lbCodeFiles.CheckedIndices.Contains(i))
                {
                    arguments.Add(item);
                }
                else
                {
                    arguments.Add("--assume=" + item);
                }
            }

            tbCommandLine.Text = string.Join(" ", arguments.Select(str => str.Contains(" ") ? "\"" + str + "\"" : str));
        }

        private void lbCodeFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateCommandLine();
        }

        private void lbCodeFiles_Click(object sender, EventArgs e)
        {
            UpdateCommandLine();
        }

        private void lbCodeFiles_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateCommandLine();
        }

        private void lbCodeFiles_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCommandLine();
        }
    }
}
