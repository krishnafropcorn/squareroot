using System;
using System.Collections.Generic;

using Xamarin.Forms;
using CardReader.Interfaces;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace SquareRoot
{
    public partial class SwipeCard : ContentPage
    {
		private ICardReaderHelper _cardReaderHelper;

		public SwipeCard()
        {
            InitializeComponent();

			_cardReaderHelper = UnityProvider.Container.Resolve<ICardReaderHelper>();
        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_cardReaderHelper.StartListening ((string cardDetails) => {
				Device.BeginInvokeOnMainThread (async () => {
					if (_cardReaderHelper.CreditCardDetails == null)
						await DisplayAlert ("Invalid Swipe", "Please swipe again", "OK`");
					else
						await DisplayAlert ("Valid Swipe", "Card Details: " + cardDetails, "OK`");
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

