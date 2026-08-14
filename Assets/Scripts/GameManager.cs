using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadingManager.Instance.HideLoadingScreen();
        Transform spawnLoc = GetSpawnLoc();
        PhotonNetwork.Instantiate(playerPrefab.name, spawnLoc.position, spawnLoc.rotation);

        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }

    public Transform GetSpawnLoc()
    {
        return spawnPoints[(PhotonNetwork.LocalPlayer.ActorNumber - 1) % spawnPoints.Length];
    }
}
