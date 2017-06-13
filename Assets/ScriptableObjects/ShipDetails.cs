using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship/Ship Details")]
public class ShipDetails : ShipSO {

    public float Acceleration;
    public float MaxSpeed;
    public float Mass;
    public float RotationRate;
    public float Crew;
    public float Battery;
    public float BatteryRechargeRate;
    public float AngularDrag;
    public float Drag;
    public float Scale = 1;

}
