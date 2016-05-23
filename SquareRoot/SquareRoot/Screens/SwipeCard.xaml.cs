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
            PaymentChargeView.IsVisible = false;
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
			ShowReaderAvailableUi();
        }

        public async void OnChargeClicked(object sender,EventArgs args)
        {
            if (BtnCharge.Text == "Charge")
            {
                Device.BeginInvokeOnMainThread(async () =>
                    {
                        BtnCharge.Text = "Charging...";
                    });

                var paymentService = UnityProvider.Container.Resolve<IPaymentService>();
                var result = await paymentService.ChargeCard(new CardDetails()
                    {
                        CreditCardNumber = _cardReaderHelper.CreditCardDetails.CreditCardNumber, //"4000000000000077",
                        CardExpiryMonth = _cardReaderHelper.CreditCardDetails.CardExpiryMonth,// 04,
                        CardExpiryYear = _cardReaderHelper.CreditCardDetails.CardExpiryYear,//2018,
                        CVV = TxtCCV.Text
                    }, Convert.ToInt16(TxtAmonut.Text));

                if (result.IsSuccessFull)
                {
					await DisplayAlert("Payment Done", TxtAmonut.Text + " cents were charged on your card", "OK");
                }
                else
                {
                    await DisplayAlert("Payment Failed", "Because: " + result.FailureMessage, "OK");
                }

                BtnCharge.Text = "Charge";
            }
        }

		public async void OnRetryClicked(object sender,EventArgs args) {
			if (_cardReaderHelper.IsReaderPlugged) {
				Device.BeginInvokeOnMainThread (() => ShowSwipeCardUi ());
			}				
		}

        private async Task ShowReaderAvailableUi ()
        {
            UserInstructionLabel.IsVisible = true;
            PaymentChargeView.IsVisible = false;
            
			if (!_cardReaderHelper.IsReaderPlugged) {
				UserInstructionLabel.Text = "Please connect the reader";
                BtnRetry.IsVisible = true;
			}
			else
            	ShowSwipeCardUi ();
        }

        private void ShowSwipeCardUi ()
        {
            UserInstructionLabel.Text = "Swipe Card Now";
            BtnRetry.IsVisible = false;
            PaymentChargeView.IsVisible = false;
        }

		private async void ShowPaymentScreen ()
        {
            PaymentChargeView.IsVisible = true;
            BtnRetry.IsVisible = false;
            UserInstructionLabel.IsVisible = false;
        }
    }
}

