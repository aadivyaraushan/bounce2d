using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    [SerializeField]
    private Transform pfBullet;
    [SerializeField]
    private AimWeapon AimWeapon;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        AimWeapon.OnShoot += AimWeaponOnShoot;
    }

    private void AimWeaponOnShoot(object sender, AimWeapon.OnShootEventArgs e)
    {
        // Shoot
        Transform ProjectileTransform = Instantiate(pfBullet, e.GunEndPointPosition, Quaternion.identity);

        Vector3 ShootDirection = (e.ShootPosition - e.GunEndPointPosition).normalized;
        ProjectileTransform.GetComponent<Projectile>().Setup(ShootDirection);
    }
}
