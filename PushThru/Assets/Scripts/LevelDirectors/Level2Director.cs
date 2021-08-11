using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Director : MonoBehaviour
{
    public static Level2Director instance;

    public AnimationController playerAnimController;
    public MeshRenderer[] InvertExpanding;
    public GameObject cameraTargetTransition;

    public GameObject healthBars;
    public GameObject shadowBro;

    private void Awake()
    {
        instance = this;
        foreach (MeshRenderer renderer in InvertExpanding)
        {
            renderer.sharedMaterial.SetFloat("_Distance", 0);
        }
    }

    public void SwapColors()
    {
        StartCoroutine(Corout_SwapColorsCutscene());
    }

    public void EnemyFinishingAttack()
    {
        StartCoroutine(Corout_FinishingAttack());
    }

    private IEnumerator Corout_FinishingAttack()
    {
        InputManager.instance.IncrementAllInputEnabled();
        OverlayEffectsScript.instance.StartCutScene(shadowBro);
        yield return new WaitForSecondsRealtime(1f);
        DialogueTextManager.instance.QueueMessage("???: . . . You've held out this time~");
        DialogueTextManager.instance.QueueMessage("???: See you tomorrow, then.");
        while (DialogueTextManager.instance.isDisplayingMessage)
        {
            yield return null;
        }
        ParticleManager.particleManager.PlayParticle("FinalAttackParticles");
        OverlayEffectsScript.instance.EndCutScene();
        yield return new WaitForSeconds(2.5f);
        PlayerEntity.player.TakeDamage(new Attack(100000, Vector3.zero));
        InputManager.instance.DecrementAllInputEnabled();
    }

    private IEnumerator Corout_SwapColorsCutscene()
    {
        InputManager.instance.IncrementAllInputEnabled();
        int frames = 20;
        for(int x = 0;x <= frames;x++)
        {
            float t = (float)x / frames;
            float setDistance = Mathf.Lerp(0, 10, t);
            foreach(MeshRenderer renderer in InvertExpanding)
            {
                renderer.sharedMaterial.SetFloat("_Distance", setDistance);
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
        OverlayEffectsScript.instance.StartCutScene(cameraTargetTransition);
        shadowBro.SetActive(true);
        DialogueTextManager.instance.QueueMessage("???: Here again?");
        DialogueTextManager.instance.QueueMessage("???: You'll never get rid of me, and it seems like I'll never get rid of you.");
        DialogueTextManager.instance.QueueMessage("???: I guess you really want to do this till we die. Fair enough~");
        frames = 100;
        for (int x = 0; x <= frames; x++)
        {
            float t = (float)x / frames;
            float setDistance = Mathf.Lerp(10, 75, t);
            foreach (MeshRenderer renderer in InvertExpanding)
            {
                renderer.sharedMaterial.SetFloat("_Distance", setDistance);
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
        foreach (MeshRenderer renderer in InvertExpanding)
        {
            renderer.sharedMaterial.SetFloat("_Distance", 100000);
        }
        while (DialogueTextManager.instance.isDisplayingMessage)
        {
            yield return null;
        }
        OverlayEffectsScript.instance.EndCutScene();
        yield return new WaitForSeconds(1f);
        InputManager.instance.DecrementAllInputEnabled();
        healthBars.SetActive(true);
    }


}
