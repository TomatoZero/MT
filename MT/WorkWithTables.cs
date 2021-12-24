using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    class WorkWithTables
    {
        public DataTable GetTable(string[] input)
        {
            DataTable output = new DataTable();

            int startNumerating = -input.Length / 2;
            int start2 = startNumerating;

            for (int i = 0; i < input.Length; i++)
            {
                output.Columns.Add(i.ToString()).ColumnName = $"{++startNumerating}";
            }

            DataRow row = output.NewRow();

            for (int i = 0; i < input.Length; i++)
            {
                row[(++start2).ToString()] = input[i];
            }
            output.Rows.Add(row);
            return output;
        }

        public DataTable GetTable(string[,] input)
        {
            DataTable output = new DataTable();

            for (int i = 0; i < input.GetLength(1); i++)
            {
                output.Columns.Add(input[0, i]);
            }

            for (int i = 1; i < input.GetLength(0); i++)
            {
                DataRow row = output.NewRow();

                for (int j = 0; j < input.GetLength(1); j++)
                {
                    row[input[0, j]] = input[i, j].ToString();
                }
                output.Rows.Add(row);
            }


            return output;
        }

        public DataTable GetTable(StringBuilder input)
        {
            string[] arrOutput = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
                arrOutput[i] = input[i].ToString();

            return GetTable(arrOutput);
        }

        /// <summary>
        /// Повертає проміжок з колекції в вигляді DataTable
        /// </summary>
        /// <param name="input">Колекція з якої береться проміжок</param>
        /// <param name="index">індекс початку проміжку</param>
        /// <param name="length">довжина проміжку</param>
        /// <returns></returns>
        public DataTable GetTable(string[] input, int index, int length)
        {
            string[] arrInput = new string[length];

            for (int i = index, j = 0; i < index + length; i++, j++)
            {
                arrInput[j] = input[i];
            }

            return GetTable(arrInput);
        }


        /// <summary>
        /// Повертає проміжок з колекції в вигляді DataTable
        /// </summary>
        /// <param name="input">StringBuilder з якого буреться проміжок</param>
        /// <param name="index">індекс початку проміжку</param>
        /// <param name="length">довжина проміжку</param>
        /// <returns></returns>
        public DataTable GetTable(StringBuilder input, int index, int length)
        {
            string[] arrInput = new string[length];

            for (int i = index, j = 0; i < index + length; i++, j++)
            {
                arrInput[j] = input[i].ToString();
            }

            return GetTable(arrInput);
        }


        /// <summary>
        /// Повертає DataTable з нумерацією стовбців
        /// </summary>
        /// <param name="input">Колекція з якої береться інформація</param>
        /// <param name="startNum">Число з якого потрібно почати нумерацію колонок</param>
        /// <returns></returns>
        public DataTable GetNumberingTable(string[,] input, int startNum)
        {
            DataTable output = new DataTable();

            int start = startNum - 1;

            for (int i = 0; i < input.GetLength(1); i++, start++)
            {
                output.Columns.Add(start.ToString());
            }

            for (int i = 0; i < input.GetLength(0); i++)
            {
                DataRow row = output.NewRow();
                start = startNum - 1;

                for (int j = 0; j < input.GetLength(1); j++, start++)
                {
                    row[start.ToString()] = input[i, j];
                }
                output.Rows.Add(row);
            }

            return output;
        }

        /// <summary>
        /// Повертає DataTable з нумерацією стовбців
        /// </summary>
        /// <param name="input">Колекція з якої береться інформація</param>
        /// <param name="startNum">Число з якого потрібно почати нумерацію колонок</param>
        /// <returns></returns>
        public DataTable GetNumberingTable(StringBuilder input, int startNum)
        {
            string[,] arrOutput = GetArr(input, 0, input.Length);

            return GetNumberingTable(arrOutput, startNum);
        }

        /// <summary>
        /// Повертає DataTable з нумерацією стовбців з частини колекції
        /// </summary>
        /// <param name="input">Колекція з якої береться інформація</param>
        /// <param name="indexStart">Індекс з якого потрібно почати записувати таблицю</param>
        /// <param name="length">Довжина під таблиці</param>
        /// <param name="startNum">Число з якого потрібно почати нумерацію колонок</param>
        /// <returns></returns>
        public DataTable GetNumberingTable(StringBuilder input, int indexStart, int length, int startNum)
        {
            return GetNumberingTable(GetArr(input, indexStart, length), startNum);
        }

        private static string[,] GetArr(StringBuilder input, int indexStart, int length)
        {
            string[,] arrOutput = new string[1, length];

            for (int i = indexStart, j = 0; i < length + indexStart; i++, j++)
                arrOutput[0, j] = input[i].ToString();
            return arrOutput;
        }

        /// <summary>
        /// Повертає таблицю у вигляді матриці
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string[,] ToArr(DataTable input)
        {
            var column = input.Columns;

            string[,] output = new string[input.Rows.Count + 1, column.Count];

            for (int i = 0; i < column.Count; i++)
            {
                output[0, i] = column[i].ColumnName;
            }

            for (int i = 1; i < input.Rows.Count + 1; i++)
            {
                DataRow row = input.Rows[i - 1];

                for (int j = 0; j < column.Count; j++)
                {
                    output[i, j] = row[j].ToString();
                }
            }

            return output;
        }

        public List<string> ToList(DataTable input)
        {
            List<string> output = new List<string>();
            output.Add("");

            var column = input.Columns;
            for (int i = 0; i < column.Count; i++)
            {
                if (i == 0)
                    output[0] += "@ ";
                else
                    output[0] += column[i].ColumnName + " ";
            }

            for (int i = 0; i < input.Rows.Count; i++)
            {
                DataRow row = input.Rows[i];
                output.Add("");

                for (int j = 0; j < column.Count; j++)
                {
                    if (row[j].ToString() == " " || String.IsNullOrEmpty(row[j].ToString()))
                        output[i + 1] += "@ ";
                    else
                        output[i + 1] += row[j].ToString() + " ";
                }
            }

            return output;
        }

        public DataTable AddPointerToTable(DataTable input, int pointer)
        {
            var column = input.Columns;

            DataRow row = input.NewRow();

            for (int i = 0; i < column.Count; i++)
            {
                if (column[i].ColumnName == pointer.ToString())
                {
                    row[i] = "^";
                }
                else
                    row[i] = "-";
            }
            input.Rows.Add(row);

            return input;
        }
    }
}
