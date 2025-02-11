using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkInGameMessage : NetworkBehaviour
{
    InGameMessageUIHandler inGameMessageUIHandler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SendInGameRPCMessage(string userNickname, string message)
    {
        RPC_InGameMessage($"<b>{userNickname}</b> {message}");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_InGameMessage(string message, RpcInfo info = default)
    {
        Debug.Log($"[RPC] InGameMessage {message}");

        if (inGameMessageUIHandler == null)
            inGameMessageUIHandler = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<InGameMessageUIHandler>();

        if (inGameMessageUIHandler != null)
            inGameMessageUIHandler.OnGameMessageReceived(message);
    }
}
