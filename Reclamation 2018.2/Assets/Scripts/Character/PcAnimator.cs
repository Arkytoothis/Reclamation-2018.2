using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PcAnimator : MonoBehaviour
{
    const float animationSmoothTime = 0.1f;

    public Animator animator;
    CharacterController characterController;
    EncounterPcController pcController;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("animator == null");

        characterController = GetComponent<CharacterController>();
        if (characterController == null) Debug.LogError("controller == null");

        pcController = GetComponent<EncounterPcController>();
        if (pcController == null) Debug.LogError("pcController == null");
    }

    void Update()
    {
        float speedPercent = 0f;

        if (characterController != null && pcController.gameObject.GetComponent<AIPath>().canMove == true)
            speedPercent = characterController.velocity.magnitude / pcController.moveSpeed;

        animator.SetFloat("speedPercent", speedPercent, animationSmoothTime, Time.deltaTime);
    }

    public void WorldInteraction()
    {
        animator.SetTrigger("interact");
    }

    public void Stop()
    {
        animator.SetFloat("speedPercent", 0);
    }
}