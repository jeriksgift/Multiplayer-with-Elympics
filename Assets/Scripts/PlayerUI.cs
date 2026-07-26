using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TextMeshProUGUI nicknameTxt;

    private void Start()
    {
        DestroyOtherCanvases();

        nicknameTxt.text = photonView.Owner.NickName;
    }

    private void DestroyOtherCanvases()
    {
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");
        foreach(GameObject player in canvases)
        {
            if (!PhotonView.Get(player).IsMine)
            {
                Destroy(player);
            }
        }
    }
}
