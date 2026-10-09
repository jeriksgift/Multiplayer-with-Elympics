using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerPrefab;

    [Header("Spawn Points")]
    [Tooltip("Four points forming the Blue team's line.")]
    [SerializeField] private Transform[] blueSpawnPoints;

    [Tooltip("Four points forming the Red team's line, facing the Blue line.")]
    [SerializeField] private Transform[] redSpawnPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.HideLoadingScreen();
        }

        if (!PhotonNetwork.InRoom) return;

        if (TryGetSpawnLoc(RoomManager.GetLocalTeam(), RoomManager.GetLocalTeamIndex(), out Transform spawn))
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawn.position, spawn.rotation);
        }
        else
        {
            Debug.LogWarning("No free spawn point available, spawning at origin.");
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity);
        }
    }

    public bool TryGetSpawnLoc(DataTypes.Team team, int preferredIndex, out Transform spawn)
    {
        spawn = null;

        Transform[] points = team == DataTypes.Team.Blue ? blueSpawnPoints : redSpawnPoints;
        if (points == null || points.Length == 0) return false;

        bool[] taken = GetTakenFlags(points.Length);

        if (preferredIndex >= 0 && preferredIndex < points.Length && !taken[preferredIndex])
        {
            spawn = points[preferredIndex];
            return true;
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (taken[i]) continue;
            spawn = points[i];
            return true;
        }

        return false;
    }

    public Transform GetSpawnLoc()
    {
        return TryGetSpawnLoc(RoomManager.GetLocalTeam(), RoomManager.GetLocalTeamIndex(), out Transform spawn)
            ? spawn
            : null;
    }

    private bool[] GetTakenFlags(int count)
    {
        var taken = new bool[count];

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player == null || player.IsLocal) continue;

            int index = RoomManager.GetTeamIndex(player);
            if (index >= 0 && index < count) taken[index] = true;
        }

        return taken;
    }
}