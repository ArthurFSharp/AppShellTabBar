using AppShellTabBar;
using AppShellTabBar.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AppShell), typeof(MyShellRenderer))]
namespace AppShellTabBar.iOS.Renderers
{
    public class MyShellRenderer : ShellRenderer
    {
        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker() =>
            new CustomTabbarAppearance();
    }

    public class CustomTabbarAppearance : IShellTabBarAppearanceTracker
    {
        UIColor _defaultBarTint;
        UIColor _defaultTint;
        UIColor _defaultUnselectedTint;

        public void Dispose() { }

        public void ResetAppearance(UITabBarController controller)
        {
            if (_defaultTint == null)
                return;

            var tabBar = controller.TabBar;
            tabBar.BarTintColor = _defaultBarTint;
            tabBar.TintColor = _defaultTint;
            tabBar.UnselectedItemTintColor = _defaultUnselectedTint;
        }

        public void SetAppearance(UITabBarController controller, ShellAppearance appearance)
        {
            IShellAppearanceElement appearanceElement = appearance;
            var backgroundColor = appearanceElement.EffectiveTabBarBackgroundColor;
            var unselectedColor = appearanceElement.EffectiveTabBarUnselectedColor;
            var titleColor = appearanceElement.EffectiveTabBarTitleColor;

            var tabBar = controller.TabBar;
            bool operatingSystemHasUnselectedTint = UIDevice.CurrentDevice.CheckSystemVersion(10, 0);

            if (_defaultTint == null)
            {
                _defaultBarTint = tabBar.BarTintColor;
                _defaultTint = tabBar.TintColor;
                if (operatingSystemHasUnselectedTint)
                {
                    _defaultUnselectedTint = tabBar.UnselectedItemTintColor;
                }
            }

            if (!backgroundColor.IsDefault)
                tabBar.BarTintColor = backgroundColor.ToUIColor();

            if (!titleColor.IsDefault)
                tabBar.TintColor = titleColor.ToUIColor();

            if (operatingSystemHasUnselectedTint && !unselectedColor.IsDefault)
            {
                tabBar.UnselectedItemTintColor = unselectedColor.ToUIColor();
            }
        }

        public void UpdateLayout(UITabBarController controller)
        {
            var myTabBar = controller.TabBar;

            foreach (var barItem in myTabBar.Items)
            {
                UITextAttributes normalTextAttributes = new UITextAttributes();
                normalTextAttributes.Font = UIFont.FromName("Roboto-Regular", 16.0F); // unselected
                barItem.SetTitleTextAttributes(normalTextAttributes, UIControlState.Normal);

                UITextAttributes boldTextAttributes = new UITextAttributes();
                boldTextAttributes.Font = UIFont.FromName("Roboto-Medium", 16.0F); // selected
                barItem.SetTitleTextAttributes(boldTextAttributes, UIControlState.Selected);
            }
        }
    }
}