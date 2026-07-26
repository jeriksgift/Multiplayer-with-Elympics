using UnityEngine;
using Photon.Pun;

public class WeaponSystem : MonoBehaviourPun
{
    [SerializeField] private float bulletDamage;

    [SerializeField] private Transform muzzlePoint;

    private void Update()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        if (InputManager.Instance.fire.WasPressedThisFrame()) Fire();
    }

    private void Fire()
    {
        RaycastHit? hit = FireHit();

        if (hit.HasValue)
        {
            if (hit.Value.collider.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
            {
                healthComponent.Damage(bulletDamage);
            }
        }
    }

    private RaycastHit? FireHit()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit;
        }
        else
        {
            return null;
        }
    }
}
