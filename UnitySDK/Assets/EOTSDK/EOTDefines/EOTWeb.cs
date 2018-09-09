namespace EOTSDK.EOTDefines
{
    public static class EOTWeb
    {
        // Web地址
        private const string baseURL = "https://user.eontoken.io/";

        // 用户验证地址
        public const string UserVerify = baseURL + "verify/?" +
                                         "appid={0}&" +
                                         "appuser={1}&" +
                                         "verifyid={2}";

        // 转账确认地址
        public const string UserTransfer = baseURL + "transfer/?" +
                                           "appid={0}&" +
                                           "tid={1}&" +
                                           "from={2}&" +
                                           "to={3}&" +
                                           "coin={4}&" +
                                           "amount={5}";
    }
}