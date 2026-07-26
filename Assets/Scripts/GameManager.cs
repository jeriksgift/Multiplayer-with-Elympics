using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform[] spawnPoints;
    private List<int> usedSpawnPoints = new List<int>();
    private int rand = -1;

    private void Start()
    {
        LoadingManager.Instance.HideLoadingScreen();
        Transform spawnLoc = GetRandomSpawnPoint();
        PhotonNetwork.Instantiate(playerPrefab.name, spawnLoc.position, spawnLoc.rotation);

        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }

    private Transform GetRandomSpawnPoint()
    {
        do
        {
            rand = Random.Range(0, spawnPoints.Length);
        }
        while (usedSpawnPoints.Contains(rand));

        usedSpawnPoints.Add(rand);

        return spawnPoints[rand];
    }
}
