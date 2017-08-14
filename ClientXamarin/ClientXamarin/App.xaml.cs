using ClientXamarin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ClientXamarin
{
    
	public partial class App : Application
	{
        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            var mainTabbedPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new PageFriendsList())
                    {
                        Title = "친구",
                        //Icon = Device.OnPlatform("tab_feed.png",null,null),
                    },
                    new NavigationPage(new PageChattingGroupList())
                    {
                        Title = "대화방",
                        //Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                    new NavigationPage(new PageSettings())
                    {
                        Title = "설정",
                        //Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
            };
            

            Current.MainPage = mainTabbedPage;
        }

        
	}
}
