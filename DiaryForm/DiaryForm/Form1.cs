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
        private List<TagControlData> _tagData = new List<TagControlData>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveDiary(
                title: textBox1.Text,
                date: dateTimePicker1.Value,
                content: richTextBox1.Text
            );
        }

        private void SaveDiary(string title, DateTime date, string content, bool encrypted = false)
        {
            var diary = new DiaryData();
            diary.Title = title;
            diary.Date = date;
            diary.Content = content;
            diary.Encrypted = encrypted;

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

        private void button2_Click(object sender, EventArgs e)
        {
            var password = PasswordTextbox.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("암호를 확인하세요.");
                return;
            }

            while (password.Trim().Length < 32)
            {
                password = password.Trim() + password.Trim();
            }

            byte[] key = password
                .Select(x => (byte)x)
                .Take(32)
                .ToArray();
            byte[] iv = key.Take(16).ToArray();

            var encrypted = Encryptor.EncryptStringToBytes(richTextBox1.Text, key, iv);
            var serialized = string.Join(",", encrypted);

            SaveDiary(
                title: textBox1.Text,
                date: dateTimePicker1.Value,
                content: serialized,
                encrypted: true
            );
        }
    }
}
