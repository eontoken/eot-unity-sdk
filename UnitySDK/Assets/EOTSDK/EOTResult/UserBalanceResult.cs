using System.Collections.Generic;
using EOTSDK.EOTDefines;
using EOTSDK.EOTResponse;

namespace EOTSDK.EOTResult
{
    public class UserBalanceResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public List<Token> Balance { get; set; }

        public UserBalanceResult()
        {
            Code = EOTConstant.CODE_NETWORK_ERROR;
            Msg = EOTConstant.MSG_NETWORK_ERROR;
            Balance = null;
        }
    }
}