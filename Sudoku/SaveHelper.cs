using Sudoku.Properties;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using System.Collections.Generic;
using System;

namespace Sudoku
{
    static class SaveHelper
    {
        #region field

        private static string[] filename =
        {
            "Setting.json", "Puzzle.json"
        };

        private static object getSaveThings(int index)
        {
            switch (index)
            {
                case 0:
                    return setting;
                case 1:
                    return puzzles;
            }
            return null;
        }

        #endregion

        #region function

        private static string getLocation(int index)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sudoku\" + filename[index];
        }

        #endregion

        #region property

        public class Setting
        {
            public Language language { get; set; }

            public void InitText(Forms forms, Control[] UIControls)
            {
                StringReader UITextReader;
                UITextReader = new StringReader(TextContent[(int)forms][(int)language]);

                for (int i = 0; i < UIControls.Length; i++)
                    UIControls[i].Text = UITextReader.ReadLine();
            }

            private static string[][] TextContent =
            {
                new string[] {Resources.Menu_English, Resources.Menu_Chinese},
                new string[] {Resources.Setting_English, Resources.Setting_Chinese},
                new string[] {Resources.Puzzle_English, Resources.Puzzle_Chinese},
                new string[] {Resources.Choose_English, Resources.Choose_Chinese},
                new string[] {Resources.Play_English, Resources.Play_Chinese}
            };
        }

        public enum DocsIndex
        {
            Setting, Puzzle
        }

        public static Setting setting;

        public static List<Puzzle>[] puzzles;
        public static readonly string[][] difficultyStrings =
        {
            new string[] { "Easy", "Normal", "Hard" },
            new string[] { "簡單", "普通", "困難" }
        };

        #endregion

        #region api

        public static void Init()
        {
            setting = JsonSerializer.Deserialize<Setting>(File.ReadAllText(getLocation((int)DocsIndex.Setting)));
            puzzles = JsonSerializer.Deserialize<List<Puzzle>[]>(File.ReadAllText(getLocation((int)DocsIndex.Puzzle)));
        }

        public static void Save(DocsIndex docsIndex, string location = null)
        {
            int index = (int)docsIndex;
            File.WriteAllText(location ?? getLocation(index), JsonSerializer.Serialize(getSaveThings(index)));
        }

        #endregion
    }

    public class Puzzle
    {
        public Puzzle()
        {
            Sudoku = new int[9][];
            for (int i = 0; i < 9; i++)
                Sudoku[i] = new int[9];
        }
        public int[][] Sudoku { get; set; }
    }

    public enum Language
    {
        English, 中文
    }

    public enum Forms
    {
        Menu, Setting, Puzzle, Choose, Play
    }

    public enum Difficulty
    {
        Easy, Normal, Hard
    }
}
