using System.Collections.Generic;
using EOTSDK.EOTDefines;
using UnityEngine;

namespace EOTSDK.EOTResponse
{
    [System.Serializable]
    public class Token
    {
        [SerializeField] private TokenCode coin_code;
        public string amount;

        public TokenCode tokenCode
        {
            get { return coin_code; }
        }
    }

    [System.Serializable]
    public class BalanceResponse
    {
        public bool success;
        public string msg;
        public int code;
        public List<Token> balance;
    }
}