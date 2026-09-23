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

namespace SentimentApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public string RunPythonModel(string text)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"sentiment_model.py \"{text}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true

            };

            using (Process process = Process.Start(psi) )
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output.Trim();
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            string inputText = txtInput.Text;
            string result = RunPythonModel(inputText);
            lblresult.Text = result;
        }


    }
}
