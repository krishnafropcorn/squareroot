using System;
using Xamarin.Forms;
using CardReader.Interfaces;
using Microsoft.Practices.Unity;
using Payment;

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
			base.OnAppearing();
			_cardReaderHelper.StartListening(() =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					if (BtnRetry.IsVisible)
					{
						if (_cardReaderHelper.IsReaderPlugged)
							ShowSwipeCardUi();
					}
					else {
						if (_cardReaderHelper.CreditCardDetails != null)
							ShowPaymentScreen();
					}
				});
			});

			if (!_cardReaderHelper.IsReaderPlugged) 
				ShowReaderAvailableUi(); 
			else 
				ShowSwipeCardUi();
		}

		public async void OnChargeClicked(object sender,EventArgs args)
        {
            if (BtnCharge.Text == "Charge")
            {
                Device.BeginInvokeOnMainThread(() =>
                    {
                        BtnCharge.Text = "Charging...";
                    });

                var paymentService = UnityProvider.Container.Resolve<IPaymentService>();

				_cardReaderHelper.CreditCardDetails.CVV = TxtCCV.Text;

                var result = await paymentService.ChargeCard(_cardReaderHelper.CreditCardDetails, Convert.ToInt16(TxtAmonut.Text));

                if (result.IsSuccessFull)
                {
                    await DisplayAlert("Payment Done", TxtAmonut.Text  + " dollar/s was charged on your card", "OK");
                }
                else
                {
                    await DisplayAlert("Payment Failed", "Because: " + result.FailureMessage, "OK");
                }

                TxtCCV.Text = "";
                TxtAmonut.Text = "";

                BtnCharge.Text = "Charge";
            }
        }

		public void OnRetryClicked(object sender,EventArgs args) {
			if (_cardReaderHelper.IsReaderPlugged) {
				Device.BeginInvokeOnMainThread (() => ShowSwipeCardUi ());
			}				
		}

        private void ShowReaderAvailableUi ()
        {
            UserInstructionLabel.IsVisible = true;
			UserInstructionLabel.Text = "Please connect the reader";
			BtnRetry.IsVisible = true;
			PaymentChargeView.IsVisible = false;
        }

        private void ShowSwipeCardUi ()
        {
            UserInstructionLabel.IsVisible = true;
			UserInstructionLabel.Text = "Swipe Card Now";
            BtnRetry.IsVisible = false;
            PaymentChargeView.IsVisible = false;
        }

		private void ShowPaymentScreen ()
        {
			UserInstructionLabel.IsVisible = false;
			UserInstructionLabel.Text = "";
			BtnRetry.IsVisible = false;
			PaymentChargeView.IsVisible = true;
        }
    }
}

