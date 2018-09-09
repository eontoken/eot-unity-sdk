using System;
using UnityEngine;

namespace EOTSDK.Examples
{
    [Serializable]
    public class LoginResponse
    {
        public bool success;
        public string msg;
        public int code;
        public string username;

        [SerializeField] private bool is_verified;

        public bool isVerified
        {
            get { return is_verified; }
        }
    }
}