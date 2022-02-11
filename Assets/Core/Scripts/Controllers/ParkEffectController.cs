using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkEffectController : MonoBehaviour
{
    [SerializeField] private GameObject tick, x;

    public void PlayEffect(bool isSuccess)
    {
        transform.localScale = Vector3.zero;

        if (isSuccess)
            tick.SetActive(true);
        else
            x.SetActive(true);

        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }
}
