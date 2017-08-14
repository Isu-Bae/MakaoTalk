using MakaoTalk.Services.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageFriendsList : ContentPage
    {
        public PageFriendsList()
        {
			InitializeComponent ();
            BindingContext = new PageFriendsListViewModel();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            await DisplayAlert("Selected", e.SelectedItem.ToString(), "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }



    class PageFriendsListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Grouping<string, Item>> ItemsGrouped { get; }

        public PageFriendsListViewModel()
        {
            LoadData();

            var sorted = from item in Items
                         orderby item.Text
                         group item by item.Text[0].ToString() into itemGroup
                         select new Grouping<string, Item>(itemGroup.Key, itemGroup);

            ItemsGrouped = new ObservableCollection<Grouping<string, Item>>(sorted);

            RefreshDataCommand = new Command(
                async () => await RefreshData());
        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            //Load Data Here
            LoadData();
            await Task.Delay(1000);

            IsBusy = false;
        }

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
                ((Command)RefreshDataCommand).ChangeCanExecute();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public class Item
        {
            public string Text { get; set; }
            public string Detail { get; set; }
            public ImageCell Image { get; set; }

            public override string ToString() => Text;
        }

        public class Grouping<K, T> : ObservableCollection<T>
        {
            public K Key { get; private set; }

            public Grouping(K key, IEnumerable<T> items)
            {
                Key = key;
                foreach (var item in items)
                    this.Items.Add(item);
            }
        }

        #region Private Functions
        private void LoadData()
        {
            var list = new ObservableCollection<Item>();

            IMessageService service = new MessageService();

            foreach(var item in service.GetFriends("01071270202"))
            {
                list.Add(new Item { Text = item.UserName, Detail = item.FriendID });
            }

            Items = list;
        }

        #endregion
    }
}
