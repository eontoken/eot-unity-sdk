using UnityEngine;

namespace EOTSDK.EOTResponse
{
    [System.Serializable]
    public class TransferRequestResponse
    {
        public bool success;
        public string msg;
        public int code;

        [SerializeField] private string transaction_id;

        public string transactionID
        {
            get { return transaction_id; }
        }
    }
}