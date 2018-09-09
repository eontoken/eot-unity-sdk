using System;

namespace EOTSDK.Examples
{
    [Serializable]
    public class VerifyResponse
    {
        public bool success;
        public string msg;
        public int code;
    }
}