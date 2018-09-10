using System;
using System.Collections;
using System.Collections.Generic;
using EOTSDK.EOTDefines;
using EOTSDK.EOTResponse;
using EOTSDK.EOTResult;
using SimpleHTTP;
using UnityEngine;
using UnityEngine.UI;

namespace EOTSDK.Examples
{
    public class DemoManager : MonoBehaviour
    {
        private string _username;
        private string _password;

        public GameObject StartPanel;
        public GameObject LoginPanel;
        public GameObject RegisterPanel;
        public GameObject HomePanel;
        public GameObject VerifyPanel;
        public GameObject BalancePanel;
        public GameObject TransferPanel;

        public GameObject ProcessingAnimPanel;
        public GameObject ErrorMsgPanel;
        public Text ErrorMsg;
        public GameObject SuccessMsgPanel;
        public Text SuccessMsg;

        public InputField AppLoginUserName;
        public InputField AppLoginUserPassword;

        public InputField AppRegisterUserName;
        public InputField AppRegisterUserPassword;

        public Text HomeUserNameLabel;
        public Text BalanceUserLabel;
        public Text VerifyUserLabel;

        public Text BalanceInfo;

        public InputField TransferToUserName;
        public InputField TransferETHAmount;

        // Use this for initialization
        private void Start()
        {
            Screen.SetResolution(480, 768, false);

            EOTClient.Init(DemoUtils.DEMO_APP_ID, DemoUtils.DEMO_APP_KEY);
            ShowStartPanel();
        }

        public void OnDoLoginButtonClick()
        {
            ShowLoginPanel();
        }

        public void OnDoRegisterButtonClick()
        {
            ShowRegisterPanel();
        }

        public void OnBackButtonClick()
        {
            ShowStartPanel();
        }

        public void OnCloseErrorMsgButtonClick()
        {
            ErrorMsgPanel.SetActive(false);
        }

        public void OnCloseSuccessMsgButtonClick()
        {
            SuccessMsgPanel.SetActive(false);
        }

        private void SetUserName(string username)
        {
            VerifyUserLabel.text = username;
            BalanceUserLabel.text = username;
            HomeUserNameLabel.text = "Hi, " + username;
        }

        public void OnLoginButtonClick()
        {
            string appUserName = AppLoginUserName.text;
            string appUserPassword = AppLoginUserPassword.text;

            if (string.IsNullOrEmpty(appUserName))
            {
                ErrorMsg.text = "Please input user name!";
                ErrorMsgPanel.SetActive(true);

                throw new Exception("Please input user name!");
            }

            if (string.IsNullOrEmpty(appUserPassword))
            {
                ErrorMsg.text = "Please input user password!";
                ErrorMsgPanel.SetActive(true);

                throw new Exception("Please input user password!");
            }

            _username = appUserName;
            _password = appUserPassword;
            ProcessingAnimPanel.SetActive(true);

            StartCoroutine(LoginCoroutine(appUserName, appUserPassword));
        }

        private IEnumerator LoginCoroutine(string username, string password)
        {
            FormData formData = new FormData()
                .AddField("username", username)
                .AddField("password", password);
            Request request = new Request(DemoUtils.DEMO_APP_LOGIN_URL).Post(RequestBody.From(formData));
            Client http = new Client();
            yield return http.Send(request);

            ProcessingAnimPanel.SetActive(false);

            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                Debug.Log("LoginResponse: " + resp.Body());

                LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(resp.Body());

                if (loginResponse.success)
                {
                    SetUserName(loginResponse.username);

                    if (loginResponse.isVerified)
                    {
                        ShowHomePanel();
                    }
                    else
                    {
                        ShowVerifyPanel();
                    }
                }
                else
                {
                    ErrorMsg.text = loginResponse.msg;
                    ErrorMsgPanel.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("NetWorkError: " + http.Error());

                ErrorMsg.text = http.Error();
                ErrorMsgPanel.SetActive(true);
            }
        }

        public void OnRegisterButtonClick()
        {
            string appUserName = AppRegisterUserName.text;
            string appUserPassword = AppRegisterUserPassword.text;

            if (string.IsNullOrEmpty(appUserName))
            {
                ErrorMsg.text = "Please input user name!";
                ErrorMsgPanel.SetActive(true);

                throw new Exception("Please input user name!");
            }

            if (string.IsNullOrEmpty(appUserPassword))
            {
                ErrorMsg.text = "Please input user password!";
                ErrorMsgPanel.SetActive(true);

                throw new Exception("Please input user password!");
            }

            _username = appUserName;
            _password = appUserPassword;
            ProcessingAnimPanel.SetActive(true);

            StartCoroutine(RegisterCoroutine(appUserName, appUserPassword));
        }

        private IEnumerator RegisterCoroutine(string username, string password)
        {
            FormData formData = new FormData()
                .AddField("username", username)
                .AddField("password", password);
            Request request = new Request(DemoUtils.DEMO_APP_REGISTER_URL).Post(RequestBody.From(formData));
            Client http = new Client();
            yield return http.Send(request);

            ProcessingAnimPanel.SetActive(false);

            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                Debug.Log("RegisterResponse: " + resp.Body());

                RegisterResponse registerResponse = JsonUtility.FromJson<RegisterResponse>(resp.Body());

                if (registerResponse.success)
                {
                    SetUserName(registerResponse.username);
                    ShowVerifyPanel();
                }
                else
                {
                    ErrorMsg.text = registerResponse.msg;
                    ErrorMsgPanel.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("NetWorkError: " + http.Error());

                ErrorMsg.text = http.Error();
                ErrorMsgPanel.SetActive(true);
            }
        }

