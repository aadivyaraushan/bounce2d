using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs: EventArgs {
        public Vector3 GunEndPointPosition;
        public Vector3 ShootPosition;
    }
    private Transform AimTransform;
    private Vector3 MousePosition;
    
    [SerializeField]
    public GameObject GunEndPoint;
    [SerializeField]

    // Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPosition() 
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }


    // Start is called before the first frame update
    void Start()
    {
        AimTransform = transform.Find("Aim");
        // GunEndPointTransform = transform.Find("GunEndPointPosition");
        // ShellPositionTransform = transform.Find("ShellPosition");
    }

    // Update is called once per frame
    void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        MousePosition = GetMouseWorldPosition();
        Vector3 AimDirection = (MousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;
        // Mathf.Atan2() converts angle to radians
        // Mathf.Rad2Deg converts radians to degrees.
        AimTransform.eulerAngles = new Vector3(0, 0, angle);
        // Debug.Log(angle);
    }

    private void HandleShooting()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MousePosition = GetMouseWorldPosition();

            OnShoot?.Invoke(this, new OnShootEventArgs {
                GunEndPointPosition = GunEndPoint.transform.position,
                ShootPosition = MousePosition,
            });
        }
    }
}
