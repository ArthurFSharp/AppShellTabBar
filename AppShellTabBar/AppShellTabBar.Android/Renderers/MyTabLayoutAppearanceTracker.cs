using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Text.Style;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppShellTabBar.Droid.Renderers
{
    public class MyTabLayoutAppearanceTracker : ShellBottomNavViewAppearanceTracker
    {
        public MyTabLayoutAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem) { }

        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);

            var menu = bottomView.Menu;
            for (int i = 0; i < bottomView.Menu.Size(); i++)
            {
                var menuItem = menu.GetItem(i);
                var title = menuItem.TitleFormatted;
                Typeface typeface = Typeface.CreateFromAsset(MainActivity.Instance.Assets, "Chocolate Covered Raindrops.ttf");
                var sb = new SpannableStringBuilder(title);

                sb.SetSpan(new CustomTypefaceSpan("", typeface), 0, sb.Length(), SpanTypes.InclusiveInclusive);
                menuItem.SetTitle(sb);
            }
        }
    }

    class CustomTypefaceSpan : TypefaceSpan
    {
        private Typeface newType;

        public CustomTypefaceSpan(String family, Typeface type) : base(family)
        {
            newType = type;
        }
        public override void UpdateDrawState(TextPaint ds)
        {
            applyCustomTypeFace(ds, newType);

        }
        public override void UpdateMeasureState(TextPaint paint)
        {
            applyCustomTypeFace(paint, newType);
        }
        private static void applyCustomTypeFace(Paint paint, Typeface tf)
        {
            TypefaceStyle oldStyle;
            Typeface old = paint.Typeface;
            if (old == null)
            {
                oldStyle = 0;
            }
            else
            {
                oldStyle = old.Style;
            }

            TypefaceStyle fake = oldStyle & ~tf.Style;
            if ((fake & TypefaceStyle.Bold) != 0)
            {
                paint.FakeBoldText = true;
            }

            if ((fake & TypefaceStyle.Italic) != 0)
            {
                paint.TextSkewX = -0.25f;
            }

            paint.SetTypeface(tf);
        }
    }
}