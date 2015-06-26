namespace NativeCode.Mobile.AppCompat.Renderers.Renderers
{
    using System.Collections.Generic;
    using System.Linq;

    using Android.Support.Design.Widget;
    using Android.Views;

    using NativeCode.Mobile.AppCompat.Controls;
    using NativeCode.Mobile.AppCompat.Extensions;
    using NativeCode.Mobile.AppCompat.Renderers.Extensions;

    using Xamarin.Forms.Platform.Android;

    public class NavigationLayoutRenderer : ViewRenderer<NavigationLayout, NavigationView>, NavigationView.IOnNavigationItemSelectedListener
    {
        private readonly Dictionary<IMenuItem, NavigationLayoutMenu> mappings = new Dictionary<IMenuItem, NavigationLayoutMenu>();

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            try
            {
                this.mappings[menuItem].ExecuteCommand();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.mappings.Any())
            {
                foreach (var kvp in this.mappings)
                {
                    kvp.Key.Dispose();
                }

                this.mappings.Clear();
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationLayout> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var context = this.Context.GetAppCompatThemedContext();
                var control = new NavigationView(context);
                control.SetFitsSystemWindows(true);
                control.SetNavigationItemSelectedListener(this);

                this.SetNativeControl(control);

                this.UpdateBackgroundColor();
                this.UpdateMenuItems();
                this.UpdateHeaderView();
            }
        }

        private void UpdateHeaderView()
        {
            if (this.Element.HeaderView != null)
            {
                // TODO: It's adding it, but it never shows up in the XML in monitor.
                this.Control.AddHeaderView(this.Element.HeaderView.GetNativeView());
            }
        }

        private void UpdateMenuItems()
        {
            this.mappings.Clear();

            for (var index = 0; index < this.Element.Children.Count; index++)
            {
                // NOTE: Not sure if it's just my local machine or not, but this doesn't seem to resolve until compile-time.
                var menu = (NavigationLayoutMenu)this.Element.Children[index];
                var item = this.Control.Menu.Add(0, index, index, menu.Text);

                if (menu.Icon != null)
                {
                    item.SetIcon(menu.Icon.ToBitmapDrawable());
                }

                this.mappings.Add(item, menu);
            }
        }
    }
}