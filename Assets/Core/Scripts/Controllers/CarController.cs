using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    [SerializeField] private Renderer[] carModels;

    [SerializeField] private ParkEffectController parkEffectPrefab;

    private GateData data;
    private ParkArea currentParkArea;

    public void InitialiseCar(GateData gateData)
    {
        data = gateData;
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
        currentParkArea = targetPath.ParkArea;
        targetPath.ParkArea.IsParked = true;
        var pathTween = transform.DOPath(targetPath.dtPath.wps.ToArray(), data.CarSpeed, PathType.CatmullRom, PathMode.Full3D, 10, Color.red)
            .SetSpeedBased()
            .SetEase(data.CarEaseType)
            .OnComplete(CheckParkStatus)
            .SetId("movementTween");
        pathTween.SetLookAt(-1, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        CarController car = other.GetComponent<CarController>();

        if (car != null)
        {
            Instantiate(parkEffectPrefab, transform.position + Vector3.up * 10, parkEffectPrefab.transform.rotation).PlayEffect(false);
            InterruptMovement();
        }
    }

    private void CheckParkStatus()
    {
        if (currentParkArea.GateType == data.GateType)
        {
            Instantiate(parkEffectPrefab, transform.position + Vector3.up * 10, parkEffectPrefab.transform.rotation).PlayEffect(true);
            GameManager.Instance.IncreaseCurrentParkAmount();
        }
        else
        {
            Instantiate(parkEffectPrefab, transform.position + Vector3.up * 10, parkEffectPrefab.transform.rotation).PlayEffect(false);
            InterruptMovement();
        }
    }

    private void InterruptMovement()
    {
        StartCoroutine(InterruptMovementCo());
    }

    private IEnumerator InterruptMovementCo()
    {
        DOTween.Kill("movementTween");

        yield return new WaitForSeconds(2);

        GameManager.Instance.CompleteStage(false);
    }
}
