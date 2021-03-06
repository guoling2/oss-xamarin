﻿namespace Demo
{
    using Demo.Views;

    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            ShowChooser();
        }

        internal static MasterDetailPage MasterDetail { get; private set; }

        internal static INavigation Navigation { get; private set; }

        public static INavigation Navigate()
        {
            return Navigation;
        }

        public static void ShowChooser()
        {
            Current.MainPage = new ChooserView();
        }

        public static void ShowMasterDetailPatternOne()
        {
            var master = CreateMasterDetailPage(new MenuView(), new MainView());
            Current.MainPage = CreateNavigationPage(master);
        }

        public static void ShowMasterDetailPatternTwo()
        {
            var master = CreateMasterDetailPage(new MenuView(), CreateNavigationPage(new MainView()));
            Current.MainPage = master;
        }

        public static void ShowNavigation()
        {
            Current.MainPage = CreateNavigationPage(new NestNavigationView(), false);
        }

        public static void ShowTabbed()
        {
            var tabbed = new TabbedPage();
            tabbed.Children.Add(new LoremIpsumView { Title = "Page 1" });
            tabbed.Children.Add(new LoremIpsumView { Title = "Page 2" });
            tabbed.Children.Add(new LoremIpsumView { Title = "Page 3" });

            Current.MainPage = CreateNavigationPage(CreateMasterDetailPage(new MenuView(), tabbed));
        }

        private static MasterDetailPage CreateMasterDetailPage(Page master, Page detail)
        {
            return MasterDetail = new MasterDetailPage { Detail = detail, Master = master, MasterBehavior = GetMasterBehavior(), Title = "AppCompat Demo" };
        }

        private static NavigationPage CreateNavigationPage(Page page, bool handleEvents = true)
        {
            var navigation = new NavigationPage(page);

            if (handleEvents)
            {
                navigation.Popped += (sender, args) => HideMenu();
                navigation.PoppedToRoot += (sender, args) => HideMenu();
                navigation.Pushed += (sender, args) => HideMenu();
            }

            Navigation = navigation.Navigation;

            return navigation;
        }

        private static MasterBehavior GetMasterBehavior()
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return MasterBehavior.Popover;
            }

            return MasterBehavior.SplitOnLandscape;
        }

        private static void HideMenu()
        {
            if (MasterDetail.MasterBehavior == MasterBehavior.Popover)
            {
                MasterDetail.IsPresented = false;
            }
        }
    }
}