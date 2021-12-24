using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MT
{
    class Turing
    {
        private string[,] action;
        private TapeCondition tapeC;
        private ObservableCollection<TapeCondition> tapeConditions;

        public string[,] Action { get => action; set => action = value; }
        public TapeCondition TapeC { get => tapeC; set => tapeC = value; }
        public ObservableCollection<TapeCondition> TapeConditions { get => tapeConditions; set => tapeConditions = value; }

        public Turing()
        {
            TapeC = new TapeCondition();
            TapeConditions = new ObservableCollection<TapeCondition>();
        }

        public Turing(string[,] act) : this() { Action = act; }

        public void TuringMachine(string w, int pointer)
        {
            if (Action == null || String.IsNullOrEmpty(w))
                return;

            int l = w.Length / 2;

            TapeC.Tape = new StringBuilder(new String('#', 100 - l));
            TapeC.Tape.Append(w);
            TapeC.Tape.Append(new String('#', 200 - TapeC.Tape.Length));

            TapeC.Pointer = pointer + 100;
            TapeC.CurentState = Action[1, 0];

            TapeC.AddWord(100 - l);
            TapeConditions.Add((TapeCondition)TapeC.Clone());

            Machine(l);
        }

        private void Machine(int l)
        {
            bool run = true;
            while (run)
            {
                TapeC.CurentChar = TapeC.Tape[TapeC.Pointer];

                for (int i = 1; i < Action.GetLength(1); i++)
                {
                    if (Action[0, i] == TapeC.CurentChar.ToString())
                    {
                        TapeC.IndexChar = i;
                        break;
                    }
                }

                if (TapeC.IndexChar == 0)
                    return;

                for (int i = 0; i < Action.GetLength(0); i++)
                {
                    if (Action[i, 0] == TapeC.CurentState)
                    {
                        TapeC.IndexState = i;
                        break;
                    }
                }

                if (TapeC.IndexState == 0)
                    return;

                run = ToAssignAction(l);
            }
        }

        private bool ToAssignAction(int l)
        {
            string act = Action[TapeC.IndexState, TapeC.IndexChar];

            if (String.IsNullOrWhiteSpace(act))
            {
                MessageBox.Show("Записана програма є з помилками.");
                return false;
            }

            TapeC.AddWord(100 - l);
            TapeConditions.Add((TapeCondition)TapeC.Clone());

            if (IsInAlphabet(act[0]))
            {
                TapeC.Tape[TapeC.Pointer] = act[0];

                if (!MiniToDoAction(act, 0))
                    return false;
            }
            else
                if (!MiniToDoAction(act, -1))
                return false;

            return true;
        }

        private bool MiniToDoAction(string act, int i)
        {
            string state = DoAction(act, i);

            if (state == "Stop" || state == "qz")
                return false;

            if (!String.IsNullOrEmpty(state))
                TapeC.CurentState = state;
            return true;
        }

        private string DoAction(string act, int i)
        {
            string currentState = "";

            if (act == "qz")
                return "Stop";

            if (act[1 + i] == 'L')
                TapeC.Pointer--;
            else if (act[1 + i] == 'R')
                TapeC.Pointer++;
            else if (act[1 + i] == 'E') { }
            else if (act[i + 1] == 'q')
                return $"q{act.Substring(2 + i, act.Length - 2 - i)}";
            else
            {
                MessageBox.Show("Записана програма є з помилками.");
                return "Stop";
            }

            if (act.Length <= 2 + i) { return currentState; }

            if (act[2 + i] == 'q')
                currentState = $"q{act.Substring(3 + i, act.Length - 3 - i)}";
            else
            {
                MessageBox.Show("Записана програма є з помилками.");
                return "Stop";
            }

            return currentState;
        }

        private bool IsInAlphabet(char act)
        {
            for (int i = 0; i < Action.GetLength(1); i++)
            {
                if (Action[0, i] == act.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public string ReturnWord()
        {
            string word = "";

            for (int i = 0; i < TapeC.Tape.Length; i++)
            {
                if (TapeC.Tape[i] != '#')
                    word += TapeC.Tape[i];
            }
            return word;
        }
    }
}
