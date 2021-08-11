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
        ChangeScenes(scene, 0);
    }
    public void ChangeScenes(string scene,float delay)
    {
        StartCoroutine(Corout_ChangeScenes(scene,delay));
    }
    private IEnumerator Corout_ChangeScenes(string scene,float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        OverlayEffectsScript.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(scene);
    }
}
