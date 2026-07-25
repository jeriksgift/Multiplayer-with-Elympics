using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviourPun
{
    [SerializeField] private GameObject bulletPrefab;

    private Queue<GameObject> bullets = new();

    [SerializeField] private int maxBulletsCount;

    [SerializeField] private float bulletForce;

    [SerializeField] private Transform muzzlePoint;

    private void Start()
    {
        InstantiateBullets();
    }

    private void Update()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        if (InputManager.Instance.fire.WasPressedThisFrame()) Fire();
    }

    private void InstantiateBullets()
    {
        for (int i = 0; i < maxBulletsCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);

            bullets.Enqueue(bullet);
        }
    }

    private void Fire()
    {
        GameObject bullet = bullets.Dequeue();
        bullet.SetActive(true);
        bullet.transform.position = muzzlePoint.position;
        bullet.GetComponent<Collider>().enabled = true;
        bullet.GetComponent<Rigidbody>().AddForce(Camera.main.gameObject.transform.forward * bulletForce, ForceMode.Impulse);
        bullets.Enqueue(bullet);
    }
}
