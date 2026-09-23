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
                Arguments = $"\"C:\\Users\\glenn\\Downloads\\SentimentApp\\sentiment_model.py\" \"{text}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,   // capture errors too
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = "C:\\Users\\glenn\\Downloads\\SentimentApp"
           

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

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            var testData = new[]
    {
        new { Text = "I love this product!", Expected = "POSITIVE" },
        new { Text = "This is terrible.", Expected = "NEGATIVE" },
        new { Text = "The movie was fantastic!", Expected = "POSITIVE" },
        new { Text = "I hate waiting in line.", Expected = "NEGATIVE" }
    };

            int correct = 0;

            foreach (var item in testData)
            {
                string result = RunPythonModel(item.Text);
                MessageBox.Show($"Raw result: {result}");

                // Split into lines and take the last one
                string[] lines = result.Split(
                    new[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries
                );

                if (lines.Length == 0)
                {
                    MessageBox.Show($"No output for: {item.Text}\nRaw result: {result}");
                    continue;
                }

                string lastLine = lines[lines.Length - 1];
                string predicted = lastLine.Split('|')[0].Trim();

                MessageBox.Show($"Text: '{item.Text}'\nExpected: '{item.Expected}'\nPredicted: '{predicted}'");

                if (predicted.Equals(item.Expected, StringComparison.OrdinalIgnoreCase))
                    correct++;
            }

            // ✅ Accuracy calculated once after all test cases
            double accuracy = (double)correct / testData.Length * 100;
            MessageBox.Show($"Evaluation Accuracy: {accuracy}%");
        }


    }
}
