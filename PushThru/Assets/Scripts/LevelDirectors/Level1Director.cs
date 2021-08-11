using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Director : MonoBehaviour
{
    public AnimationController playerAnimController;
    public InputManager playerInputManager;
    public GameObject pressSpacebartostart;

    public GameObject bridgeToDrop;
    public GameObject bridgeDropCameraTarget;
    public GameObject bridgeGate;

    bool readyToGetUp = false;
    bool started = false;

    private void Update()
    {
        if(readyToGetUp && Input.GetKeyDown(KeyCode.E))
        {
            readyToGetUp = false;
            PlayerGetsUp();
        }
        if(!started && Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
            pressSpacebartostart.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void Awake()
    {
        playerAnimController.PlayAnimation("LyingDown");
        playerAnimController.SetHipSword(false);
        playerInputManager.IncrementAllInputEnabled();

        Time.timeScale = 0.000001f;
    }

    private void Start()
    {
        StartCoroutine(Corout_StartingTextPrompt());
    }

    #region wakeup
    private IEnumerator Corout_StartingTextPrompt()
    {
        yield return new WaitForSeconds(12f);
        DialogueTextManager.instance.QueueMessage("You open your eyes.");
        readyToGetUp = true;
    }

    public void PlayerGetsUp()
    {
        playerAnimController.PlayAnimation("GetUp");
        StartCoroutine(Corout_PlayerGetsUp());
    }

    private IEnumerator Corout_PlayerGetsUp()
    {
        yield return new WaitForSeconds(5f);
        DialogueTextManager.instance.QueueMessage("This place looks familiar           \n                          \n but something's changed . . .");
        DialogueTextManager.instance.QueueMessage("\n            [WASD] to move\n[E] to interact with Yellow Glowing Objects");
        playerInputManager.movementInputEnabled--;
    }

    public void PlayerGetsRuler()
    {
        playerAnimController.SetHipSword(true);
        DialogueTextManager.instance.QueueMessage("You remember swinging yardsticks around in school a lot");
        DialogueTextManager.instance.QueueMessage("The teachers hated it. ");
        DialogueTextManager.instance.QueueMessage("You'd better take it along ");
        DialogueTextManager.instance.QueueMessage("In case you need to defend yourself with some super slick moves.");
        DialogueTextManager.instance.QueueMessage("[LMB] to do slick moves \n     \n[RMB] to block");
        playerInputManager.actionInputEnabled--;
    }
    #endregion

    public void DropPortal()
    {        
        StartCoroutine(Corout_DropPortal());
    }

    private IEnumerator Corout_DropPortal()
    {
        playerInputManager.IncrementAllInputEnabled();
        yield return new WaitForSeconds(2.5f);
        OverlayEffectsScript.instance.StartCutScene(bridgeDropCameraTarget);
        yield return new WaitForSeconds(3f);
        //Drops the bridge
        float startY = bridgeToDrop.transform.position.y;
        for (int x = 0; x <= 80;x++)
        {
            Vector3 pos = bridgeToDrop.transform.position;
            pos.y = Mathf.Lerp(startY, 0, x / 80f);
            bridgeToDrop.transform.position = pos;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        OverlayEffectsScript.instance.EndCutScene();
        yield return new WaitForSeconds(2f);
        playerInputManager.DecrementAllInputEnabled();
        DialogueTextManager.instance.QueueMessage("Your Path falls into Place \n                    \n Literally ");
        bridgeGate.SetActive(false);
    }

    public void EnableDash()
    {
        playerInputManager.dashInputEnabled--; 
    }
}
