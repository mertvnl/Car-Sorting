using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HCB.Core;

public class GateData : MonoBehaviour
{
    private void OnEnable()
    {
        if (Managers.Instance == null) return;

    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;

    }
}
