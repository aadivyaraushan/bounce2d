using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 ShootDir;
    public float MoveSpeed;

    public void Setup(Vector3 ShootDirection)
    {
        ShootDir = ShootDirection;
    }

    public void Update()
    {
        transform.position += ShootDir * Time.deltaTime * MoveSpeed;
    }
}
