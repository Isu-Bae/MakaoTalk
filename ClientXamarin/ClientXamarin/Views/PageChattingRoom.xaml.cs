using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageChattingRoom : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }
        public string Title { get; set; }

        public PageChattingRoom(string title)
        {
            Title = title;
            InitializeComponent();

            Items = new ObservableCollection<string>();

            BindingContext = this;

            MessageInput.Completed += MessageInput_Completed;
        }

        private void MessageInput_Completed(object sender, EventArgs e)
        {
            Items.Add(MessageInput.Text);
            MessageInput.Text = string.Empty;
            OnPropertyChanged("Items");
        }
    }
}