namespace WordsFromWord
{
    public partial class Main : Form
    {
        private List<string> words = new();
        private string[] result = Array.Empty<string>();
        private bool haveDictionary;

        public Main()
        {
            InitializeComponent();
            LoadDefaultDictionary();
        }

        private void LoadDefaultDictionary()
        {
            try
            {
                LoadDictionary();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Словарь по умолчанию не найден!\nДля работы программы добавьте словарь вручную.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                haveDictionary = false;
            }
        }

        private void LoadDictionary()
        {
            words = File.ReadAllLines(openFileDialog.FileName).ToList();
            dictNameLabel.Text = Path.GetFileName(openFileDialog.FileName);
            wordCountLabel.Text = words.Count.ToString();
            haveDictionary = true;
            EnableSearch();
        }

        private string[] FindWords()
        {
            var result = new List<string>();
            var input = wordTextBox.Text;
            foreach (var word in words)
            {
                if (word.All(input.Contains))
                {
                    var temp = input.ToList();
                    var valid = true;
                    foreach (var symbol in word)
                    {
                        if (temp.Contains(symbol))
                        {
                            temp.Remove(symbol);
                        }
                        else
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                    {
                        result.Add(word);
                    }
                }
            }
            return result.ToArray();
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            wordTextBox.Text =  wordTextBox.Text.ToLower();
            resultsListBox.Items.Clear();
            result = FindWords().OrderBy(x => x).ToArray();
            resultsListBox.Items.AddRange(result);
            resultWordCountLabel.Text = result.Length.ToString();
            CheckOptions();
        }

        private void chooseDictButton_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadDictionary();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = "result";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var text = "Слово: " + wordTextBox.Text +"\nРезультат: " + "\n" + string.Join("\n", result);
                File.WriteAllText(saveFileDialog.FileName, text);
            }
        }

        private void CheckOptions() => saveToolStripMenuItem.Enabled = resultsListBox.Items.Count != 0;

        private void EnableSearch() => findButton.Enabled = wordTextBox.Text != string.Empty && haveDictionary;

        private void helpToolStripMenuItem_Click(object sender, EventArgs e) => Help.ShowHelp(this, "Resources/Help.chm");

        private void wordTextBox_TextChanged(object sender, EventArgs e) => EnableSearch();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void Main_HelpRequested(object sender, HelpEventArgs hlpevent) => Help.ShowHelp(this, "Resources/Help.chm");
    }
}