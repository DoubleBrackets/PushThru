using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayEffectsScript : MonoBehaviour
{
    public static OverlayEffectsScript instance;

    public Animator effectsAnimator;
    [SerializeField]RectTransform cutsceneBars;
    [SerializeField]private Image fadeToBlack;

    private void Awake()
    {
        instance = this;
        FadeFromBlack();
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
        for(int x = 0;x <= 20;x++)
        {
            float t = (float)x / 20;
            Vector3 cScale = cutsceneBars.localScale;
            cScale.y = Mathf.Lerp(start, end, t);
            cutsceneBars.localScale = cScale;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartCutScene(GameObject cameraTarget)
    {
        OrthoPixelMoveCamera.orthoCam.target = cameraTarget.transform;
        ShowCutsceneBars();
    }
    public void EndCutScene()
    {
        HideCutsceneBars();
        OrthoPixelMoveCamera.orthoCam.target = PlayerEntity.player.gameObject.transform;
    }

    public void FadeToBlack()
    {
        StartCoroutine(Corout_FadeScreen(0, 1));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Corout_FadeScreen(1, 0));
    }

    private IEnumerator Corout_FadeScreen(float start, float end)
    {
        int frames = 40;
        for(int x = 0;x <= frames;x++)
        {
            Color c = fadeToBlack.color;
            c.a = Mathf.Lerp(start, end, (float)x / frames);
            fadeToBlack.color = c;
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
}
