using System;
using System.Collections.Generic;

using Xamarin.Forms;
using CardReader.Interfaces;
using System.Threading.Tasks;

namespace SquareRoot
{
    public partial class SwipeCard : ContentPage
    {
		private ICardReaderHelper _cardReaderHelper;

		public SwipeCard(ICardReaderHelper cardReaderHelper)
        {
            InitializeComponent();

			_cardReaderHelper = cardReaderHelper;
        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_cardReaderHelper.StartListening (() => {
				Device.BeginInvokeOnMainThread (async () => {
					if (_cardReaderHelper.CreditCardDetails == null)
						await DisplayAlert ("Invalid Swipe", "Please swipe again", "OK`");
					else
						ShowPaymentScreen ();
				});
			});

			ShowReaderAvailableUi ();
		}

		private async Task ShowReaderAvailableUi ()
		{
			UserInstructionLabel.Text = "Connect Reader";

			while (!_cardReaderHelper.IsReaderPlugged) {
				await DisplayAlert ("Reader Unavailable", "Connect the reader", "Retry`");
			}

			ShowSwipeCardUi ();
		}

		private void ShowSwipeCardUi ()
		{
			UserInstructionLabel.Text = "Swipe Card";
		}

		void ShowPaymentScreen ()
		{
			UserInstructionLabel.IsVisible = false;


		}
    }
}

