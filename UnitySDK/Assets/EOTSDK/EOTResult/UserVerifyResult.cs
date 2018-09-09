using EOTSDK.EOTDefines;

namespace EOTSDK.EOTResult
{
    public class UserVerifyResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }

        public UserVerifyResult()
        {
            Code = EOTConstant.CODE_NETWORK_ERROR;
            Msg = EOTConstant.MSG_NETWORK_ERROR;
        }
    }
}