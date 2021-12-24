using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    class TapeCondition : ICloneable
    {
        private int pointer;

        private string curentState;
        private int indexState;

        private char curentChar;
        private int indexChar;

        private StringBuilder tape;
        private StringBuilder word;


        public int Pointer { get => pointer; set => pointer = value; }
        public int RealPointer { get => pointer - 100; }

        public string CurentState { get => curentState; set => curentState = value; }
        public int IndexState { get => indexState; set => indexState = value; }

        public char CurentChar { get => curentChar; set => curentChar = value; }
        public int IndexChar { get => indexChar; set => indexChar = value; }

        public StringBuilder Tape { get => tape; set => tape = value; }
        public StringBuilder Word { get => word; set => word = value; }

        public TapeCondition()
        {
            Tape = new StringBuilder(new String('#', 200));
            Word = new StringBuilder(new String('#', 20));
        }

        public void AddWord(int indexStart)
        {
            string s = "";

            for (int i = indexStart; i < indexStart + 30; i++)
            {
                if (Tape[i] == '#' && Tape[i + 1] == '#')
                    break;

                s += Tape[i];
            }

            Word = new StringBuilder(s);
        }

        public object Clone()
        {
            StringBuilder newTape = new StringBuilder(Tape.ToString());
            StringBuilder newPartTape = new StringBuilder(Word.ToString());

            return new TapeCondition
            {
                Pointer = this.Pointer,

                CurentState = this.CurentState,
                IndexState = this.IndexState,

                CurentChar = this.CurentChar,
                IndexChar = this.IndexChar,

                Tape = newTape,
                Word = newPartTape
            };
        }

        public override string ToString()
        {
            string s = $"Pointer {Pointer}\t Curent State: {CurentState}\t Curent Char: {CurentChar}\nPart Tape: {Word}";

            return s;
        }
    }
}
