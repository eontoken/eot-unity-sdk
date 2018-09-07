using EOTSDK.EOTDefines;
using UnityEngine;

namespace EOTSDK.EOTResponse
{
    [System.Serializable]
    public class VerifyResultResponse
    {
        public bool success;
        public string msg;
        public int code;

        [SerializeField] private string verify_id;

        public string verifyID
        {
            get { return verify_id; }
        }

        [SerializeField] private ResultCode result_code = ResultCode.FAILED;

        public ResultCode resultCode
        {
            get { return result_code; }
        }
    }
}