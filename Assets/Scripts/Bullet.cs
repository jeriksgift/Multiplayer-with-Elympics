using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float time;
    private void OnEnable()
    {
        time = Time.time;
    }

    private void OnDisable()
    {
        time = 0;
    }

    private void Update()
    {
        if (Time.time > time + 5f)
        {
            ResetObject();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit " + collision.gameObject.GetComponent<PhotonView>().Owner.NickName);
        }

        ResetObject();
    }

    private void ResetObject()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        gameObject.SetActive(false);
    }
}
