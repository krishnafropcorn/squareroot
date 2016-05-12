namespace Payment
{
    public class PaymentExceptionInfo
    {
        public string ErrorCode
        {
            get;
            set;
        }

        public string ErrorParam
        {
            get;
            set;
        }

        public string ErrorType
        {
            get;
            set;
        }

        public override string ToString()
        {
			return "";
                //$"[PaymentExceptionInfo: ErrorCode={ErrorCode}, ErrorParam={ErrorParam}, ErrorType={ErrorType}]";
        }
    }
}