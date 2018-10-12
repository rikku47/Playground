using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Playground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Group group = new Group("Group1");

            group.Links.Add(new Link("http://mtv-grone.de"));

            group.Links.Add(new Link("mtv-grone.de/abteilungen/tischtennis"));

            List<Link> links = new List<Link>
            {
                new Link("www.mtv-grone.de/abteilungen/tischtennis1"),

                new Link("www.mtv-grone.de/abteilungen/tischtennis2")
            };

            links.ForEach(group.Links.Add);

            group.Links[0].Profiles.Add(new Profile("Titles", "H1", "H2"));

            group.Links[0].Profiles.Add(new Profile("Paragraph and footer", "p", "footer"));

            group.Links[0].Profiles[0].Selectors[0].Results.Add(new Result("Test1"));

            group.Links[0].Profiles[0].Selectors[1].Results.Add(new Result("Test2"));

            group.Links[0].Profiles[0].Selectors[0].Results.Add(new Result("Test3"));

            group.Links[0].Profiles[0].Selectors[1].Results.Add(new Result("Test7"));

            group.Links[0].Profiles[1].Selectors[0].Results.Add(new Result("Test8"));

            group.Links[0].Profiles[1].Selectors[1].Results.Add(new Result("Test4"));

            group.Links[0].Profiles[1].Selectors[0].Results.Add(new Result("Test5"));

            group.Links[0].Profiles[1].Selectors[1].Results.Add(new Result("Test6"));

            group.Links[0].LoginSettings.Username = @"";

            group.Links[0].LoginSettings.Usernamefield = @"#loginBoxForm1 > div > div:nth-child(6) > div.InputField > input";

            group.Links[0].LoginSettings.Password = @"";

            group.Links[0].LoginSettings.Passwordfield = @"#loginBoxForm1 > div > div:nth-child(7) > div.InputField > input";

            group.Links[0].LoginSettings.LoginPage = @"";

            group.Links[0].LoginSettings.LoginPageField = @"#loginBoxForm1";

            DataContext = group.Links;

            dgLinks.ItemsSource = group.Links;

            try
            {
                SqlConnection con = new SqlConnection
                {
                    ConnectionString = "Data Source=localhost;" + "Database=SWC;" + "Integrated Security=true;"
                };

                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "SELECT  * FROM dbo.Groups",

                    Connection = con
                };

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                string test = "";

                while (dr.Read())
                {
                    test += dr[0];
                }

                cmd.ExecuteNonQuery();

                dr.Close();

                con.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;

            link.NavigateUri = new Uri(((Link)((DataGridCell)sender).DataContext).Protocoll + link.NavigateUri);

            Process.Start(link.NavigateUri.AbsoluteUri);
        }

        private void BtnAddProfiles_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow();
            profileWindow.ShowDialog();
        }

        private void CboProfiles_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow
            {
                DataContext = ((ComboBox)sender).DataContext
            };

            profileWindow.ShowDialog();
        }

        private int[] GenerateArrayHoursOfADay()
        {
            int[] hours = new int[24];

            for (int i = 0; i < 24; i++)
            {
                hours[i] = i;
            }

            return hours;
        }

        private int[] GenerateArrayMinutesOrSecondsOfAnHourOrMinute()
        {
            int[] sixties = new int[60];

            for (int i = 0; i < 60; i++)
            {
                sixties[i] = i;
            }

            return sixties;
        }

        private string[] GenerateProtocollsHttpAndHttps()
        {
            return new string[] { "http://", "https://" };
        }

        private void AddHour_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox cbo)
            {
                cbo.ItemsSource = GenerateArrayHoursOfADay();
            }
        }

        private void CboAddMinutesOrSeconds_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox cbo)
            {
                cbo.ItemsSource = GenerateArrayMinutesOrSecondsOfAnHourOrMinute();
            }
        }

        private void BtnStartInterval_Click(object sender, RoutedEventArgs e)
        {
            CrawlLinks(sender);
        }

        private static void CrawlLinks(object sender)
        {
            var links = (ObservableCollection<Link>)((Button)(sender)).DataContext;

            foreach (var link in links)
            {
                link.CrawlAsync();
            }
        }

        private void BtnAddLinks_Click(object sender, RoutedEventArgs e)
        {
            if (txtLinks.Text.Length > 0)
            {
                var links = txtLinks.Text.Split(';');

                var linksList = (ObservableCollection<Link>)((Button)sender).DataContext;

                foreach (var link in links)
                {
                    if (!string.IsNullOrEmpty(link) && linksList.Any(s => s.Equals(link)) == false)
                    {
                        linksList.Add(new Link(link));
                    }
                }
            }
            e.Handled = true;
        }

        private void CboProtocoll_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox cbo)
            {
                cbo.ItemsSource = GenerateProtocollsHttpAndHttps();
            }
        }

        private void BtnCrawl_Click(object sender, RoutedEventArgs e)
        {
            CrawlLinks(sender);
        }

        private void TxtUserLogin_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            ((Link)((Button)sender).DataContext).LoginAsync();
        }

        private void DetectPuppeteer()
        {
            var process = new Process();
            var processStartInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                //WorkingDirectory
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            process.StartInfo = processStartInfo;
            process.Start();

            var global = @"npm list -g --depth=0";
            var local = @"npm list --depth=0";

            process.StandardInput.WriteLine(global);

            
            

            try
            {
                
            }
            catch (Exception)
            {

                throw;
            }

            
        }
    }
}
