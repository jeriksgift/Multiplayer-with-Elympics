using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TextMeshProUGUI nicknameTxt;

    private void Start()
    {
        if (!photonView.IsMine) return;

        nicknameTxt.text = photonView.Owner != null ? photonView.Owner.NickName : string.Empty;

        DestroyOtherCanvases();
    }

    /// <summary>
    /// Player HUD canvases resolve their owning player through PhotonView.Get, which
    /// walks up from Player/Canvas to the view on the player root. Canvases that belong
    /// to no player - the Lobby canvas, the LoadingManager overlay, the debug console -
    /// return null and are left alone.
    /// </summary>
    private void DestroyOtherCanvases()
    {
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");

        foreach (GameObject canvasObject in canvases)
        {
            PhotonView owner = PhotonView.Get(canvasObject);

            // Not a player HUD - scene/LoadingManager/IngameDebugConsole. Keep it.
            if (owner == null) continue;

            if (owner.IsMine) continue;

            Destroy(canvasObject);
        }
    }
}
