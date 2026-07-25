using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI nickNameInputField;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting...");
    }

    private void Start()
    {
        LoadingManager.Instance.ShowLoadingScreen("Connecting...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master " + Time.time);
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        LoadingManager.Instance.HideLoadingScreen();
    }

    public void CreateOrJoinRoomBtn()
    {
        if (nickNameInputField == null) return;
        if (nickNameInputField.text.Length <= 0)
        {
            Debug.Log("Invalid Nickname");
            return;
        }

        PhotonNetwork.NickName = nickNameInputField.text;
        PhotonNetwork.JoinRandomRoom();

        LoadingManager.Instance.ShowLoadingScreen("Loading...");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.NickName = nickNameInputField.text;
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        LoadingManager.Instance.LoadNetworkLevel("Game");
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
