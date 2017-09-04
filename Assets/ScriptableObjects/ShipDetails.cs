using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship/Ship Details")]
public class ShipDetails : ShipSO {

    public float Acceleration = 1f;
    public float MaxSpeed = 1f;
    public float Mass = 1f;
    public float RotationRate = 1f;
    public float Crew = 1f;
    public float Battery = 1f;
    public float BatteryRechargeRate = 1f;
    public float AngularDrag = 1f;
    public float Drag = 1f;
    public float Scale = 1f;
    public PrimaryDetails Primary;
    public SecondaryDetails Secondary;
    public string ShipName = "";
    public float AIPrimaryUsagePercent = 70f;
    public float AISecondaryUsagePercent = 30f;
}
