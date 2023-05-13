using Sudoku.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Sudoku
{
    static class SaveHelper
    {
        #region field

        private static string saveFolder
        {
            get { return Application.StartupPath + @"\Savefiles\"; }
        }
        private static string[] filename =
        {
            "Setting.xml", "Puzzles.xml", "Records.xml"
        };
        private static XmlDocument[] docs = new XmlDocument[3];

        #endregion

        #region function
        
        private static string getLocation(int index)
        {
            return saveFolder + filename[index];
        }

        #endregion

        #region property

        public enum DocsIndex
        {
            Setting, Puzzle, Record
        }
        public struct AttrNames
        {
            public const string Language = "Language";
        }

        #endregion

        #region api
        public static void Init()
        {
            for(int i = 0; i < docs.Length; i++)
            {
                docs[i] = new XmlDocument();
                docs[i].LoadXml(File.ReadAllText(getLocation(i)));
            }
        }
    
        public static void Save(DocsIndex docsIndex)
        {
            int index = (int)docsIndex;
            docs[index].Save(getLocation(index));
        }

        public static XmlDocument getDoc(DocsIndex index)
        {
            return docs[(int)index];
        }

        #endregion
    }

    static class SettingHelper
    {
        public enum Language
        {
            English, Chinese
        }
        private static Language _language;
        public static Language language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
                setting.SetAttribute(SaveHelper.AttrNames.Language, ((int)_language).ToString());
                SaveHelper.Save(SaveHelper.DocsIndex.Setting);

                if (Form1.instance == null) return;

                Form1.instance.RefreshText();
            }
        }
            
        private static XmlElement setting;

        public static void Init()
        {
            setting = SaveHelper.getDoc(SaveHelper.DocsIndex.Setting).DocumentElement;
            language = (Language)(setting.GetAttribute(SaveHelper.AttrNames.Language)[0] - '0');
        }

        public static void InitText(Forms forms, Control[] UIControls)
        {
            StringReader UITextReader;
            UITextReader = new StringReader(TextContent[(int)forms][(int)language]);

            for (int i = 0; i < UIControls.Length; i++)
                UIControls[i].Text = UITextReader.ReadLine();
        }

        public static string[][] TextContent =
        {
            new string[] {Resources.Menu_English, Resources.Menu_Chinese},
            new string[] {Resources.Setting_English, Resources.Setting_Chinese},
            new string[] {Resources.Menu_English, Resources.Menu_Chinese},
            new string[] {Resources.Menu_English, Resources.Menu_Chinese}
        };
    }

    public enum Forms
    {
        Menu, Setting
    }
}
