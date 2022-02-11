using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkArea : MonoBehaviour
{
    private bool isParked;
    public bool IsParked { get { return isParked; } set { isParked = value; } }

    private bool isInitialised;
    public bool IsInitialised { get { return isInitialised; } }

    private GateData data;
    public void InitialiseParkArea(GateData gateData)
    {
        isInitialised = true;
        data = gateData;
        GetComponentInChildren<SpriteRenderer>().color = data.MainColor;
    }
}
