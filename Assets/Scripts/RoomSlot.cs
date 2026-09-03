using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSlot : MonoBehaviour
{
    public bool isOccupied = false;
    private string playerName;
    private int index;
    
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private DataTypes.Team team;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        
    }

    public void UpdateSlot(bool isOccupied, string playerName)
    {
        this.isOccupied = isOccupied;
        this.playerName = playerName;
        playerNameText.text = playerName;
    }
    public void UpdateSlot(bool isOccupied)
    {
        this.isOccupied = isOccupied;
        playerName = "";
        playerNameText.text = playerName;
    }
}
