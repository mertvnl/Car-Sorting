using CustomEventSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GateData GateData;
    [SerializeField] private Path[] paths;
    [SerializeField] private Renderer barrierMesh;

    private GateButtonController gateButton;
    public GateButtonController GateButton { get { return gateButton == null ? gateButton = GetComponentInChildren<GateButtonController>() : gateButton; } }

    private List<CarController> cars = new List<CarController>();

    public int PathCount { get { return paths.Length; } }

    private void OnEnable()
    {
        Events.OnGateButtonPressed.AddListener(OpenGate);
    }

    private void OnDisable()
    {
        Events.OnGateButtonPressed.RemoveListener(OpenGate);
    }

    public void InitialiseGate(int desiredParkCount)
    {
        SetColor();

        for (int i = 0; i < desiredParkCount; i++)
        {
            CarController car = Instantiate(GateData.CarPrefab, transform).GetComponent<CarController>();
            car.InitialiseCar(GateData);
            cars.Add(car);

            GetEmptyPath().ParkArea.InitialiseParkArea(GateData);
        }

        GateButton.InitialiseButton(GateData);
    }

    private void OpenGate(GateType type)
    {
        if (type != GateData.GateType)
            return;


        SendNextCar();
    }

    private void SetColor()
    {
        Material[] newMaterials = barrierMesh.materials;
        newMaterials[1].color = GateData.MainColor;
        newMaterials[2].color = GateData.SecondaryColor;

        barrierMesh.materials = newMaterials;
    }

    private void SendNextCar()
    {
        if (cars.Count == 0 || cars == null)
            return;

        CarController nextCar = cars[0];
        nextCar.HandleMovement(GetNextPossiblePath());
        cars.Remove(nextCar);
        ReorderCars();
    }

    private void ReorderCars()
    {
        //car positioning
    }

    private Path GetNextPossiblePath()
    {
        Path nextPath = null;

        foreach (var path in paths)
        {
            if (!path.ParkArea.IsParked)
            {
                nextPath = path;
                break;
            }
        }

        return nextPath;
    }

    private Path GetEmptyPath()
    {
        Path emptyPath = null;

        List<Path> tempPath = paths.ToList();

        tempPath.Shuffle();

        foreach (var path in tempPath)
        {
            if (!path.ParkArea.IsInitialised)
            {
                emptyPath = path;
                break;
            }
        }

        return emptyPath;
    }
}
