using System;
using System.Collections;
using System.Globalization;
using EOTSDK.EOTDefines;
using EOTSDK.EOTResponse;
using EOTSDK.EOTResult;
using SimpleHTTP;
using UnityEngine;

namespace EOTSDK
{
    public class EOTClient
    {
        private static EOTClient _instance;

        private string _appID;
        private string _appKey;

        private BalanceResponse _balanceResponse;
        private VerifyRequestResponse _verifyRequestResponse;
        private VerifyResultResponse _verifyResultResponse;
        private TransferRequestResponse _transferRequestResponse;
        private TransferResultResponse _transferResultResponse;

        public UserBalanceResult UserBalanceResult { get; private set; }
        public UserTransferResult UserTransferResult { get; private set; }
        public UserVerifyResult UserVerifyResult { get; private set; }

        private EOTClient(string appID, string appKey)
        {
            _appID = appID;
            _appKey = appKey;

            UserVerifyResult = new UserVerifyResult();
            UserTransferResult = new UserTransferResult();
            UserBalanceResult = new UserBalanceResult();
        }

        public static EOTClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Instance is not created, use Init() first.");
                }

                return _instance;
            }
        }

        public static void Init(string appID, string appKey)
        {
            if (_instance != null)
            {
                throw new Exception("Instance has been created.");
            }

            _instance = new EOTClient(appID, appKey);
        }

        // 验证用户
        public IEnumerator VerifyUser(string userName)
        {
            yield return GetUserVerifyID(userName);

            if (_verifyRequestResponse == null)
            {
                UserVerifyResult.Code = EOTConstant.CODE_NETWORK_ERROR;
                UserVerifyResult.Msg = EOTConstant.MSG_NETWORK_ERROR;

                Debug.LogError("VerifyRequestResponse is null!!!");

                yield break;
            }

            if (!_verifyRequestResponse.success)
            {
                UserVerifyResult.Code = _verifyRequestResponse.code;
                UserVerifyResult.Msg = _verifyRequestResponse.msg;

                Debug.LogError("Get user verify id failed!!!");
                Debug.LogError("Code: " + _verifyRequestResponse.code + " Msg: " + _verifyRequestResponse.msg);

                yield break;
            }

            // 打开浏览器，让用户自行验证
            string webURl = string.Format(EOTWeb.UserVerify, _appID, userName, _verifyRequestResponse.verifyID);
            Application.OpenURL(webURl);
            yield return new WaitForSeconds(6.0f);

            _verifyResultResponse = null;

            while (true)
            {
                yield return GetUserVerifyResult(userName, _verifyRequestResponse.verifyID);

                if (_verifyResultResponse == null)
                {
                    UserVerifyResult.Code = EOTConstant.CODE_NETWORK_ERROR;
                    UserVerifyResult.Msg = EOTConstant.MSG_NETWORK_ERROR;
                    break;
                }

                if (!_verifyResultResponse.success)
                {
                    UserVerifyResult.Code = _verifyResultResponse.code;
                    UserVerifyResult.Msg = _verifyResultResponse.msg;
                    break;
                }

                if (_verifyResultResponse.resultCode == ResultCode.SUCCESSFUL)
                {
                    UserVerifyResult.Code = EOTConstant.CODE_SUCCESS;
                    UserVerifyResult.Msg = EOTConstant.MSG_SUCCESS;
                    break;
                }

                yield return new WaitForSeconds(1);
            }
        }

        // 确认交易
        public IEnumerator Transfer(string transferFrom, string transferTo, TokenCode tokenCode, string amount)
        {
            yield return GetUserTransferID(transferFrom, transferTo, tokenCode, amount);

            if (_transferRequestResponse == null)
            {
                UserTransferResult.Code = EOTConstant.CODE_NETWORK_ERROR;
                UserTransferResult.Msg = EOTConstant.MSG_NETWORK_ERROR;

                Debug.LogError("TransferRequestResponse is null!!!");

                yield break;
            }

            if (!_transferRequestResponse.success)
            {
                UserTransferResult.Code = _transferRequestResponse.code;
                UserTransferResult.Msg = _transferRequestResponse.msg;

                Debug.LogError("Get user transfer id failed!!!");
                Debug.LogError("Code: " + _transferRequestResponse.code + " Msg: " + _transferRequestResponse.msg);

                yield break;
            }

            // 打开浏览器，让用户自行验证
            string webURl = string.Format(EOTWeb.UserTransfer, _appID, _transferRequestResponse.transactionID,
                transferFrom, transferTo, (int)tokenCode, amount);
            Application.OpenURL(webURl);
            yield return new WaitForSeconds(6.0f);

            _transferResultResponse = null;

            while (true)
            {
                yield return GetUserTransferResult(transferFrom, transferTo, _transferRequestResponse.transactionID);

                if (_transferResultResponse == null)
                {
                    UserTransferResult.Code = EOTConstant.CODE_NETWORK_ERROR;
                    UserTransferResult.Msg = EOTConstant.MSG_NETWORK_ERROR;
                    break;
                }

                if (!_transferResultResponse.success)
                {
                    UserTransferResult.Code = _transferResultResponse.code;
                    UserTransferResult.Msg = _transferResultResponse.msg;
                    break;
                }

                if (_transferResultResponse.resultCode == ResultCode.SUCCESSFUL)
                {
                    UserTransferResult.Code = EOTConstant.CODE_SUCCESS;
                    UserTransferResult.Msg = EOTConstant.MSG_SUCCESS;
                    UserTransferResult.TransactionID = _transferRequestResponse.transactionID;
                    break;
                }

                yield return new WaitForSeconds(1);
            }
        }

        // 获取用户Balance
        public IEnumerator GetUserBalance(string userName)
        {
            FormData formData = new FormData()
                .AddField("app_id", _appID)
                .AddField("app_key", _appKey)
                .AddField("app_user", userName);

            Request request = new Request(EOTAPI.Balance).Post(RequestBody.From(formData));
            Client http = new Client();

            yield return http.Send(request);

            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                Debug.Log("BalanceResponse: " + resp.Body());

                _balanceResponse = JsonUtility.FromJson<BalanceResponse>(resp.Body());

                if (_balanceResponse.success)
                {
                    UserBalanceResult.Code = EOTConstant.CODE_SUCCESS;
                    UserBalanceResult.Msg = EOTConstant.MSG_SUCCESS;
                    UserBalanceResult.Balance = _balanceResponse.balance;
                }
                else
                {
                    UserBalanceResult.Code = _balanceResponse.code;
                    UserBalanceResult.Msg = _balanceResponse.msg;
                    UserBalanceResult.Balance = null;
                }
            }
            else
            {
                UserBalanceResult.Code = EOTConstant.CODE_NETWORK_ERROR;
                UserBalanceResult.Msg = EOTConstant.MSG_NETWORK_ERROR;

                Debug.LogError("NetWorkError: " + http.Error());
            }
        }

        // 获取用户验证凭证
        private IEnumerator GetUserVerifyID(string userName)
        {
            FormData formData = new FormData()
                .AddField("app_id", _appID)
                .AddField("app_key", _appKey)
                .AddField("app_user", userName);

            Request request = new Request(EOTAPI.VerifyRequest).Post(RequestBody.From(formData));
            Client http = new Client();

            yield return http.Send(request);

            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                Debug.Log("VerifyRequestResponse: " + resp.Body());

                _verifyRequestResponse = JsonUtility.FromJson<VerifyRequestResponse>(resp.Body());
            }
            else
            {
                Debug.LogError("NetWorkError: " + http.Error());
            }
        }

        // 查询用户验证结果
        private IEnumerator GetUserVerifyResult(string userName, string verifyID)
        {
            FormData formData = new FormData()
                .AddField("app_id", _appID)
                .AddField("app_key", _appKey)
                .AddField("app_user", userName)
                .AddField("verify_id", verifyID);

            Request request = new Request(EOTAPI.VerifyResult).Post(RequestBody.From(formData));
            Client http = new Client();

            yield return http.Send(request);

            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                Debug.Log("VerifyResultResponse: " + resp.Body());

                _verifyResultResponse = JsonUtility.FromJson<VerifyResultResponse>(resp.Body());
            }
            else
            {
                Debug.LogError("NetWorkError: " + http.Error());
            }
        }

        // 获取用户转账事务ID
        private IEnumerator GetUserTransferID(string transferFrom, string transferTo,
            TokenCode tokenCode, string amount)
        {
            FormData formData = new FormData()
                .AddField("app_id", _appID)
                .AddField("app_key", _appKey)
                .AddField("transfer_from", transferFrom)
                .AddField("transfer_to", transferTo)
                .AddField("coin_code", ((int) tokenCode).ToString())
                .AddField("coin_amount", amount);

            Request request = new Request(EOTAPI.TransferRequest).Post(RequestBody.From(formData));
            Client http = new Client();

            yield return http.Send(request);

            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                Debug.Log("TransferRequestResponse: " + resp.Body());

                _transferRequestResponse = JsonUtility.FromJson<TransferRequestResponse>(resp.Body());
            }
            else
            {
                Debug.LogError("NetWorkError: " + http.Error());
            }
        }

        // 查询用户转账结果
        private IEnumerator GetUserTransferResult(string transferFrom, string transferTo, string transactionID)
        {
            FormData formData = new FormData()
                .AddField("app_id", _appID)
                .AddField("app_key", _appKey)
                .AddField("transfer_from", transferFrom)
                .AddField("transfer_to", transferTo)
                .AddField("transaction_id", transactionID);

            Request request = new Request(EOTAPI.TransferResult).Post(RequestBody.From(formData));
            Client http = new Client();

            yield return http.Send(request);

            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                Debug.Log("TransferResultResponse: " + resp.Body());

                _transferResultResponse = JsonUtility.FromJson<TransferResultResponse>(resp.Body());
            }
            else
            {
                Debug.LogError("NetWorkError: " + http.Error());
            }
        }
    }
}