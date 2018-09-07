using UnityEngine;

namespace EOTSDK.EOTResponse
{
    [System.Serializable]
    public class VerifyRequestResponse
    {
        public bool success;
        public string msg;
        public int code;

        [SerializeField] private string verify_id;

        public string verifyID
        {
            get { return verify_id; }
        }
    }
}