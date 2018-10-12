using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Playground
{
    public class Profile : INotifyPropertyChanged
    {
        private Profile()
        {

        }

        public Profile(string name)
        {
            Name = name;
            Selectors = new ObservableCollection<Selector>();
            Selectors.CollectionChanged += Selectors_CollectionChanged;
            CreationDate = DateTime.Now;
        }

        public Profile(string name, params string[] selectors) : this(name)
        {
            foreach (var selector in selectors)
            {
                Selectors.Add(new Selector(selector));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Selectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TotalSelectors = Selectors.Count;

            foreach (var item in e.NewItems)
            {
                ((Selector)item).Profile = this;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as string);
        }

        public bool Equals(string profile)
        {
            return profile != null &&
                   Name == profile;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public string Name { get; set; }

        public ObservableCollection<Selector> Selectors { get; set; }

        public Link Link { get; set; }

        public int TotalSelectors { get; set; }

        public AutopilotSettings Settings { get; set; }

        public DateTime CreationDate { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}