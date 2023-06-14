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
            textBox1.Styles[ScintillaNET.Style.Cpp.Preprocessor].ForeColor = Color.Orange;
            textBox1.Styles[ScintillaNET.Style.Cpp.Number].ForeColor = Color.RoyalBlue;
            textBox1.Styles[ScintillaNET.Style.Cpp.Comment].ForeColor = Color.DimGray;
            textBox1.Styles[ScintillaNET.Style.Cpp.String].ForeColor = Color.ForestGreen;
            textBox1.Styles[ScintillaNET.Style.Cpp.CommentLine].ForeColor = Color.DimGray;
            textBox1.Styles[ScintillaNET.Style.Cpp.Default].Font = "Consolas";
            textBox1.Styles[ScintillaNET.Style.Cpp.Default].Size = 11;
            textBox1.Styles[ScintillaNET.Style.Cpp.Word].ForeColor = Color.FromArgb(0, 0, 255);
            textBox1.SetKeywords(0, "auto break case catch char class const continue default delete do double else enum explicit extern false float for friend goto if inline int long mutable namespace new operator private protected public register reinterpret_cast return short signed sizeof static static_cast struct switch template this throw true try typedef typeid typename union unsigned using virtual void volatile while");
            textBox1.Margins[0].Width = 40;
            textBox1.Margins[0].Type = ScintillaNET.MarginType.Number;
            textBox1.Styles[ScintillaNET.Style.LineNumber].Font = "Consolas";
            textBox1.Styles[ScintillaNET.Style.LineNumber].Size = 10;
            textBox1.Styles[ScintillaNET.Style.LineNumber].ForeColor = Color.Black;
            textBox1.Styles[ScintillaNET.Style.LineNumber].BackColor = Color.LightGray;
        }

        public void OpenFile()
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
            if (needToSave)
            {
                DialogResult r = MessageBox.Show("Do you want to save before quit?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (openedAFile)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        OpenFile();
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                        OpenFile();
                    }
                }
                else if (r == DialogResult.No) OpenFile();
            }
            else OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(file);
            sw.Write(textBox1.Text);
            sw.Close();
            sw.Dispose();
            needToSave = false;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            needToSave = true;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Save File";
            fileDialog.FileName = file;
            fileDialog.Filter = "C++ Source Files (*.cpp;*.cxx:*.c++;*.cc;*.cp)|*.cpp;*.cxx;*.cc;*.cp|C Source Files (*.c)|*.c|C/C++ Headers (*.h)|*.h";
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

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            textBox1.SelectAll();
            Cursor = Cursors.Default;
        }

        public void Compile()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = ".\\MinGW\\bin\\g++.exe";
            string filebc;
            try
            {
                if (file.EndsWith(".cpp") || file.EndsWith(".cxx") || file.EndsWith(".c++")) filebc = file.Substring(0, file.Length - 4);
                else if (file.EndsWith(".c"))
                {
                    filebc = file.Substring(0, file.Length - 2);
                    process.StartInfo.FileName = ".\\MinGW\\bin\\gcc.exe";
                }
                else filebc = file.Substring(0, file.Length - 3);
                process.StartInfo.Arguments = $"{file} -o {filebc}.exe"; process.Start(); 
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void optimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (needToSave)
            {
                DialogResult r = MessageBox.Show("Do you want to save before compile?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (openedAFile)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        Compile();
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                        Compile();
                    }
                }
                else if (r == DialogResult.No) Compile();
            }
            else Compile();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 1.0.0.0 Alpha 2\nCopyright ©  2023  (Python Object Developers)\nCompiler: MinGW-W64\nWelcome to contribute code!", "About C++ Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void NewEmpty()
        {
            textBox1.Text = "int main() {\n    \n    return 0;\n}\n";
            openedAFile = false;
            file = null;
            Text = "C++ Studio";
        }

        public void NewCmdProgram()
        {
            textBox1.Text = "#include <iostream>\nusing namespace std;\n\n// Main function.\nint main() {\n    cout << \"Hello World!\" << endl;\n    return 0;\n}\n";
            openedAFile = false;
            file = null;
            Text = "C++ Studio";
        }
        
        public void NewWFProgram()
        {
            textBox1.Text = "#include <windows.h>\n\nLRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);\n\nint WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)\n{\n    // Register the window class\n    const char CLASS_NAME[]  = \"My Window Class\";\n\n    WNDCLASS wc = { };\n\n    wc.lpfnWndProc   = WndProc;\n    wc.hInstance     = hInstance;\n    wc.lpszClassName = CLASS_NAME;\n\n    RegisterClass(&wc);\n\n    // Create the window\n    HWND hwnd = CreateWindowEx(\n        0,                              // Optional window styles\n        CLASS_NAME,                     // Window class\n        \"Form\",                         // Window text\n        WS_OVERLAPPEDWINDOW,            // Window style\n\n        // Size and position\n        CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,\n\n        NULL,       // Parent window\n        NULL,       // Menu\n        hInstance,  // Instance handle\n        NULL        // Additional application data\n        );\n\n    if (hwnd == NULL)\n    {\n        return 0;\n    }\n\n    // Show the window\n    ShowWindow(hwnd, nCmdShow);\n\n    // Run the message loop\n    MSG msg = { };\n    while (GetMessage(&msg, NULL, 0, 0))\n    {\n        TranslateMessage(&msg);\n        DispatchMessage(&msg);\n    }\n\n    return 0;\n}\n\nLRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)\n{\n    switch (msg)\n    {\n        case WM_DESTROY:\n            PostQuitMessage(0);\n            break;\n\n        default:\n            return DefWindowProc(hwnd, msg, wParam, lParam);\n    }\n\n    return 0;\n}";
            openedAFile = false;
            file = null;
            Text = "C++ Studio";
        }


        private void emptyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (needToSave)
            {
                DialogResult r = MessageBox.Show("Do you want to save before creating a new file?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (openedAFile)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        NewEmpty();
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                        NewEmpty();
                    }
                }
                else if (r == DialogResult.No) NewEmpty();
            }
            else NewEmpty();
        }

        private void commandProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (needToSave)
            {
                DialogResult r = MessageBox.Show("Do you want to save before creating a new file?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (openedAFile)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        NewCmdProgram();
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                        NewCmdProgram();
                    }
                }
                else if (r == DialogResult.No) NewCmdProgram();
            }
            else NewCmdProgram();
        }

        private void releasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Python-Object-Developers/C-Plus-Plus-Studio/releases");
        }

        private void windowsFormProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (needToSave)
            {
                DialogResult r = MessageBox.Show("Do you want to save before creating a new file?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (openedAFile)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        NewWFProgram();
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                        NewWFProgram();
                    }
                }
                else if (r == DialogResult.No) NewWFProgram();
            }
            else NewWFProgram();
        }

        public void Debug() 
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = ".\\MinGW\\bin\\g++.exe";
            string filebc;
            try
            {
                if (file.EndsWith(".cpp") || file.EndsWith(".cxx") || file.EndsWith(".c++")) filebc = file.Substring(0, file.Length - 4);
                else if (file.EndsWith(".c"))
                {
                    filebc = file.Substring(0, file.Length - 2);
                    process.StartInfo.FileName = ".\\MinGW\\bin\\gcc.exe";
                }
                else filebc = file.Substring(0, file.Length - 3);
                process.StartInfo.Arguments = $"-g {file} -o {filebc}.exe";
                process.Start();
                process.WaitForExit();                
                process.StartInfo.FileName = ".\\MinGW\\bin\\gdb.exe";
                process.StartInfo.Arguments = $"{filebc}.exe";
                process.Start();                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (needToSave)
            {
                DialogResult r = MessageBox.Show("Do you want to save before compile?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (openedAFile)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        Debug();
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                        Debug();
                    }
                }
                else if (r == DialogResult.No) Debug();
            }
            else Debug();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            textBox1.Undo();
            Cursor = Cursors.Default;
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            textBox1.Redo();
            Cursor = Cursors.Default;
        }
    }
}
