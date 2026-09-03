using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    //public List<Player> players;

    [SerializeField] private GameObject slotsContainer;

    private Dictionary<int, Player> players = new();
    private List<RoomSlot> slots = new();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var slot in slotsContainer.GetComponentsInChildren<RoomSlot>())
        {
            slots.Add(slot);
        }
    }

    public override void OnJoinedRoom()
    {
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        players.Add(GetEmptySlotIndex(), PhotonNetwork.LocalPlayer);
        UpdateSlots();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        players.Add(GetEmptySlotIndex(), newPlayer);
        UpdateSlots();
    }

    private int GetEmptySlotIndex()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].isOccupied)
            {
                return i;
            }
        }
        return -1;
    }

    private void UpdateSlots()
    {
        ClearAllSlots();
        foreach (var player in players)
        {
            if (player.Value.IsMasterClient)
            {
                slots[player.Key].UpdateSlot(true, player.Value.NickName + " (Host)");
            }
            slots[player.Key].UpdateSlot(true, player.Value.NickName);
        }
    }

    private void ClearAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.UpdateSlot(false);
        }
    }
}
