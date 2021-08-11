using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    private void Awake()
    {
        instance = this;
    }

    public void ChangeScenes(string scene)
    {
        StartCoroutine(Corout_ChangeScenes(scene));
    }

    private IEnumerator Corout_ChangeScenes(string scene)
    {
        OverlayEffectsScript.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(scene);
    }
}
