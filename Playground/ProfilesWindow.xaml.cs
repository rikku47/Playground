using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Playground
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private ObservableCollection<Profile> _profiles;
        private TextBox _textBoxSelectors;

        public ProfileWindow()
        {
            InitializeComponent();
        }

        private void TcProfiles_Loaded(object sender, RoutedEventArgs e)
        {
            if (((Link)DataContext)?.Profiles != null)
            {
                _profiles = ((Link)DataContext).Profiles;
                tcProfiles.ItemsSource = _profiles;
            }
        }

        private void LvSelectedSelectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var test = ((ListView)sender).SelectedItems;
        }

        private void BtnAddSelectors_Click(object sender, RoutedEventArgs e)
        {
            if(_textBoxSelectors.Text.Length > 0)
            {
                var selectors = _textBoxSelectors.Text.Split(';');

                Profile profile = (Profile)((Button)sender).DataContext;

                foreach (var selector in selectors)
                {
                    if(!string.IsNullOrEmpty(selector) && profile.Selectors.Any(s => s.Equals(selector)) == false)
                    {
                        profile.Selectors.Add(new Selector(selector));
                    }
                }
            }
        }

        private void TxtSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                _textBoxSelectors = tb;
            }

            e.Handled = true;
        }

        private void BtnAddProfiles_Click(object sender, RoutedEventArgs e)
        {
            if (txtProfiles.Text.Length > 0)
            {
                var profiles = txtProfiles.Text.Split(';');

                foreach (var profile in profiles)
                {
                    if(!string.IsNullOrEmpty(profile) && profiles.Any(s => s.Equals(profile)) == false)
                    {
                        _profiles.Add(new Profile(profile));
                    }
                }
            }

            e.Handled = true;
        }
    }
}
