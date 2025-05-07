using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using PlayFab.Networking;
using UnityEngine;

public class ClientStartUp : MonoBehaviour
{
    void Start()
    {
        LoginWithCustomIDRequest request= new LoginWithCustomIDRequest()
        {
            TitleId=PlayFabSettings.TitleId,
            CreateAccount=true,
            CustomId=SystemInfo.deviceUniqueIdentifier
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess,OnLoginError);
    }

    void OnPlayFabLoginSuccess(LoginResult loginResult)
    {
        Debug.Log(message:"Login successful!");
        RequestMultiplayerServer();
    }
    
    private void RequestMultiplayerServer()
    {
        Debug.Log(message:"[ClientStartup].RequestMultiplayerServer");
        RequestMultiplayerServerRequest requestData=new RequestMultiplayerServerRequest
        {
            BuildId="0064f127-8122-4f68-8609-33b76a27b37c",
            SessionId = System.Guid.NewGuid().ToString(),//System.Guid.NewGuid().ToString()
            PreferredRegions=new List<string>{"NorthEurope"}
        };

        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData,OnRequestMultiplayerServer,OnRequestMultiplayerServerError);
    }
    void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
    {
        if(response==null) return;
        Debug.Log(message:"*** THESE ARE THE DETAILS *** --- IP: "+response.IPV4Address+" Port: "+(ushort)response.Ports[0].Num);

        UnityNetworkServer.Instance.networkAddress=response.IPV4Address;
        UnityNetworkServer.Instance.GetComponent<kcp2k.KcpTransport>().Port=(ushort)response.Ports[0].Num;

        UnityNetworkServer.Instance.StartClient();
    }
    void OnRequestMultiplayerServerError(PlayFab.PlayFabError playFabError)
    {
        Debug.Log(message:"An error occured!");
    }
    void OnLoginError(PlayFab.PlayFabError playFabError)
    {
        Debug.Log(message:"Login failed...");
    }
}
