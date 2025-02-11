using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public TextMeshProUGUI playerNicknameTxt;
    public static NetworkPlayer Local { get; set; }

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    public NetworkString<_16> nickname { get; set; }

    bool isPublicJoinMessageSent = false;

    public LocalCameraHandler localCameraHandler;
    public GameObject localUI;

    NetworkInGameMessage networkInGameMessage;

    private void Awake()
    {
        networkInGameMessage = GetComponent<NetworkInGameMessage>();
    }

    void Start()
    {

    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            Camera.main.gameObject.SetActive(false);

            RPC_SetNickname(PlayerPrefs.GetString("PlayerNickname"));

            Debug.Log("Spawned local player");
        }
        else
        {
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;

            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;

            localUI.SetActive(false);

            Debug.Log("Spawned remote player");
        }

        Runner.SetPlayerObject(Object.InputAuthority, Object);

        transform.name = $"Player {Object.Id}";
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if(Runner.TryGetPlayerObject(player, out NetworkObject playerLeftNetworkObject))
            {
                if (playerLeftNetworkObject == Object)
                    Local.GetComponent<NetworkInGameMessage>().SendInGameRPCMessage(playerLeftNetworkObject.GetComponent<NetworkPlayer>().nickname.ToString(), "left");
            }
        }

        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }

    static void OnNicknameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnNicknameChanged value {changed.Behaviour.nickname}");

        changed.Behaviour.OnNicknameChanged();
    }

    private void OnNicknameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickname} for player {gameObject.name}");

        playerNicknameTxt.text = nickname.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickname(string nickname, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickname {nickname}");
        this.nickname = nickname;

        if (!isPublicJoinMessageSent)
        {
            networkInGameMessage.SendInGameRPCMessage(nickname, "joined");

            isPublicJoinMessageSent = true;
        }
    }
}
