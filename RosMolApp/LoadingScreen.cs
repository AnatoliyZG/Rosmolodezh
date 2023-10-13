using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RosMolApp
{
    internal class LoadingScreen : ContentView
    {
        public static readonly BindableProperty LoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(LoadingScreen), false);

        public static readonly BindableProperty AvailProperty = BindableProperty.Create(nameof(IsAvail), typeof(bool), typeof(LoadingScreen), true);

        public bool IsLoading
        {
            get => (bool)GetValue(LoadingProperty);
            set => SetValue(LoadingProperty, value);
        }

        public bool IsAvail
        {
            get => (bool)GetValue(AvailProperty);
            set => SetValue(AvailProperty, value);
        }

        public void ActiveLoading(bool active)
        {
            IsLoading = active;
            IsAvail = !active;
        }

        private Grid screenGreed;

        public LoadingScreen()
        {

            var box = new BoxView()
            {
                BackgroundColor = new Color(0, 0, 0, 40),
                ZIndex = 10,
                BindingContext = this,
            };
            var indicator = new ActivityIndicator()
            {
                ZIndex = 10,
                WidthRequest = 50,
                HeightRequest = 50,
                BindingContext = this,
            };
            box.SetBinding(IsVisibleProperty, "IsLoading");
            indicator.SetBinding(IsVisibleProperty, "IsLoading");
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");

            screenGreed = new Grid()
            {
                box,
                indicator,
            };
        }

        protected override void OnChildAdded(Element child)
        {
            if (child == screenGreed) return;

            base.OnChildAdded(child);

            if (child is View view)
            {
                screenGreed.Add(view);

                Content = screenGreed;
            }
        }
    }
}
