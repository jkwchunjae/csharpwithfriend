using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiaryForm
{
    public partial class Form1 : Form
    {
        private DiaryData diary;
        public Form1()
        {
            InitializeComponent();
            diary = new DiaryData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var date = dateTimePicker1.Value;

            diary.Title = textBox1.Text;
            diary.Date = date;
            diary.Content = richTextBox1.Text;

            var jsonText = JsonConvert.SerializeObject(diary, Formatting.Indented);

            var path = @"C:\Users\jkwch\Documents\diary\test";
            // var fileName0 = date.ToString("yyyyMMdd") + ".txt"; // 20201114.txt
            var fileName = $"{date:yyyyMMdd}.json"; // 20201114.json
            var diaryPath = Path.Combine(path, fileName);

            File.WriteAllText(diaryPath, jsonText);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var y = 338;
                var newTagLabel = new Label();
                newTagLabel.Text = textBox2.Text;
                newTagLabel.SetBounds(
                    textBox2.Left,
                    y,
                    100,
                    30);
                textBox2.Left += 100;

                diary.Tags.Add(textBox2.Text);

                this.Controls.Add(newTagLabel);
            }
        }
    }
}
