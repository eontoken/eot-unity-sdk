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
            Code = EOTConstant.CODE_SUCCESS;
            Msg = EOTConstant.MSG_SUCCESS;
        }
    }
}