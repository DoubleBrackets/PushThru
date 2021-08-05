using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashVFXScript : MonoBehaviour
{

    [System.Serializable]
    public struct LineStreak
    {
        public LineRenderer renderer;
        public Vector3 offset;
    }


    public LineStreak[] lineRenderers;
    public float lineDuration;
    public float marginSize;

    public Vector3 sonicBoomOffset;
    public GameObject sonicBoomObject;

    [ContextMenu("test")]
    public void CreateDashVFXTest()
    {
        CreateDashVFX(transform.position, transform.position + new Vector3(0, 2, 0), Vector3.zero);
    }

    public void CreateDashVFX(Vector3 startPos, Vector3 endPos,Vector3 dir)
    {
        Quaternion direction = Quaternion.Euler(new Vector3(0, -Mathf.Rad2Deg*Mathf.Atan2(dir.z, dir.x)+90f, 0));
        print(direction.eulerAngles);
        CreateLineStreaks(startPos, endPos,dir,transform.rotation);
        sonicBoomObject.transform.position = transform.position + direction * sonicBoomOffset;
        sonicBoomObject.transform.rotation = direction;
        ParticleManager.particleManager.PlayParticle("DashBoomParticles");
    }

    private void CreateLineStreaks(Vector3 startPos, Vector3 endPos,Vector3 dir,Quaternion playerRotation)
    {
        startPos -= dir * marginSize;
        endPos += dir * marginSize;
        foreach(LineStreak lineStreak in lineRenderers)
        {
            lineStreak.renderer.positionCount = 6;
            Vector3 offset = playerRotation * lineStreak.offset;

            for(int c = 0;c <= 5;c++)
            {
                lineStreak.renderer.SetPosition(c, Vector3.Lerp(startPos,endPos,c/5f) + offset);
            }
        }
        StartCoroutine(Corout_HideLineStreaks(startPos,endPos,playerRotation));
    }

    IEnumerator Corout_HideLineStreaks(Vector3 startPos, Vector3 endPos,Quaternion playerRotation)
    {
        yield return new WaitForSeconds(lineDuration);
        float interval = lineDuration / 10f;
        for(int x = 1;x <= 10f;x++)
        {
            foreach (LineStreak lineStreak in lineRenderers)
            {
                Vector3 offset = playerRotation * lineStreak.offset;
                for (int c = 0; c <= 5; c++)
                {
                    lineStreak.renderer.SetPosition(c, Vector3.Lerp(Vector3.Lerp(startPos,endPos,x/10f), endPos, c / 5f)+ offset);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}

