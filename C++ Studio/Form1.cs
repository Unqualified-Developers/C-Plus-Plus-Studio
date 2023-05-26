using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace C___Studio
{
    public partial class Form1 : Form
    {
        private string file;
        public Form1()
        {
            InitializeComponent();
            menuStrip1.Width = Width;
            textBox1.Width = Width - 15;
            textBox1.Height = Height - 61;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a File";
            fileDialog.Filter = "C++ Source Files (*.cpp)|*.cpp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                file = fileDialog.FileName;
                try
                {
                    using (StreamReader sr = new StreamReader(fileDialog.FileName))
                    {
                        saveToolStripMenuItem.Enabled = true;
                        textBox1.Lines = sr.ReadToEnd().Split('\n');
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kidding", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(file);
            sw.Write(textBox1.Text);
            sw.Close();
            sw.Dispose();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Save File";
            fileDialog.FileName = file;
            fileDialog.Filter = "C++ Source Files (*.cpp)|*.cpp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                file = fileDialog.FileName;
                try
                {
                    using (StreamWriter sw = new StreamWriter(fileDialog.FileName))
                    {
                        saveToolStripMenuItem.Enabled = true;
                        sw.Write(textBox1.Text);
                        sw.Close();
                        sw.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kidding", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void FormResize(object sender, EventArgs e)
        {
            menuStrip1.Width = Width;
            textBox1.Width = Width - 15;
            textBox1.Height = Height - 61;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                textBox1.Paste();
                string fzStr = textBox1.Text;
                Cursor = Cursors.Default;
            }
            catch (Exception)
            {
                Cursor = Cursors.Default;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string fzStr = textBox1.Text;
                if (fzStr.Equals("")) return;
                Clipboard.Clear();
                Clipboard.SetText(fzStr);
                Cursor = Cursors.Default;
            }
            catch (Exception)
            {
                Cursor = Cursors.Default;
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string fzStr = textBox1.Text;
                if (fzStr.Equals("")) return;
                Clipboard.Clear();
                textBox1.Cut();
                Cursor = Cursors.Default;
            }
            catch (Exception)
            {
                Cursor = Cursors.Default;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                textBox1.Undo();
                Cursor = Cursors.Default;
            }
            catch (Exception)
            {
                Cursor = Cursors.Default;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            textBox1.SelectAll();
            Cursor = Cursors.Default;
        }

        private void TextChange(object sender, EventArgs e)
        {
            string syntax = @"(\b(abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|while)\b)|(\/\/.*)";
            MatchCollection matches = Regex.Matches(textBox1.Text, syntax, RegexOptions.Multiline);
            int startIndex = textBox1.SelectionStart;
            int length = textBox1.SelectionLength;
            textBox1.SelectAll();
            textBox1.SelectionColor = Color.Black;
            textBox1.Select(startIndex, length);
            foreach (Match match in matches)
            {
                textBox1.Select(match.Index, match.Length);
                textBox1.SelectionColor = Color.Blue;
                textBox1.Select(startIndex, length);
            }
        }
    }
}
