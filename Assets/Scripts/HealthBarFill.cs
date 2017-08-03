using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFill : MonoBehaviour {
    public Image ArmorBarL;
    public Image ArmorBarR;

    public Image EnergyBarL;
    public Image EnergyBarR;
    Quaternion rotation;

    

    //float CurrentArmor;

    // Use this for initialization
    private void Awake()
    {
       
        rotation = transform.rotation;
    }
    

    void Start () {
        // float MaxArmor = Mathf.Round(GetComponentInParent<ShipHandling>().getCurrentCrew());    
        //ArmorBarL = GetComponent<Image>();
        //ArmorBarR = GetComponent<Image>();
        //EnergyBarL = GetComponent<Image>();
        //EnergyBarR = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {

        {
            transform.rotation = rotation;
        }

        //Image Health = GetComponentInChildren<CrewBar>();

        //Image Energy = GetComponentInChildren<BatteryBar>();

        

        ArmorBarL.GetComponent<Image>().fillAmount = Mathf.Round(GetComponentInParent<ShipHandling>().getCurrentCrew()) / Mathf.Round(GetComponentInParent<ShipHandling>().shipDetails.Crew);
        ArmorBarR.GetComponent<Image>().fillAmount = Mathf.Round(GetComponentInParent<ShipHandling>().getCurrentCrew()) / Mathf.Round(GetComponentInParent<ShipHandling>().shipDetails.Crew);

        EnergyBarL.GetComponent<Image>().fillAmount = Mathf.Round(GetComponentInParent<ShipHandling>().getCurrentBattery()) / Mathf.Round(GetComponentInParent<ShipHandling>().shipDetails.Battery);
        EnergyBarR.GetComponent<Image>().fillAmount = Mathf.Round(GetComponentInParent<ShipHandling>().getCurrentBattery()) / Mathf.Round(GetComponentInParent<ShipHandling>().shipDetails.Battery);

    }
}
