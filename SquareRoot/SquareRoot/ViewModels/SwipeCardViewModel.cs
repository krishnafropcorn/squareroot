using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.Practices.Unity;
using Payment;
using Common;

namespace SquareRoot
{
    public class SwipeCardViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ICommand ChargeCommand { get; set; }

        public bool HasValidInput 
        {
            get 
            {
                return !string.IsNullOrEmpty(_cvv.ToString()) && !string.IsNullOrEmpty(_amount.ToString()); 
            }
        }

        private int _cvv;

        private int _amount;

        public int CVV
        {
            get { return _cvv; }
            set
            {
                if (value != _cvv)
                {
                    _cvv = value;
                    OnPropertyChanged("CVV");
                    OnPropertyChanged("HasValidInput");
                }
            }
        }

        public int Amount
        {
            get { return _amount; }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("HasValidInput");
                }
            }
        }

        public SwipeCardViewModel()
        {
            CVV = null;

            Amount = null;

            ChargeCommand = new Command(Charge);

        }

        private async void Charge()
        {
//                var paymentService = UnityProvider.Container.Resolve<IPaymentService>();
//                var result = await paymentService.ChargeCard(new CardDetails()
//                    {
//                        CreditCardNumber = "4000000000000077",
//                        CardExpiryMonth = 04,
//                        CardExpiryYear = 2018,
//                        CVV = TxtCCV.Text
//                    }, Convert.ToInt16(TxtAmonut.Text));
//
//                if (result.IsSuccessFull)
//                {
//                    await DisplayAlert("Payment Done", "YoooHooo! We just charged $1000 on your card", "OK");
//                    await DisplayAlert("Just Kidding", "We are running on test account so nothing was charged :-)", "OK");
//                }
//                else
//                {
//                    await DisplayAlert("Payment Failed", "Because: " + result.FailureMessage, "OK");
//                }
        }

    }
}

