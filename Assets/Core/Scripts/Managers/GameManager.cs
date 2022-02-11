using CustomEventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isStageCompleted;

    private int totalCarToPark;
    private int currentParkAmount;

    public void CompleteStage(bool isSuccess, float delay = 0)
    {
        if (isStageCompleted)
            return;

        isStageCompleted = true;
        Events.OnLevelSuccess.Invoke();
        StartCoroutine(CompleteStageCo(isSuccess, delay));
    }

    private IEnumerator CompleteStageCo(bool isSuccess, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        if (isSuccess)
            LevelManager.Instance.RestartLevel();
        else
            LevelManager.Instance.RestartLevel();
    }

    public void SetTotalCar(int i)
    {
        totalCarToPark = i;
    }

    public void IncreaseCurrentParkAmount()
    {
        currentParkAmount++;
        CheckParkStatus();
    }

    public void CheckParkStatus()
    {
        if (currentParkAmount >= totalCarToPark)
            CompleteStage(true, 5);
    }
}
