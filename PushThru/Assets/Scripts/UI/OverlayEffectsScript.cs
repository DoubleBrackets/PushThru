using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayEffectsScript : MonoBehaviour
{
    public static OverlayEffectsScript instance;

    public Animator effectsAnimator;
    public RectTransform cutsceneBars;

    private void Awake()
    {
        instance = this;
    }

    public void PlayEffect(string name)
    {
        effectsAnimator.Play(name,0,0);
    }

    float shown = 1;
    float hidden = 1.2f;
    public void ShowCutsceneBars()
    {
        StartCoroutine(Corout_MoveCutsceneBars(hidden,shown));
    }

    public void HideCutsceneBars()
    {
        StartCoroutine(Corout_MoveCutsceneBars(shown, hidden));
    }

    private IEnumerator Corout_MoveCutsceneBars(float start, float end)
    {
        for(int x = 0;x <= 10;x++)
        {
            float t = (float)x / 10;
            Vector3 cScale = cutsceneBars.localScale;
            cScale.y = Mathf.Lerp(start, end, t);
            cutsceneBars.localScale = cScale;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
