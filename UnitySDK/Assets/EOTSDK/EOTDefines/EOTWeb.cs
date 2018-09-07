namespace EOTSDK.EOTDefines
{
    public static class EOTWeb
    {
        // Web地址
        private const string baseURL = "https://user.eontoken.io/";

        // 用户验证地址
        public const string UserVerify = baseURL + "verify/?" +
                                         "app_id={0}&" +
                                         "app_user={1}&" +
                                         "verify_id={2}";

        // 转账确认地址
        public const string UserTransfer = baseURL + "transfer/?" +
                                           "app_id={0}&" +
                                           "transfer_transaction_id={1}&" +
                                           "transfer_from={2}&" +
                                           "transfer_to={3}&" +
                                           "coin_code={4}&" +
                                           "coin_amount={5}";
    }
}