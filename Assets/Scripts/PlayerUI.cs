using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TextMeshProUGUI nicknameTxt;

    private void Start()
    {
        nicknameTxt.text = photonView.Owner.NickName;
    }
}
