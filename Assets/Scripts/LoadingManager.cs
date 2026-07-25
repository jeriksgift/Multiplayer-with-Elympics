using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [SerializeField] private RectTransform loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private RectTransform loadingImage;
    [SerializeField] private float loadingIconRotateSpeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (loadingPanel.gameObject.activeSelf)
        {
            loadingImage.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * loadingIconRotateSpeed);
        }
    }

    public void ShowLoadingScreen(string loadingTxt)
    {
        loadingPanel.gameObject.SetActive(true);
        loadingText.text = loadingTxt;
    }

    public void HideLoadingScreen()
    {
        loadingPanel.gameObject.SetActive(false);
    }

    public void LoadNetworkLevel(string levelName)
    {
        loadingPanel.gameObject.SetActive(true);
        loadingText.text = "Loading...";
        PhotonNetwork.LoadLevel(levelName);
    }
}
