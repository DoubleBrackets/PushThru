using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtils : MonoBehaviour
{
    public static TimeUtils instance;

    Coroutine freezeTimeCoroutine;

    public static float fixedTimeStep;

    private void Awake()
    {
        instance = this;
        fixedTimeStep = Time.fixedDeltaTime;
    }
    public void FreezeTime(float timescale, float duration)
    {
        Time.timeScale = timescale;
        if (timescale != 0)
            Time.fixedDeltaTime = fixedTimeStep / timescale;
        FreezeTime(timescale, duration, 0);
    }

    public void FreezeTime(float timescale, float duration, float delay)
    {
        if (freezeTimeCoroutine != null)
        {
            StopCoroutine(freezeTimeCoroutine);
        }
        freezeTimeCoroutine = StartCoroutine(FreezeTimeCorout(timescale, duration, delay));
    }

    private IEnumerator FreezeTimeCorout(float timescale, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = timescale;
        if (timescale != 0)
            Time.fixedDeltaTime = fixedTimeStep / timescale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedTimeStep;
        freezeTimeCoroutine = null;
    }

}
