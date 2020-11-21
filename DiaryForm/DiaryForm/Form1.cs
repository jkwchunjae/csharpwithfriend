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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var date = dateTimePicker1.Value;
            var text = richTextBox1.Text;
            var diary = new DiaryData();
            diary.Content = text;
            var jsonText = JsonConvert.SerializeObject(diary);

            var path = @"C:\Users\jkwch\Documents\diary\test";
            // var fileName0 = date.ToString("yyyyMMdd") + ".txt"; // 20201114.txt
            var fileName = $"{date:yyyyMMdd}.json"; // 20201114.json
            var diaryPath = Path.Combine(path, fileName);

            File.WriteAllText(diaryPath, jsonText);
        }
    }
}
