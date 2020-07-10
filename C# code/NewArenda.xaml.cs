using System;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace bicycles
{
    public partial class NewArenda : Window
    {

        string idBycle, Price, Diposit;

        public NewArenda(string idBycle, string Price, string Diposit)
        {
            InitializeComponent();
            this.idBycle = idBycle;
            this.Price = Price;
            this.Diposit = Diposit;
        }

        string _FIO, _Passport, _Televon, _Sum;
        DateTime _DateText;

        public static string idManager;

        bool CheckClosed, Accept;

        private string FIO 
        { 
            get { return _FIO; } 
            set 
            {
                if (value != string.Empty)
                {
                    RightInputFIO = true;
                    _FIO = value;
                }
                else
                    MessageBox.Show("Поле ФИО пустое");
            }
        }

        private string Passport
        {
            get { return _Passport; }
            set
            {
                if (value.Length == 10)
                {
                    RightInputPassport = true;
                    _Passport = value;
                }
                else
                    MessageBox.Show("Данные некорректного формата");
            }
        }

        private string Televon
        {
            get { return _Televon; }
            set
            {
                if (value.Replace("_", "").Length == 16)
                {
                    RightInputPhone = true;
                    _Televon = value;
                }
                else
                    MessageBox.Show("Данные некорректного формата");
            }
        }

        private DateTime Date
        { 
            get { return _DateText; }
            set 
            {
                if (value > DateTime.Now.AddDays(1))
                {
                    RightInputDate = true;
                    _DateText = value;
                }
                else
                    MessageBox.Show("Данные некорректные");
            }
        }
        
        bool RightInputFIO, RightInputDate, RightInputPhone, RightInputPassport;

        private void PassportText__TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (PassportText_.Text.Length > 10)
                PassportText_.Text = PassportText_.Text.Remove(PassportText_.Text.Length - 1);
        }

        private void ADD(object sender, RoutedEventArgs e)
        {
            if (RightInputFIO && RightInputDate && RightInputPhone && RightInputPassport)
            {
                int nowTime = DateTime.Today.DayOfYear;
                int endTime = Date.DayOfYear;

                int Arenda = (endTime - nowTime) * int.Parse(Price);
                string textCommand = $"INSERT INTO offers(start_date, end_date, bicycle, rent, deposit, manager_id, passport, fio, phone)" +
                $" VALUES ('{DateTime.Today:yyyy-MM-dd}', '{Date:yyyy-MM-dd}', '{idBycle}', '{Arenda}', '{_Sum}', '{idManager}', '{Passport}', '{FIO}', '{Televon}')";
                MySQLItems.RunCommand(textCommand);
                Accept = true;
                Close();
            }
            else
                MessageBox.Show("Неправильные данные");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Accept)
            {
                object nameNewFile = Environment.CurrentDirectory + $"\\Dogovor\\{_FIO}.doc";
                File.Copy(Environment.CurrentDirectory + "\\Dogovor\\Pattern.doc", nameNewFile.ToString(), true);
                Word.Application app = new Word.Application();
                app.Documents.Open(ref nameNewFile);
                Word.Find find = app.Selection.Find;
                for (int i = 1; i < 12; i++)
                {
                    find.Text = "[" + i + "]";
                    string textReplace = string.Empty;
                    switch (i)
                    {
                        case 1:
                            textReplace = "1";
                            break;
                        case 2:
                            textReplace = _FIO;
                            break;
                        case 3:
                            textReplace = _Passport;
                            break;
                        case 4:
                            textReplace = _Televon;
                            break;
                        case 5:
                            textReplace = idBycle;
                            break;
                        case 6:
                            textReplace = _Sum;
                            break;
                        case 7:
                            textReplace = Price;
                            break;
                        case 8:
                            textReplace = "";
                            break;
                        case 9:
                            textReplace = DateTime.Now.ToShortDateString();
                            break;
                        case 10:
                            textReplace = Date.ToShortDateString();
                            break;
                        case 11:
                            textReplace = _FIO;
                            break;
                    }
                    find.Replacement.Text = textReplace;
                    find.Execute(FindText: Type.Missing,
                                MatchCase: true,
                                MatchWholeWord: true,
                                MatchWildcards: false,
                                MatchSoundsLike: Type.Missing,
                                MatchAllWordForms: false,
                                Forward: true,
                                Wrap: Word.WdFindWrap.wdFindContinue,
                                Format: false,
                                ReplaceWith: Type.Missing,
                                Replace: Word.WdReplace.wdReplaceAll);
                }
                app.ActiveDocument.Save();
                app.ActiveDocument.Close();
                app.Quit();

                CheckClosed = true;
            }
        }

        private void FIOText(object sender, RoutedEventArgs e)
        {
            if(!CheckClosed)
                FIO = FIOText_.Text;
        }

        private void PassportText(object sender, RoutedEventArgs e)
        {
            if(!CheckClosed)
                Passport = PassportText_.Text;
        }

        private void TelevonText(object sender, RoutedEventArgs e)
        {
            if(!CheckClosed)
                Televon = TelevonText_.Text;
        }

        private void DateText(object sender, RoutedEventArgs e)
        {
            DateTime end;
            if (!CheckClosed)
                if (DateTime.TryParse(DateText_.Text, out end))
                {
                    Date = end;
                    SumText.Text = _Sum = Convert.ToString(Math.Round((Date - DateTime.Now).TotalDays * int.Parse(Diposit)));
                }
                else
                    MessageBox.Show("Данные некорректного формата");
        }
    }
}