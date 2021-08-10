using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Director : MonoBehaviour
{
    public AnimationController playerAnimController;
    public InputManager playerInputManager;

    bool readyToGetUp = false;
    bool started = false;

    private void Update()
    {
        if(readyToGetUp && Input.GetKeyDown(KeyCode.E))
        {
            readyToGetUp = false;
            PlayerGetsUp();
        }
        if(!started && Input.GetMouseButtonDown(0))
        {
            started = true;
            Time.timeScale = 1;
        }
    }

    private void Awake()
    {
        playerAnimController.PlayAnimation("LyingDown");
        playerAnimController.SetHipSword(false);
        playerInputManager.dashInputEnabled++;
        playerInputManager.movementInputEnabled++;
        playerInputManager.actionInputEnabled++;

        Time.timeScale = 0.000001f;
    }

    private void Start()
    {
        StartCoroutine(Corout_StartingTextPrompt());
    }

    private IEnumerator Corout_StartingTextPrompt()
    {
        yield return new WaitForSeconds(5f);
        DialogueTextManager.instance.QueueMessage("Get Up.");
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
        DialogueTextManager.instance.QueueMessage("This place looks familiar           \n                          \n but a little different . . . ");
        DialogueTextManager.instance.QueueMessage("\n            [WASD] to move");
        playerInputManager.movementInputEnabled--;
    }
}
