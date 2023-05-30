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
        private bool needToSave = false;
        private bool openedAFile = false;
        public Form1()
        {
            InitializeComponent();
            menuStrip1.Width = Width;
            textBox1.Width = Width - 15;
            textBox1.Height = Height - 61;
        }
                
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (needToSave)
            {
                DialogResult r = MessageBox.Show("Do you want to save before quit?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (openedAFile)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        Quit();
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                        Quit();
                    }
                }
                else if (r == DialogResult.No) Quit();
            }
            else Quit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a File";
            fileDialog.Filter = "All Supported Files (*.cpp;*.cxx;*.c++;*.c;*.h;*.cc;*.cp)|*.cpp;*.cxx;*.c++;*.c;*.h;*.cc;*.cp|C++ Source Files (*.cpp;*.cxx:*.c++;*.cc;*.cp)|*.cpp;*.cxx;*.cc;*.cp|C Source Files (*.c)|*.c|C/C++ Headers (*.h)|*.h";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                file = fileDialog.FileName;
                Text = $"{file} - C++ Studio";
                try
                {
                    using (StreamReader sr = new StreamReader(fileDialog.FileName))
                    {
                        saveToolStripMenuItem.Enabled = true;
                        textBox1.Text = sr.ReadToEnd();
                        openedAFile = true;
                        needToSave = false;
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
            needToSave = false;
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
                Text = $"{file} - C++ Studio";
                try
                {
                    using (StreamWriter sw = new StreamWriter(fileDialog.FileName))
                    {
                        saveToolStripMenuItem.Enabled = true;
                        sw.Write(textBox1.Text);
                        sw.Close();
                        sw.Dispose();
                    }
                    using (StreamReader sr = new StreamReader(fileDialog.FileName))
                    {
                        saveToolStripMenuItem.Enabled = true;
                        textBox1.Text = sr.ReadToEnd();
                        needToSave = false;
                        openedAFile = true;
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
            needToSave = true;
            string o = @"\/\/.*";
            string pres = @"(\b(include|define|ifndef|endif)\b)";
            string classes = @"(\b(bool|char|byte|class|double|int|void|const|float|long|namespace|private|protected|public|readonly|static|short)\b)";
            string keys = @"(\b(abstract|as|base|break|case|catch|checked|continue|decimal|default|delegate|delete|do|else|enum|event|explicit|extern|false|finally|fixed|for|foreach|goto|if|implicit|in|interface|internal|is|lock|new|null|object|operator|out|override|params|ref|return|sbyte|sealed|sizeof|stackalloc|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|volatile|while)\b)";
            MatchCollection keymatches = Regex.Matches(textBox1.Text, keys, RegexOptions.Multiline);
            MatchCollection classmatches = Regex.Matches(textBox1.Text, classes, RegexOptions.Multiline);
            MatchCollection prematches = Regex.Matches(textBox1.Text, pres, RegexOptions.Multiline);
            MatchCollection omatches = Regex.Matches(textBox1.Text, o, RegexOptions.Multiline);
            int startIndex = textBox1.SelectionStart;
            int length = textBox1.SelectionLength;
            textBox1.SelectAll();
            textBox1.SelectionColor = Color.White;
            textBox1.Select(startIndex, length);
            foreach (Match match in keymatches)
            {
                textBox1.Select(match.Index, match.Length);
                textBox1.SelectionColor = Color.Red;
                textBox1.Select(startIndex, length);
            }
            foreach (Match match in classmatches)
            {
                textBox1.Select(match.Index, match.Length);
                textBox1.SelectionColor = Color.FromArgb(100, 100, 255);
                textBox1.Select(startIndex, length);
            }
            foreach (Match match in prematches)
            {
                textBox1.Select(match.Index - 1, match.Length + 1);
                textBox1.SelectionColor = Color.Orange;
                textBox1.Select(startIndex, length);
            }
            foreach (Match match in omatches)
            {
                textBox1.Select(match.Index, match.Length);
                textBox1.SelectionColor = Color.FromArgb(127, 127, 127);
                textBox1.Select(startIndex, length);
            }
        }
    }
}
