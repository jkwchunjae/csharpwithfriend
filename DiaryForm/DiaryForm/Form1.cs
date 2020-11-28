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
    public class TagControlData
    {
        public int Index { get; set; }
        public string TagText { get; set; }
        public Label Control { get; set; }
        public Button DeleteButton { get; set; }
    }

    public partial class Form1 : Form
    {
        private List<TagControlData> _tagData = new List<TagControlData>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var date = dateTimePicker1.Value;

            var diary = new DiaryData();
            diary.Title = textBox1.Text;
            diary.Date = date;
            diary.Content = richTextBox1.Text;

            diary.Tags = _tagData
                .Select(x => x.TagText)
                .ToList();

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
                var tagText = textBox2.Text;
                var newTagLabel = new Label();
                newTagLabel.Text = tagText;

                var deleteButton = new Button();
                deleteButton.Text = "X";
                deleteButton.Click += (_, __) =>
                {
                    try
                    {
                        var removedData = _tagData
                            .FirstOrDefault(x => x.Control == newTagLabel);

                        if (removedData != null)
                        {
                            _tagData.Remove(removedData);
                            Controls.Remove(newTagLabel);
                            Controls.Remove(deleteButton);
                            RedrawTagList();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                };

                var tagControlData = new TagControlData
                {
                    TagText = tagText,
                    Control = newTagLabel,
                    DeleteButton = deleteButton,
                };

                _tagData.Add(tagControlData);

                this.Controls.Add(deleteButton);
                this.Controls.Add(newTagLabel);

                RedrawTagList();

                textBox2.Text = "";
            }
        }

        private void RedrawTagList()
        {
            int index = 0;
            var y = 338;

            foreach (var tagData in _tagData)
            {
                tagData.Control.SetBounds(
                    index * 100 + 12,
                    y,
                    70,
                    30);
                tagData.DeleteButton.SetBounds(
                    index * 100 + 12 + 70,
                    y,
                    30,
                    30);

                index++;
            }

            textBox2.Left = index * 100 + 12;
        }
    }
}
