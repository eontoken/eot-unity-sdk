using System;

namespace EOTSDK.Examples
{
    [Serializable]
    public class RegisterResponse
    {
        public bool success;
        public string msg;
        public int code;
        public string username;
    }
}