using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;

namespace SquareRoot
{
    public partial class UniMag : ContentPage
    {
        public UniMag()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var answer = await DisplayAlert("Reader Unavailable", "Try connecting the reader first and turn on the app", "Retry", "Ok");

            Debug.WriteLine("Answer: " + answer);

            if (!answer)
            {
                await Navigation.PushAsync(new SwipeCard());
            }
            else
            {
                
            }
        }
    }
}

