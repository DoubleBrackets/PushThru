using System.Collections;
using UnityEngine;
using UnityEditor;

public class RiseScript : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;

    public float time;

    [ContextMenu("Show")]
    public void ShowAnimation()
    {
        StartCoroutine(Corout_ShowAnimation());
    }

    IEnumerator Corout_ShowAnimation()
    {
        float timer = time;
        while(timer >= 0)
        {
            float ratio = Mathf.SmoothStep(1, 0, timer / time);
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, ratio);
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        gameObject.transform.position = endPos;
    }
}
