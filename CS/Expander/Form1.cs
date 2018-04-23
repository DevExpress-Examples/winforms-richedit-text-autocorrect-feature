using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
#region #usings
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Services;
#endregion #usings

namespace Expander
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richEditControl1.CreateNewDocument();
            richEditControl1.Document.AppendText(@"This sample expander uses Mary Morken's 2007 list of abbreviations for medical transcriptions, containing 13420 entries. 
The list has been downloaded from http://www.mtdaily.com/abbvs.txt. To indicate the beginning of the list, the line containing the 'START' string is added.");
#region #autocorrectservice
            IAutoCorrectService svc = richEditControl1.GetService<IAutoCorrectService>();
            if (svc != null)
                svc.SetReplaceTable(LoadAbbrevs("abbvs.txt"));
#endregion #autocorrectservice
        }

#region #setreplacetable
        private AutoCorrectReplaceInfoCollection LoadAbbrevs(string path)
        {
            AutoCorrectReplaceInfoCollection coll = new AutoCorrectReplaceInfoCollection();
            string aLine = "";

            AutoCorrectReplaceInfo acrInfoIm = new AutoCorrectReplaceInfo(":-)", CreateImageFromResx("smile.png"));
            coll.Add(acrInfoIm);

            if (File.Exists(path)) {
                StreamReader sr = new StreamReader(path);
                while (!(sr.EndOfStream)) {
                    aLine = sr.ReadLine();
                    if (aLine != "START") continue;

                    while (!(sr.EndOfStream)) {
                        aLine = sr.ReadLine();
                        aLine = aLine.Trim();
                        string[] words = aLine.Split('=');
                        if (words.Length == 2) {
                            AutoCorrectReplaceInfo acrInfo = new AutoCorrectReplaceInfo(words[0], words[1]);
                            coll.Add(acrInfo);
                        }
                    }
                }
                sr.Close();
            }
            return coll;
        }
#endregion #setreplacetable

        private Image CreateImageFromResx(string name)
        {
            Image im;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("Expander.Images." + name)) {
                im = Image.FromStream(stream);
            }
            return im;
        }
    }
}