using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateType
{
    Gate1,
    Gate2
}

[System.Serializable]
public class Path
{
    public ParkArea ParkArea;
    public DOTweenPath dtPath;
}

[CreateAssetMenu(fileName = "NewGateData", menuName = "ScriptableObjects/Data/GateData")]
public class GateData : ScriptableObject
{
    [Header("Gate Settings")]
    public GateType GateType;
    public Color MainColor, SecondaryColor;
    public float GateOpenSpeed;
    public int MinimumParkCount;

    [Header("Car Settings")]
    public GameObject CarPrefab;
    public float CarSpeed;
    public Ease CarEaseType;
}
