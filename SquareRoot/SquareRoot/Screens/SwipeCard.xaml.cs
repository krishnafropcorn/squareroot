﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using CardReader.Interfaces;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Payment;
using Common;

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

        protected override void OnAppearing()
        {
			base.OnAppearing ();
			_cardReaderHelper.StartListening ((string cardDetails) => {
				Device.BeginInvokeOnMainThread (async () => {
					if (_cardReaderHelper.CreditCardDetails == null)
						await DisplayAlert ("Invalid Swipe", "Please swipe again", "OK`");
					else
                    {
                        await DisplayAlert ("Valid Swipe", "Please share the content of this alert box with kartikbb@gmail.com: Card Details: " + cardDetails, "OK`");
                        ShowPaymentScreen();
                    }
				});
			});
        
            ShowPaymentScreen();
		}

        public async void OnChargeClicked(object sender,EventArgs args)
        {
            Device.BeginInvokeOnMainThread(async () =>
                {
                    BtnCharge.Text = "Charging...";
                    BtnCharge.IsEnabled = false;
                });

            var paymentService = UnityProvider.Container.Resolve<IPaymentService>();
            var result = await paymentService.ChargeCard(new CardDetails () {
                CreditCardNumber = "4000000000000077",
                CardExpiryMonth = 04,
                CardExpiryYear = 2018,
                CVV = TxtCCV.Text
            },Convert.ToInt16(TxtAmonut.Text));

            if (result.IsSuccessFull)
            {
                await DisplayAlert("Payment Done", "YoooHooo! We just charged $1000 on your card", "OK");
                await DisplayAlert("Just Kidding", "We are running on test account so nothing was charged :-)", "OK");
            }
            else
            {
                await DisplayAlert("Payment Failed", "Because: " + result.FailureMessage, "OK");
            }

            BtnCharge.Text = "Charge";
            BtnCharge.IsEnabled = true;
        }

		private async Task ShowReaderAvailableUi ()
        {
            UserInstructionLabel.IsVisible = true;
            _chargeView.IsVisible = false;
			UserInstructionLabel.Text = "Connect Reader";

			while (!_cardReaderHelper.IsReaderPlugged) {
				await DisplayAlert ("Reader Unavailable", "Connect the reader", "Retry");
			}

			ShowSwipeCardUi ();
		}

		private void ShowSwipeCardUi ()
		{
			UserInstructionLabel.Text = "Swipe Card Now";
		}

		void ShowPaymentScreen ()
		{
			UserInstructionLabel.IsVisible = false;
            _chargeView.IsVisible = true;
		}
    }
}

