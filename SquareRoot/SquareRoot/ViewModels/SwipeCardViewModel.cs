using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Text.RegularExpressions;
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

        private int _cvv;

        private int _amount;

        private string _buttonStatus;

        public int CVV
        {
            get { return _cvv; }
            set
            {
                if (value != _cvv)
                {
                    _cvv = value;
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
                    OnPropertyChanged("HasValidInput");
                }
            }
        }

        public string ButtonStatus
        {
            get
            {
                return _buttonStatus;
            }
            set
            {
                _buttonStatus = value;
                OnPropertyChanged("ButtonStatus");
            }
        }

        public ICommand ChargeCommand { get; set; }

        public bool HasValidInput 
        {
            get 
            {
                return isValidCVV(_cvv) && isValidAmoount(_amount); 
            }
        }

        private bool isValidCVV(int cvv)
        {
            return Regex.IsMatch(cvv.ToString(),"/^[0-9]{3,4}$/");
        }

        private bool isValidAmoount(int cvv)
        {
            return Regex.IsMatch(cvv.ToString(),"^[0-9]*$");

        }

        public SwipeCardViewModel()
        {
            ChargeCommand = new Command(Charge);
        }

        private void Charge()
        {
            ButtonStatus = "Charging..";

//            var paymentService = UnityProvider.Container.Resolve<IPaymentService>();
//            var result = await paymentService.ChargeCard(new CardDetails()
//                {
//                    CreditCardNumber = _cardReaderHelper.CreditCardDetails.CreditCardNumber, //"4000000000000077",
//                    CardExpiryMonth = _cardReaderHelper.CreditCardDetails.CardExpiryMonth,// 04,
//                    CardExpiryYear = _cardReaderHelper.CreditCardDetails.CardExpiryYear,//2018,
//                    CVV = TxtCCV.Text
//                }, Convert.ToInt16(TxtAmonut.Text));
//
//            if (result.IsSuccessFull)
//            {
//                await DisplayAlert("Payment Done", "YoooHooo! We just charged $1000 on your card", "OK");
//                await DisplayAlert("Just Kidding", "We are running on test account so nothing was charged :-)", "OK");
//            }
//            else
//            {
//                await DisplayAlert("Payment Failed", "Because: " + result.FailureMessage, "OK");
//            }

            ButtonStatus = "Charge";
        }

    }
}

