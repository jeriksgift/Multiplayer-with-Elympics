using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI nickNameInputField;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.JoinLobby();
    }


    public void CreateOrJoinRoomBtn()
    {
        if (nickNameInputField.text.Length <= 0) return;

        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.NickName = nickNameInputField.text;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null);
        PhotonNetwork.NickName = nickNameInputField.text;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom}");
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
