using CustomEventSystem;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButtonController : MonoBehaviour
{
    private GateData data;
    [SerializeField] private Renderer buttonMesh;

    private bool isInteractable;

    public void InitialiseButton(GateData gateData)
    {
        isInteractable = true;
        data = gateData;
        buttonMesh.material.color = data.MainColor;
    }

    private void OnMouseDown()
    {
        if (!isInteractable)
            return;

        ButtonAnimation();
        OpenGate();
    }

    private void ButtonAnimation()
    {
        buttonMesh.transform.DOPunchPosition(Vector3.down * 6f, 0.5f, 1, 0).OnComplete(() => isInteractable = true);
    }

    private void OpenGate()
    {
        isInteractable = false;
        Events.OnGateButtonPressed.Invoke(data.GateType);
    }
}
