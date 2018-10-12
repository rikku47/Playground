using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Playground
{
    public class Group : INotifyPropertyChanged
    {
        private Group()
        {

        }

        public Group(string name)
        {
            Name = name;
            Links = new ObservableCollection<Link>();
            Links.CollectionChanged += Links_CollectionChanged;
            TotalLinks = Links.Count;
            Settings = new AutopilotSettings();
            CreationDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Links_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TotalLinks = Links.Count;

            foreach (var item in e.NewItems)
            {
                ((Link)item).Group = this;
            }
        }

        public string Name { get; set; }

        public ObservableCollection<Link> Links { get; set; }

        public int TotalLinks { get; set; }

        public AutopilotSettings Settings { get; set; }

        public DateTime CreationDate { get; set; }

        public void Crawl()
        {
            foreach (var link in Links)
            {
                link.CrawlAsync();
            }
        }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}