using EOTSDK.EOTDefines;

namespace EOTSDK.EOTResult
{
    public class UserTransferResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public string TransactionID { get; set; }

        public UserTransferResult()
        {
            Code = EOTConstant.CODE_NETWORK_ERROR;
            Msg = EOTConstant.MSG_NETWORK_ERROR;
        }
    }
}