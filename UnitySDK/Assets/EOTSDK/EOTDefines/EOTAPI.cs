namespace EOTSDK.EOTDefines
{
    public static class EOTAPI
    {
        // 接口地址
        private const string baseURL = "https://dev.eontoken.io/api/v1/app/user/";

        // 获取用户Balance接口
        public const string Balance = baseURL + "balance/";

        // 获取用户验证ID接口
        public const string VerifyRequest = baseURL + "verify/request/";

        // 用户验证结果查询接口
        public const string VerifyResult = baseURL + "verify/result/";

        // 获取用户转账事务ID接口
        public const string TransferRequest = baseURL + "transfer/request/";

        // 用户转账结果查询接口
        public const string TransferResult = baseURL + "transfer/result/";
    }
}