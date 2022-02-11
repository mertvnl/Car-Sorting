using CustomEventSystem;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GateData GateData;
    [SerializeField] private Path[] paths;
    [SerializeField] private Renderer barrierMesh;
    [SerializeField] private Transform carCreationPoint;

    private const float CAR_CREATION_OFFSET = 13f;

    private GateButtonController gateButton;
    public GateButtonController GateButton { get { return gateButton == null ? gateButton = GetComponentInChildren<GateButtonController>() : gateButton; } }

    private List<CarController> cars = new List<CarController>();

    private bool isOpen;

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
            CarController car = Instantiate(GateData.CarPrefab, carCreationPoint.position + (Vector3.forward * (i * CAR_CREATION_OFFSET)), GateData.CarPrefab.transform.rotation).GetComponent<CarController>();
            car.InitialiseCar(GateData);
            cars.Add(car);

            Path emptyPath = GetEmptyPath();
            emptyPath.ParkArea.InitialiseParkArea(GateData);
        }

        GateButton.InitialiseButton(GateData);
    }

    private void OpenGate(GateType type)
    {
        if (type != GateData.GateType)
            return;

        if (isOpen)
            return;

        GateAnimation();
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

        Path nextPath = GetNextPossiblePath();
        if (nextPath == null)
            nextPath = GetClosestPath(nextCar.transform.position);

        nextCar.HandleMovement(nextPath);
        cars.Remove(nextCar);
        ReorderCars();
    }

    private void ReorderCars()
    {
        foreach (var car in cars)
        {
            car.transform.DOMoveZ(car.transform.position.z - CAR_CREATION_OFFSET, 1f).SetEase(Ease.InOutSine);
        }
    }

    private void GateAnimation()
    {
        isOpen = true;
        Sequence anim = DOTween.Sequence();

        anim.Append(barrierMesh.transform.DOLocalRotate(barrierMesh.transform.localEulerAngles + Vector3.forward * -90f, GateData.GateOpenSpeed / 2)).SetSpeedBased().OnStart(SendNextCar);
        anim.Append(barrierMesh.transform.DOLocalRotate(barrierMesh.transform.localEulerAngles + Vector3.forward * 0, GateData.GateOpenSpeed / 2).OnComplete(() => isOpen = false).SetDelay(0.5f)).SetSpeedBased();
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

    private Path GetClosestPath(Vector3 positionToCompare)
    {
        Path closestPath = null;
        float distance = Mathf.Infinity;
        float currentDist = 0;

        foreach (var path in paths)
        {
            currentDist = Vector3.Distance(positionToCompare, path.ParkArea.transform.position);

            if (currentDist < distance)
            {
                closestPath = path;
                distance = currentDist;
            }
        }

        return closestPath;
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
