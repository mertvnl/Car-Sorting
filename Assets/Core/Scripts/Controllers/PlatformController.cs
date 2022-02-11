using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private void Awake()
    {
        InitialisePlatform();
    }

    private void InitialisePlatform()
    {
        List<GateController> gates = GetComponentsInChildren<GateController>().ToList();

        int totalParkArea = GetComponentsInChildren<ParkArea>().Length;

        GameManager.Instance.SetTotalCar(totalParkArea);

        foreach (var gate in gates)
        {
            int parkCount;
            if (gates.IndexOf(gate) == gates.Count - 1)
            {
                parkCount = totalParkArea;
            }
            else
            {
                parkCount = GetRandomParkCount(gate.GateData, totalParkArea);
                totalParkArea -= parkCount;
            }

            Debug.Log(gate.name + " selectec park count : " + parkCount + " available path count " + gate.PathCount);
            gate.InitialiseGate(parkCount);
        }
    }



    private int GetRandomParkCount(GateData gateData, int maxCount)
    {
        return Random.Range(gateData.MinimumParkCount, maxCount - 1);
    }
}
