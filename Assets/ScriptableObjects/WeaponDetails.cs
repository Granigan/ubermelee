using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Details")]
public class WeaponDetails : WeaponSO {

    public float Damage = 1f;
    public float Scale = 1f;
    public float Speed = 1f;
    public float TimeToLive = 1f;
    public float Mass = 1f;
    public float Drag = 1f;
    public float AngularDrag = 1f;
    public float BatteryCharge = 1f;
    public float FireRate = 1f; // How often the weapon can be shot. 1f = Once per second.
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
}
