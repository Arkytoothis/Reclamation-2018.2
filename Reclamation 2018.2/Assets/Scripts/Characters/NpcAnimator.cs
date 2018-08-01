using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

namespace Reclamation.Characters
{
    public class NpcAnimator : MonoBehaviour
    {
        const float animationSmoothTime = 0.1f;

        public Animator animator;
        RVOController characterController;
        Npc npcController;

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null) Debug.LogError("animator == null");

            characterController = GetComponent<RVOController>();
            if (characterController == null) Debug.LogError("controller == null");

            npcController = GetComponent<Npc>();
            if (npcController == null) Debug.LogError("npcController == null");
        }

        void Update()
        {
            float speedPercent = 0f;

            if (characterController != null && npcController.gameObject.GetComponent<RichAI>().canMove == true)
                speedPercent = characterController.velocity.magnitude / npcController.moveSpeed;

            animator.SetFloat("speedPercent", speedPercent, animationSmoothTime, Time.deltaTime);
        }

        public void Interact()
        {
            animator.SetTrigger("interact");
        }

        public void Stop()
        {
            animator.SetFloat("speedPercent", 0);
        }

        public void Death()
        {
            animator.SetBool("isDead", true);
            animator.SetTrigger("die");
        }

        public void Revive()
        {
            animator.SetBool("isDead", false);
        }

        public void LevelUp()
        {
            animator.SetTrigger("levelUp");
        }

        public void Attack()
        {
            animator.SetTrigger("attack");
        }
    }
}