﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SquareRoot
{
    public class App : Application
    {
        public App()
        {
			Current = this;
			MainPage = new SwipeCard ();
        }

		public static new App Current
		{
			get;
			private set;
		}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
