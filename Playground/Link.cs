using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Playground
{
    public class Link : INotifyPropertyChanged
    {
        private string _address;
        private string _protocoll;
        private string _securitystatusBasedOnProtocoll;
        private string _encoding;
        private int _totalProfiles;
        private DateTime _creationDate;

        private Link()
        {

        }

        public Link(string address)
        {
            Address = address;
            Profiles = new ObservableCollection<Profile>();
            Profiles.CollectionChanged += Profiles_CollectionChanged;
            FTPInformations = new FTPInformations();
            LoginSettings = new LoginSettings();
            AutopilotSettings = new AutopilotSettings();
            CreationDate = DateTime.Now;
        }

        public Link(string address, params string[] profiles) : this(address)
        {
            foreach (var profile in profiles)
            {
                Profiles.Add(new Profile(profile));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Profiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TotalProfiles = Profiles.Count;

            foreach (var item in e.NewItems)
            {
                ((Profile)item).Link = this;
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = RemoveAndSetProtocoll(value);
                Changed(nameof(Address));
            }
        }

        public string Protocoll
        {
            get => _protocoll;
            set
            {
                _protocoll = value;
                Changed(nameof(Protocoll));
            }
        }

        public string SecuritystatusBasedOnProtocoll
        {
            get => _securitystatusBasedOnProtocoll;
            set
            {
                _securitystatusBasedOnProtocoll = value;
                Changed(nameof(SecuritystatusBasedOnProtocoll));
            }
        }

        public string Encoding
        {
            get => _encoding;
            set
            {
                _encoding = value;
                Changed(nameof(Encoding));
            }
        }

        public ObservableCollection<Profile> Profiles { get; set; }

        public int TotalProfiles
        {
            get => _totalProfiles;
            set
            {
                _totalProfiles = value;
                Changed(nameof(TotalProfiles));
            }
        }

        public Group Group { get; set; }

        public AutopilotSettings AutopilotSettings { get; set; }

        public FTPInformations FTPInformations { get; set; }

        public LoginSettings LoginSettings { get; set; }

        public DateTime CreationDate
        {
            get => _creationDate;
            set =>_creationDate = value;
        }

        private IDocument Page { get; set; }

        private string RemoveAndSetProtocoll(string address)
        {
            //bool result = Uri.TryCreate(address, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;

            //if (result)
            //{
            //    address = "http://" + address;
            //}

            Regex re = new Regex("https://");

            Match m = re.Match(address);

            if (m.Success)
            {
                address = re.Replace(address, "");

                Protocoll = "https://";
            }
            else
            {
                re = new Regex("http://");

                address = re.Replace(address, "");

                Protocoll = "http://";
            }

            return address;
        }

        public async Task CrawlAsync()
        {
            Page = await ScrapPageAsync(Address);

            AutopilotSettings.Crawl();

            foreach (var profile in Profiles)
            {
                foreach (var selector in profile.Selectors)
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Factory.StartNew(() =>
                    {
                        ScrapFromPage(selector, Page);
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }

        private async Task<IDocument> ScrapPageAsync(string address)
        {
            var config = Configuration.Default.WithDefaultLoader();

            var document = await BrowsingContext.New(config).OpenAsync(address);

            Encoding = document.CharacterSet;

            return document;
        }

        private void ScrapFromPage(Selector selector, IDocument document)
        {
            var results = document.QuerySelectorAll(selector.CSSSelector).Select(e => e.TextContent);

            foreach (var result in results)
            {
                selector.Results.Add(new Result(result.Trim()));
            }
        }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as string);
        }

        public bool Equals(string address)
        {
            return address != null &&
                   Address == address;
        }

        public override int GetHashCode()
        {
            return -1984154133 + EqualityComparer<string>.Default.GetHashCode(Address);
        }

        public async Task<bool> LoginAsync()
        {
            IDocument page = Page;

            if(!string.IsNullOrEmpty(LoginSettings.LoginPage))
            {
                page = await ScrapPageAsync(LoginSettings.LoginPage);
            }

            return await LoginSettings.LoginAsync(page);
        }
    }
}