using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ScintillaNET;

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
            textBox1.Styles[Style.Cpp.Preprocessor].ForeColor = Color.DarkOrange;
            textBox1.Styles[Style.Cpp.Number].ForeColor = Color.DeepSkyBlue;
            textBox1.Styles[Style.Cpp.Comment].ForeColor = Color.DimGray;
            textBox1.Styles[Style.Cpp.String].ForeColor = Color.ForestGreen;
            textBox1.Styles[Style.Cpp.CommentLine].ForeColor = Color.DimGray;
            textBox1.Styles[Style.Default].Font = "Consolas";
            textBox1.Styles[Style.Default].Size = 16;
            textBox1.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            textBox1.Styles[Style.Cpp.Word2].ForeColor = Color.Brown;
            textBox1.SetKeywords(0, "auto break case catch char class const continue default delete do double else enum explicit extern false float for friend goto if inline int long mutable namespace new NULL operator private protected public register reinterpret_cast return short signed sizeof static static_cast struct switch template this throw true try typedef typeid typename union unsigned using virtual void volatile while");
            textBox1.SetKeywords(1, "main WinMain");
            textBox1.Margins[0].Width = 50;
            textBox1.Margins[0].Type = MarginType.Number;
            textBox1.Styles[Style.LineNumber].Font = "Consolas";
            textBox1.Styles[Style.LineNumber].Size = 12;
            textBox1.Styles[Style.LineNumber].ForeColor = Color.Black;
            textBox1.Styles[Style.LineNumber].BackColor = Color.LightGray;        
        }

        public void OpenFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select a File",
                Filter = "All Supported Files (*.cpp;*.cxx;*.c++;*.c;*.h;*.cc;*.cp)|*.cpp;*.cxx;*.c++;*.c;*.h;*.cc;*.cp|C++ Source Files (*.cpp;*.cxx:*.c++;*.cc;*.cp)|*.cpp;*.cxx;*.cc;*.cp|C Source Files (*.c)|*.c|C/C++ Headers (*.h)|*.h"
            };
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
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Title = "Save File",
                FileName = file,
                Filter = "C++ Source Files (*.cpp;*.cxx:*.c++;*.cc;*.cp)|*.cpp;*.cxx;*.cc;*.cp|C Source Files (*.c)|*.c|C/C++ Headers (*.h)|*.h"
            };
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
            MessageBox.Show("Version 1.0.0.0 Alpha 3\nCopyright ©  2023  (Python Object Developers)\nCompiler: MinGW-W64\nWelcome to contribute code!", "About C++ Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            textBox1.Text = "#include <windows.h>\r\n\r\n// This is where all the input to the window goes to.\r\nLRESULT CALLBACK WndProc(HWND hwnd, UINT Message, WPARAM wParam, LPARAM lParam) {\r\n switch(Message) {\r\n        \r\n        // Upon destruction, tell the main thread to stop.\r\n        case WM_DESTROY: {\r\n            PostQuitMessage(0);\r\n            break;\r\n        }\r\n        \r\n        // All other messages (a lot of them) are processed using default procedures.\r\n        default:\r\n            return DefWindowProc(hwnd, Message, wParam, lParam);\r\n    }\r\n    return 0;\r\n}\r\n\r\n// The 'main' function of Win32 GUI programs: this is where execution starts.\r\nint WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow) {\r\n    WNDCLASSEX wc; // A properties struct of our window.\r\n    HWND hwnd; // A 'HANDLE', hence the H, or a pointer to our window.\r\n    MSG msg; // A temporary location for all messages.\r\n\r\n    // Zero out the struct and set the stuff we want to modify.\r\n    memset(&wc,0,sizeof(wc));\r\n    wc.cbSize = sizeof(WNDCLASSEX);\r\n    wc.lpfnWndProc = WndProc; // This is where we will send messages to.\r\n    wc.hInstance = hInstance;\r\n    wc.hCursor = LoadCursor(NULL, IDC_ARROW);\r\n    \r\n    // White, COLOR_WINDOW is just a #define for a system color.\r\n    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW+1);\r\n    wc.lpszClassName = \"WindowClass\";\r\n    wc.hIcon = LoadIcon(NULL, IDI_APPLICATION); // Load a standard icon.\r\n    wc.hIconSm = LoadIcon(NULL, IDI_APPLICATION); // use the name \"A\" to use the project icon.\r\n\r\n    if(!RegisterClassEx(&wc)) {\r\n        MessageBox(NULL, \"Window Registration Failed!\",\"Error!\",MB_ICONEXCLAMATION|MB_OK);\r\n        return 0;\r\n    }\r\n\r\n    hwnd = CreateWindowEx(WS_EX_CLIENTEDGE,\"WindowClass\",\"Caption\",WS_VISIBLE|WS_OVERLAPPEDWINDOW,\n        CW_USEDEFAULT, // x\r\n        CW_USEDEFAULT, // y\r\n        640, // width\r\n        480, // height\r\n        NULL,NULL,hInstance,NULL);\r\n\r\n    if(hwnd == NULL) {\r\n        MessageBox(NULL, \"Window Creation Failed!\",\"Error!\",MB_ICONEXCLAMATION|MB_OK);\r\n        return 0;\r\n    }\r\n\r\n    /*\r\n        This is the heart of our program where all input is processed and \r\n        sent to WndProc. Note that GetMessage blocks code flow until it receives something, so\r\n        this loop will not produce unreasonably high CPU usage\r\n    */\r\n    while(GetMessage(&msg, NULL, 0, 0) > 0) { // If no error is received...\r\n        TranslateMessage(&msg); // Translate key codes to chars if present.\r\n        DispatchMessage(&msg); // Send it to WndProc.\r\n    }\r\n    return msg.wParam;\r\n}";
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
