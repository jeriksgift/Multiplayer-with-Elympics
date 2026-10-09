using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float> HealthChanged;

    private float health;
    [SerializeField] private float maxHealth;
    private bool isDead;

    private PhotonView photonView;

    private MeshRenderer[] renderers;
    private Collider[] colliders;
    private PlayerMovement playerMovement;
    private WeaponSystem weaponSystem;
    private Rigidbody rigidbody;

    [SerializeField] private ParticleSystem damageParticleSystem;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
        playerMovement = GetComponent<PlayerMovement>();
        weaponSystem = GetComponent<WeaponSystem>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        health = maxHealth;
        isDead = false;
    }

    [PunRPC]
    public void Damage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isDead) return;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        HealthChanged?.Invoke(health / maxHealth);
        photonView.RPC(nameof(PlayBloodEffectRpc), RpcTarget.All, hitPoint, hitNormal);
        if (health <= 0)
        {
            Dead();
            isDead = true;
        }
    }

    [PunRPC]
    private void PlayBloodEffectRpc(Vector3 hitPoint, Vector3 hitNormal)
    {
        damageParticleSystem.transform.position = hitPoint;
        damageParticleSystem.transform.rotation = Quaternion.LookRotation(hitNormal);

        damageParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        damageParticleSystem.Play();
    }

    private void Dead()
    {
        Debug.Log("Dead");
        //EnableOrDisableGraphics(false);
        if (!photonView.IsMine) return;
        photonView.RPC(nameof(EnableOrDisableGraphics), RpcTarget.All, false);
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2f);
        health = maxHealth;
        isDead = false;
        HealthChanged?.Invoke(health / maxHealth);

        Transform spawnLoc = GameManager.Instance.GetSpawnLoc();
        if (spawnLoc != null)
        {
            transform.position = spawnLoc.position;
            transform.rotation = spawnLoc.rotation;
        }

        photonView.RPC(nameof(EnableOrDisableGraphics), RpcTarget.All, true);
    }

    [PunRPC]
    private void EnableOrDisableGraphics(bool enable)
    {
        foreach (var item in renderers)
        {
            item.enabled = enable;
        }
        foreach (var item in colliders)
        {
            item.enabled = enable;
        }
        playerMovement.enabled = enable;
        weaponSystem.enabled = enable;
        rigidbody.useGravity = enable;
    }
}
