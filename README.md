# eot-unity-sdk
Unity SDK is a unity implement for EON Protocol, which can help unity app developer integrate `crypto token` into their apps very easily.

## Installation

Download the [UnitySDK-Beta.unitypackage](https://raw.githubusercontent.com/eontoken/eot-unity-sdk/master/UnitySDK-Beta.unitypackage) file
and inside Unity go to the menu `Assets -> Import New Assets...` and select the just downloaded SDK package.


## Features

Currently, `Unity SDK` is a beta vserion and supports the most part functions of EON protocol:
* Integrate ETH Token to APP 
* Get balance
* Make transfer

All of these features with a simple and consistent API. In the future it will also support:
* More Token supported
* Statistic function supported
* etc.

## Usage

`eot-unity-sdk` is very easy to integrate and use. Before you start integrate it to your app, you should create an app in [eot developer portal](https://developer.eontoken.io/) to get the `app_id` and `app_key`. 

Once you get `app_id` and `app_key`, you can initialize the `eot sdk` as following code: 

```
private void Start()
{
    EOTClient.Init(DemoUtils.DEMO_APP_ID, DemoUtils.DEMO_APP_KEY);
}
```

After intialization, you can user `EOTClient.Instanse` to invoke the following provided api.


### Verify User

`Verify user` interface is used to connect app user to `EOT Platform` user. App user need register an account on [eot user portal](https://user.eontoken.io/). Once registered, user can deposit, widraw easily on `eot user portal`. But you can just make transfer in app which is important. All balance is synchronized between app and `EOT Platform`. 

Use `EOTClient.Instance.VerifyUser` to verify user with app user name:

```
private IEnumerator VerifyCoroutine()
{
    yield return EOTClient.Instance.VerifyUser(app_username);
    UserVerifyResult userVerifyResult = EOTClient.Instance.UserVerifyResult;

    if (userVerifyResult.Code == EOTConstant.CODE_SUCCESS)
    {
        ...
    }
    else
    {
        ...
    }
}
```


### Get Balance

`Get Balance` interface is used to get app user's balance info, which is very easy to use. The following code is a simple demo to use this interface.

```
private IEnumerator GetBalanceCoroutine()
{
    yield return EOTClient.Instance.GetUserBalance(_username);
    UserBalanceResult userBalanceResult = EOTClient.Instance.UserBalanceResult;

    if (userBalanceResult.Code == EOTConstant.CODE_SUCCESS)
    {
        ...
    }
    else
    {
        ...
    }
}
```


### Transfer

`Transfer` interface is used for app user to make some transaction, which is very easy to use. The following code is a simple demo to use this interface.

```
private IEnumerator TransferCoroutine(string transferTo, string transferAmount)
{
    yield return EOTClient.Instance.Transfer(_username, transferTo, TokenCode.ETH, transferAmount);
    UserTransferResult userTransferResult = EOTClient.Instance.UserTransferResult;

    if (userTransferResult.Code == EOTConstant.CODE_SUCCESS)
    {
        ...
    }
    else
    {
        ...
    }
}
```


## Demo
In this project, we support a demo app in [Example folder](/UnitySDK/Assets/EOTSDK/Examples). The demo app used all the interfaces and the codes are very easy to understand. You can run it to understand the integration flow. Also you can use the codes in your own app.


## License

`eot-unity-sdk` is licensed under the [BSD-3-Clause](https://opensource.org/licenses/BSD-3-Clause).
