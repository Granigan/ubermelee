using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Secondary/Secondary Details")]
public class SecondaryDetails : SecondarySO {

    public float Damage = 0f;
    public float Scale = 1f;
    public float Speed = 1f;
    public float TimeToLive = 1f;
    public float Mass = 1f;
    public float Drag = 0f;
    public float AngularDrag = 0f;
    public float BatteryCharge = 1f;
    public float FireRate = 1f; // How often the special can be used. 1f = Once per second.
    public float HitPoints = 1f;
    public GameObject SecondaryPrefab;
    public float AIRangeToUse = 100f;
    public int MaxInstances = 20;
}
