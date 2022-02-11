using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Renderer[] carModels;

    private GateData data;
    private float carSpeed;

    public void InitialiseCar(GateData gateData)
    {
        data = gateData;
        carSpeed = data.CarSpeed;
        SetRandomCar();
    }

    private void SetRandomCar()
    {
        foreach (var model in carModels)
        {
            model.enabled = false;
        }

        Renderer currentCarModel = carModels.GetRandom();
        currentCarModel.enabled = true;
        currentCarModel.material.color = data.MainColor;
    }

    public void HandleMovement(Path targetPath)
    {
        //car movement logic
    }
}
