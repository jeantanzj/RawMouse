using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGClassroom.Models
{
    public class ExtObservableCollection<T>:ObservableCollection<T>
    {
        public ExtObservableCollection()
        : base() {
        }

        public ExtObservableCollection(IEnumerable<T> collection)
        : base(collection) {
        }

        public ExtObservableCollection(List<T> list)
        : base(list) {
        }

        private NotifyCollectionChangedEventHandler _event;

        private object _eventLock = new object();

        public override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                lock (_eventLock) { _event -= value; _event += value; }
            }
            remove
            {
                lock (_eventLock) { _event -= value; }
            }
        }
        

        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);

            if (item is INotifyPropertyChanged)
                (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }

        protected override void ClearItems()
        {
            for (int i = 0; i < this.Items.Count; i++)
                DeRegisterINotifyPropertyChanged(this.IndexOf(this.Items[i]));

            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            RegisterINotifyPropertyChanged(item);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            DeRegisterINotifyPropertyChanged(index);
        }

        private void RegisterINotifyPropertyChanged(T item)
        {
            if (item is INotifyPropertyChanged)
                (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }

        private void DeRegisterINotifyPropertyChanged(int index)
        {
            if (this.Items[index] is INotifyPropertyChanged)
                (this.Items[index] as INotifyPropertyChanged).PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            T item = (T)sender;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, item));
        }



    }
}
