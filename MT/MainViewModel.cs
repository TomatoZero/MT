using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MT
{
    class MainViewModel : ViewModelBase
    {
        private MainWindow mw;

        private StringBuilder myTape;

        private DataTable dataTableCollection;
        private DataTable dataTablePart;
        private DataTable program;

        private string Alphabet;
        private string word;
        private int pointer;

        private Turing turing;
        private ObservableCollection<TapeCondition> tapeConditions;
        private ObservableCollection<TapeCondition> specialTapeCondeteions;

        public DataTable DataTableCollection
        {
            get => dataTableCollection;
            set
            {
                dataTableCollection = value;
                OnPropertyChanged("dataTableCollection");
            }
        }
        public DataTable DataTablePart
        {
            get => dataTablePart;
            set
            {
                dataTablePart = value;
                OnPropertyChanged("dataTablePart");
            }
        }
        public DataTable Program
        {
            get => program;
            set
            {
                program = value;
                OnPropertyChanged("program");
            }
        }
        public StringBuilder MyTape
        {
            get => myTape;
            set => myTape = value;
        }
        public Turing Turing { get => turing; set => turing = value; }
        public ObservableCollection<TapeCondition> TapeConditions
        {
            get => tapeConditions;
            set
            {
                tapeConditions = value;
                OnPropertyChanged("TapeConditions");
            }
        }

        public string Word { get => word; set => word = value; }
        public int Pointer { get => pointer; set => pointer = value; }
        public ObservableCollection<TapeCondition> SpecialTapeCondeteions { get => specialTapeCondeteions; set => specialTapeCondeteions = value; }

        public WorkWithTables withTables = new WorkWithTables();


        public MainViewModel(MainWindow mw)
        {
            this.mw = mw;

            MyTape = new StringBuilder(new String('#', 200));

            DataTableCollection = withTables.GetNumberingTable(MyTape, -99);

            DataTablePart = withTables.GetNumberingTable(MyTape, 90, 30, -10);
            DataTablePart = withTables.AddPointerToTable(DataTablePart, Pointer);

            Turing = new Turing();

            mw.getWord.Click += GetWord_Click;
            mw.getAlphabet.Click += GetAlphabet_Click;
            mw.Play.Click += Play_Click;
            mw.openFile.Click += OpenFile_Click;
            mw.Save.Click += Save_Click;
            mw.newFile.Click += NewFile_Click;

            mw.startByStep.Click += StartByStep_Click;
            mw.nextStep.Click += NextStep_Click;
            mw.previousStep.Click += PreviousStep_Click;

        }

        private void PreviousStep_Click(object sender, RoutedEventArgs e)
        {
            if (TapeConditions.Count == 0 || TapeConditions.Count == 1)
                return;

            int i = TapeConditions.Count - 1;

            TapeConditions.RemoveAt(i);
            MyTape = SpecialTapeCondeteions[i - 1].Tape;
            Pointer = SpecialTapeCondeteions[i - 1].Pointer - 100;

            ChangeDataTables();
        }

        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (SpecialTapeCondeteions == null || SpecialTapeCondeteions.Count == TapeConditions.Count)
                return;

            int i = TapeConditions.Count - 1;
            TapeConditions.Add(SpecialTapeCondeteions[i]);

            MyTape = SpecialTapeCondeteions[i].Tape;
            Pointer = SpecialTapeCondeteions[i].Pointer - 100;

            ChangeDataTables();

        }

        private void ChangeDataTables()
        {
            DataTableCollection = withTables.GetNumberingTable(MyTape, -99);

            int indexStartWord = 0;
            bool check = false;
            for (int i = 0; i < MyTape.Length; i++)
            {
                if (MyTape[i] != '#')
                {
                    indexStartWord = i;
                    check = true;
                    break;
                }
            }

            if (check == true)
            {
                DataTablePart = withTables.GetNumberingTable(MyTape, indexStartWord - 1, 30, (indexStartWord) - 100);
                DataTablePart = withTables.AddPointerToTable(DataTablePart, Pointer);
            }
            else
            {
                DataTablePart = withTables.GetNumberingTable(MyTape, 0, 30, (indexStartWord) - 100);
                DataTablePart = withTables.AddPointerToTable(DataTablePart, Pointer);
            }
        }

        private void StartByStep_Click(object sender, RoutedEventArgs e)
        {
            if (!Checking())
                return;

            int pointer = Convert.ToInt32(mw.pointer.Text.Trim());

            Turing.Action = withTables.ToArr(Program);
            Turing.TuringMachine(Word, pointer);

            int l = Word.Length / 2;

            SpecialTapeCondeteions = Turing.TapeConditions;

            TapeConditions = new ObservableCollection<TapeCondition>();
            TapeConditions.Add(SpecialTapeCondeteions[0]);

            MyTape = SpecialTapeCondeteions[0].Tape;

            DataTableCollection = withTables.GetNumberingTable(MyTape, -99);

            int indexStartWord = 0;
            for (int i = 0; i < MyTape.Length; i++)
            {
                if (MyTape[i] != '#')
                {
                    indexStartWord = i;
                    break;
                }
            }

            DataTablePart = withTables.GetNumberingTable(MyTape, indexStartWord - 1, 30, (indexStartWord) - 100);
            DataTablePart = withTables.AddPointerToTable(DataTablePart, Pointer);
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFile saveFile = new SaveFile();

            saveFile.Save.Click += Save_Click;
            saveFile.Save.Click += NewFile;
            saveFile.DontSave.Click += NewFile;

            TapeConditions = new ObservableCollection<TapeCondition>();
            SpecialTapeCondeteions = new ObservableCollection<TapeCondition>();

            MyTape = new StringBuilder(new String('#', 200));
            ChangeDataTables();

            mw.alphabet.Text = "";

            saveFile.ShowDialog();
        }

        private void NewFile(object sender, RoutedEventArgs e)
        {
            Alphabet = "";

            Program = new DataTable();
            MyTape = new StringBuilder(new String('#', 200));
            DataTableCollection = withTables.GetNumberingTable(myTape, -99);

            DataTablePart = withTables.GetNumberingTable(myTape, 90, 30, -10);
            DataTablePart = withTables.AddPointerToTable(DataTablePart, Pointer);

            wordIsEntered = false;
            mw.alphabet.IsReadOnly = false;
            mw.alphabet.Text = "";
            mw.pointer.Text = "";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Program == null)
                return;

            SaveFileDialog saveFile = new SaveFileDialog();
            List<string> output = withTables.ToList(Program);

            if (saveFile.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFile.FileName, false, Encoding.UTF8))
                {
                    for (int i = 0; i < output.Count; i++)
                        writer.WriteLine(output[i]);
                }
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            List<string> programFromFile = new List<string>();

            if (openFile.ShowDialog() == true)
            {
                using (StreamReader reader = new StreamReader(openFile.FileName, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        programFromFile.Add(reader.ReadLine());
                    }
                }
            }
            else
                return;


            string[] s = programFromFile[0].Trim().Split();

            string alphabet = "";

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != "@")
                {
                    alphabet += s[i];
                }
            }

            string[,] act = new string[programFromFile.Count, s.Length];

            for (int i = 0; i < programFromFile.Count; i++)
            {
                string[] str = programFromFile[i].Trim().Split();

                for (int j = 0; j < s.Length; j++)
                {
                    if (str[j] == "@")
                        act[i, j] = " ";
                    else
                        act[i, j] = str[j];
                }
            }

            act[0, 0] = "@";

            Alphabet = alphabet;
            mw.alphabet.Text = Alphabet;
            mw.alphabet.IsReadOnly = true;
            Program = withTables.GetTable(act);
        }


        bool wordIsEntered = false;
        private void GetWord_Click(object sender, RoutedEventArgs e)
        {
            string s = mw.myWord.Text.Trim();

            if (String.IsNullOrEmpty(Alphabet))
            {
                MessageBox.Show("Введіть алфавіт");
                return;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if (!Alphabet.Contains(s[i]) || s[i] == ' ')
                {
                    MessageBox.Show("В слові є некоректні символи");
                    return;
                }
            }

            wordIsEntered = true;
            int l = s.Length / 2;

            Word = s;

            myTape = new StringBuilder();

            myTape.Append(new String('#', 100 - l));
            myTape.Append(s);
            myTape.Append(new String('#', 200 - MyTape.Length));

            DataTableCollection = withTables.GetNumberingTable(myTape, -99);

            int indexStartWord = 0;
            for (int i = 0; i < MyTape.Length; i++)
            {
                if (MyTape[i] != '#')
                {
                    indexStartWord = i;
                    break;
                }
            }

            DataTablePart = withTables.GetNumberingTable(MyTape, indexStartWord - 1, 30, (indexStartWord) - 100);
            DataTablePart = withTables.AddPointerToTable(DataTablePart, Pointer);
        }

        private void GetAlphabet_Click(object sender, RoutedEventArgs e)
        {
            Alphabet = new string(("#" + mw.alphabet.Text).ToCharArray().Distinct().ToArray());

            string[,] output = new string[1, Alphabet.Length + 1];

            output[0, 0] = $"@";

            for (int i = 1; i < output.GetLength(1); i++)
            {
                output[0, i] = Alphabet[i - 1].ToString();
            }

            Program = withTables.GetTable(output);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (!Checking())
                return;

            int pointer = Convert.ToInt32(mw.pointer.Text.Trim());

            Turing.Action = withTables.ToArr(Program);
            Turing.TuringMachine(Word, pointer);

            int l = Word.Length / 2;



            MyTape = Turing.TapeC.Tape;

            ChangeDataTables();

            TapeConditions = Turing.TapeConditions;
            SpecialTapeCondeteions = Turing.TapeConditions;
        }

        private bool Checking()
        {
            var t = mw.action.ItemsSource as DataView;

            if (t == null)
            {
                MessageBox.Show("Програма не записана");
                return false;
            }

            var dataTable = withTables.ToArr(t.ToTable());

            bool can = false;

            if (!String.IsNullOrEmpty(mw.pointer.Text))
            {
                if (mw.pointer.Text.Length == 1 && mw.pointer.Text[0] == '-')
                { }
                else
                {
                    int pointer = Convert.ToInt32(mw.pointer.Text);

                    if (pointer >= -99 && pointer <= 100)
                        can = true;
                }
            }
            if (can == false)
            {
                MessageBox.Show("Таку позицію для показника задати неможна");
                return false;
            }

            if (wordIsEntered == false)
            {
                MessageBox.Show("Введіть слово");
                return false;
            }

            return true;
        }

    }
}
