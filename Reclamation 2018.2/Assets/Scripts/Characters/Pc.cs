using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Reclamation.Characters;
using Reclamation.Misc;
using Reclamation.Props;
using Pathfinding;
using Pathfinding.RVO;

namespace Reclamation.Encounter
{
    [System.Serializable]
    public class Pc : MonoBehaviour
    {
        public GameObject model;
        public new Light light;

        public float moveSpeed = 3f;
        public float turnSpeed = 100f;
        public float perceptionRadius = 10f;
        public float rangedDistance = 5f;
        public float meleeDistance = 1f;

        public GameObject target = null;

        public AIDestinationSetter destinationSetter;
        public RichAI pathfinder;
        public RVOController rvo;

        public bool isAttacking = false;

        [SerializeField]
        private PcAnimator animator;
        private PcData pcData;

        // The furthest distance that the agent is able to attack from
        public float attackDistance = 1f;
        // The amount of time it takes for the agent to be able to attack again
        public float repeatAttackDelay = 1f;
        // The last time the agent attacked
        private float lastAttackTime;

        private CharacterRenderer pcRenderer = null;
        private Damagable targetDamagable = null;
        private Interactable targetInteractable = null;

        void Awake()
        {
            destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
            rvo = gameObject.GetComponent<RVOController>();
            pathfinder = gameObject.GetComponent<RichAI>();
        }

        void Start()
        {
            destinationSetter.target = null;
            InvokeRepeating("ProcessAi", 2f, 0.1f);
        }

        void LateUpdate()
        {
            FaceTarget();
        }

        void ProcessAi()
        {
            if (target != null)
            {
                targetDamagable = CheckDamagable(target);
                targetInteractable = CheckInteractable(target);
                
                if (targetInteractable != null)
                {
                    ProcessInteraction(targetInteractable);
                }
                else if (targetDamagable != null)
                {
                    ProcessAttack(targetDamagable);
                }
            }
        }

        public void SetModel(GameObject model)
        {
            this.model = model;
            pcRenderer = gameObject.GetComponentInChildren<CharacterRenderer>();
        }

        public void ProcessAttack(Damagable damagable)
        {
            if (CheckRange(target) == true && CheckTiming(target) == true)
            {
                isAttacking = true;
                lastAttackTime = Time.time;
                animator.Attack();
                CanMove(false);
            }
            else if (CheckRange(target) == false)
            {
                CanMove(true);
                MoveTo(target);
            }
        }

        public void ProcessInteraction(Interactable interactable)
        {
            if (CanInteract(target) == true)
            {
                animator.Interact();
                SetInteractionTarget(null);
                interactable.Interact(gameObject);
            }
        }

        public void SetPcData(PcData pc, GameObject model)
        {
            pcData = pc;
            pcData.onDeath += animator.Death;
            pcData.onRevive += animator.Revive;
            pcData.onLevelUp += animator.LevelUp;

            SetModel(model);
            pcRenderer.LoadEquipment(pcData);
            //animator.animator.avatar = this.model.GetComponent<Animator>().avatar;
            //animator.animator.runtimeAnimatorController = this.model.GetComponent<Animator>().runtimeAnimatorController;
        }

        public void StopAnimations()
        {
            animator.Stop();
        }

        public void SetAttackTarget(GameObject target)
        {
            this.target = target;
        }

        public void SetInteractionTarget(GameObject target)
        {
            this.target = target;
        }

        public bool CheckRange(GameObject target)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= meleeDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTiming(GameObject target)
        {
            bool canAttack = lastAttackTime + repeatAttackDelay < Time.time;
            return canAttack;
        }

        public Damagable CheckDamagable(GameObject target)
        {
            Damagable damagable = target.GetComponent<Damagable>();
            return damagable;
        }

        public Interactable CheckInteractable (GameObject target)
        {
            Interactable interactable = target.GetComponent<Interactable>();
            return interactable;
        }

        public bool CheckAttack(GameObject target)
        {
            if (target == null) return false;

            if(CheckRange(target) == false || CheckTiming(target) == false || CheckDamagable(target) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void MoveTo(GameObject target)
        {
            destinationSetter.target = target.transform;
        }

        public void FaceTarget()
        {
            if(target != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime * 10);
            }
        }

        public void CanMove(bool canMove)
        {
            rvo.locked = !canMove;
        }

        public bool CanInteract(GameObject go)
        {
            Interactable interactable = go.GetComponent<Interactable>();
            if (interactable == null)
            {
                return false;
            }

            bool canInteract = false;
            float distance = Vector3.Distance(transform.position, go.transform.position);

            if (distance <= 1f)
            {
                canInteract = true;
            }

            return canInteract;
        }
    }
}