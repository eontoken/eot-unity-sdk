using EOTSDK.EOTDefines;
using UnityEngine;

namespace EOTSDK.EOTResponse
{
    [System.Serializable]
    public class TransferResultResponse
    {
        public bool success;
        public string msg;
        public int code;

        [SerializeField] private string transaction_id;

        public string transactionID
        {
            get { return transaction_id; }
        }

        [SerializeField] private ResultCode result_code = ResultCode.FAILED;

        public ResultCode resultCode
        {
            get { return result_code; }
        }
    }
}