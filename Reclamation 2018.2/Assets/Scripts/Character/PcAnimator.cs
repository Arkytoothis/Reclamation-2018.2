using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PcAnimator : MonoBehaviour
{
    const float animationSmoothTime = 0.1f;

    Animator animator;
    CharacterController controller;
    PcMotor motor;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        if (controller == null) Debug.LogError("controller == null");

        motor = GetComponent<PcMotor>();

        if (motor == null)
        {
            Debug.LogError("EncounterPcMotor == null");
        }

    }

    void Update()
    {
        float speedPercent = 0f;

        if (controller != null && motor != null) speedPercent = controller.velocity.magnitude / motor.speed;

        animator.SetFloat("speedPercent", speedPercent, animationSmoothTime, Time.deltaTime);
    }

    public void WorldInteraction()
    {
        animator.SetTrigger("interact");
    }
}