        public void OnStartVerificationButtonClick()
        {
            ProcessingAnimPanel.SetActive(true);
            StartCoroutine(VerifyCoroutine());
        }

        private IEnumerator VerifyCoroutine()
        {
            yield return EOTClient.Instance.VerifyUser(_username);
            UserVerifyResult userVerifyResult = EOTClient.Instance.UserVerifyResult;

            if (userVerifyResult.Code == EOTConstant.CODE_SUCCESS)
            {
                FormData formData = new FormData()
                    .AddField("username", _username)
                    .AddField("password", _password);

                Request request = new Request(DemoUtils.DEMO_APP_VERIFY_URL).Post(RequestBody.From(formData));
                Client http = new Client();

                yield return http.Send(request);

                ProcessingAnimPanel.SetActive(false);

                if (http.IsSuccessful())
                {
                    Response resp = http.Response();
                    Debug.Log("VerifyResponse: " + resp.Body());

                    VerifyResponse verifyResponse = JsonUtility.FromJson<VerifyResponse>(resp.Body());

                    if (verifyResponse.success)
                    {
                        ShowHomePanel();
                    }
                    else
                    {
                        ErrorMsg.text = verifyResponse.msg;
                        ErrorMsgPanel.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogError("NetWorkError: " + http.Error());

                    ErrorMsg.text = http.Error();
                    ErrorMsgPanel.SetActive(true);
                }
            }
            else
            {
                ProcessingAnimPanel.SetActive(false);

                ErrorMsg.text = userVerifyResult.Msg;
                ErrorMsgPanel.SetActive(true);
            }
        }

        public void OnGetBalanceButtonClick()
        {
            ShowBalancePanel();
            BalanceInfo.text = "ETH: 0";

            ProcessingAnimPanel.SetActive(true);
            StartCoroutine(GetBalanceCoroutine());
        }

        private IEnumerator GetBalanceCoroutine()
        {
            yield return EOTClient.Instance.GetUserBalance(_username);

            ProcessingAnimPanel.SetActive(false);
            UserBalanceResult userBalanceResult = EOTClient.Instance.UserBalanceResult;

            if (userBalanceResult.Code == EOTConstant.CODE_SUCCESS)
            {
                List<Token> tokeList = userBalanceResult.Balance;
                foreach (Token token in tokeList)
                {
                    if (token.tokenCode == TokenCode.ETH)
                    {
                        BalanceInfo.text = "ETH: " + token.amount;
                        break;
                    }
                }
            }
            else
            {
                ErrorMsg.text = userBalanceResult.Msg;
                ErrorMsgPanel.SetActive(true);
            }
        }

        public void OnReturnToHomeButtonClick()
        {
            ShowHomePanel();
        }

        public void OnMakeTransferButtonClick()
        {
            ShowTransferPanel();
        }

        public void OnStartTransferButtonClick()
        {
            string transferTo = TransferToUserName.text;
            string transferAmount = TransferETHAmount.text;

            if (string.IsNullOrEmpty(transferTo))
            {
                ErrorMsg.text = "Please input transfer target user name!";
                ErrorMsgPanel.SetActive(true);

                throw new Exception("Please input transfer target user name!");
            }

            if (string.IsNullOrEmpty(transferAmount))
            {
                ErrorMsg.text = "Please input transfer amount!";
                ErrorMsgPanel.SetActive(true);

                throw new Exception("Please input transfer amount!");
            }

            ProcessingAnimPanel.SetActive(true);
            StartCoroutine(TransferCoroutine(transferTo, transferAmount));
        }

        private IEnumerator TransferCoroutine(string transferTo, string transferAmount)
        {
            yield return EOTClient.Instance.Transfer(_username, transferTo, TokenCode.ETH, transferAmount);

            ProcessingAnimPanel.SetActive(false);
            UserTransferResult userTransferResult = EOTClient.Instance.UserTransferResult;

            if (userTransferResult.Code == EOTConstant.CODE_SUCCESS)
            {
                SuccessMsg.text = "Transfer is successful!";
                SuccessMsgPanel.SetActive(true);
            }
            else
            {
                ErrorMsg.text = userTransferResult.Msg;
                ErrorMsgPanel.SetActive(true);
            }
        }

        private void HideAllPanels()
        {
            StartPanel.SetActive(false);
            LoginPanel.SetActive(false);
            RegisterPanel.SetActive(false);
            HomePanel.SetActive(false);
            VerifyPanel.SetActive(false);
            BalancePanel.SetActive(false);
            TransferPanel.SetActive(false);
        }

        private void ShowStartPanel()
        {
            HideAllPanels();
            StartPanel.SetActive(true);
        }

        private void ShowLoginPanel()
        {
            HideAllPanels();
            LoginPanel.SetActive(true);
        }

        private void ShowRegisterPanel()
        {
            HideAllPanels();
            RegisterPanel.SetActive(true);
        }

        private void ShowHomePanel()
        {
            HideAllPanels();
            HomePanel.SetActive(true);
        }

        private void ShowVerifyPanel()
        {
            HideAllPanels();
            VerifyPanel.SetActive(true);
        }

        private void ShowBalancePanel()
        {
            HideAllPanels();
            BalancePanel.SetActive(true);
        }

        private void ShowTransferPanel()
        {
            HideAllPanels();
            TransferPanel.SetActive(true);
        }
    }
}