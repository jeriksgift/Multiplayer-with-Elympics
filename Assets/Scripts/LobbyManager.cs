using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI nickNameInputField;

    [SerializeField] private TMP_InputField roomNameInputField;

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;

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
        Debug.Log("Joined Lobby " + PhotonNetwork.CurrentLobby.Name);
        LoadingManager.Instance.HideLoadingScreen();
    }

    public void CreateOrJoinRoomBtn()
    {
        if (nickNameInputField == null) return;
        if (string.IsNullOrEmpty((nickNameInputField.text).Trim()) || nickNameInputField.text.Length <= 1) // The text length is giving 1 even if no text was typed
        {
            Debug.Log("Invalid Nickname");
            return;
        }

        PhotonNetwork.NickName = nickNameInputField.text;

        if (roomNameInputField.text.Length <= 1)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Joining room " + roomNameInputField.text);
            PhotonNetwork.JoinRoom(roomNameInputField.text);
        }

        LoadingManager.Instance.ShowLoadingScreen("Loading...");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.NickName = nickNameInputField.text;
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { MaxPlayers = 8 });
        Debug.Log("Creating room " + roomNameInputField.text);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.NickName = nickNameInputField.text;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8 });
    }

    public override void OnJoinedRoom()
    {
        //LoadingManager.Instance.LoadNetworkLevel("Game");

        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

        LoadingManager.Instance.HideLoadingScreen();
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
