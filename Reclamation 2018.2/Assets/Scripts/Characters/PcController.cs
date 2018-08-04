using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Gui;
using Reclamation.Equipment;
using Reclamation.Misc;
using Reclamation.Props;
using Pathfinding;
using Pathfinding.RVO;

namespace Reclamation.Characters
{
    [System.Serializable]
    public class PcController : CharacterController
    {
        public GameObject model;
        public new Light light;

        public float moveSpeed = 3f;
        public float turnSpeed = 100f;
        public float perceptionRadius = 10f;
        public float attackDistance = 1f;
        public float repeatAttackDelay = 1f;

        public GameObject target = null;

        public AIDestinationSetter destinationSetter;
        public RichAI pathfinder;
        public RVOController rvo;

        public bool isFighting = false;

        [SerializeField]
        private PcAnimator animator;
        [SerializeField] private PcData pcData;
        public PcData PcData { get { return pcData; } }
        public new AttributeManager Attributes { get { return pcData.attributes; } }

        // The amount of time it takes for the agent to be able to attack again
        // The last time the agent attacked
        private float lastAttackTime;

        [SerializeField] private CharacterRenderer pcRenderer = null;
        [SerializeField] private Damagable targetDamagable = null;
        [SerializeField] private Interactable targetInteractable = null;

        [SerializeField] private IAttack currentAttack;
        [SerializeField] private IDamageable currentDefense;
        public IDamageable CurrentDefense { get { return currentDefense; } }

        public event OnDeath onDeath;
        public event OnRevive onRevive;
        public event OnInteract onInteract;
        public event OnAttack onAttack;

        public delegate void OnLevelUp();
        public event OnLevelUp onLevelUp;

        void Awake()
        {
            destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
            rvo = gameObject.GetComponent<RVOController>();
            pathfinder = gameObject.GetComponent<RichAI>();
            currentAttack = gameObject.GetComponent<IAttack>();
            currentDefense = gameObject.GetComponent<IDamageable>();

            if (currentAttack == null)
            {
                Debug.Log("currentAttack == null");
            }
            if (currentDefense == null)
            {
                Debug.Log("currentDefense == null");
            }
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
            if (CheckIsAlive() == true && target != null)
            {
                targetDamagable = CheckDamagable(target);
                targetInteractable = CheckInteractable(target);
                
                if (targetDamagable != null)
                {
                    ProcessAttack(targetDamagable);
                }
                else if (targetInteractable != null)
                {
                    ProcessInteraction(targetInteractable);
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
            if (target.GetComponent<CharacterController>() != null && target.GetComponent<CharacterController>().CheckIsAlive() == true)
            {
                if (CheckRange(target) == true && CheckTiming(target) == true)
                {
                    isFighting = true;
                    lastAttackTime = Time.time;
                    animator.Attack();
                    currentAttack.Attack(target);
                    CanMove(false);
                }
                else if (CheckRange(target) == false)
                {
                    isFighting = false;
                    CanMove(true);
                    MoveTo(target);
                }
            }
        }

        public void ProcessInteraction(Interactable interactable)
        {
            if (CanInteract(interactable) == true)
            {
                animator.Interact();
                interactable.GetComponent<Interactable>().Interact(gameObject);
                SetInteractionTarget(null);
            }
        }

        public void SetPcData(PcData pcData, GameObject model)
        {
            this.pcData = pcData;
            this.pcData.onLevelUp += animator.LevelUp;

            onDeath += animator.Death;
            onRevive += animator.Revive;
            onAttack += Attack;
            onInteract += Interact;
            onLevelUp += LevelUp;
            onLevelUp += pcData.LevelUp;

            ItemData item = this.pcData.inventory.EquippedItems[(int)EquipmentSlot.Right_Hand];

            if (item != null)
            {
                attackDistance = (float)item.WeaponData.Attributes[(int)WeaponAttributes.Range].Value;
                repeatAttackDelay = (float)item.RecoveryTime;
            }

            SetModel(model);
            pcRenderer.LoadEquipment(this.pcData);

            currentDefense.SetController(this.pcData);
            currentAttack.SetController(this.pcData);
            this.pcData.attributes.controller = this;
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
            if (target == null) return false;

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= attackDistance)
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

            if (target.GetComponent<CharacterController>().CheckIsAlive() == false) return false;

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

        public void MoveTo(Transform target)
        {
            destinationSetter.target = target;
        }

        public void FaceTarget()
        {
            if(CheckIsAlive() == true && target != null)
            {
                if (target.transform.position - transform.position != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime * 10);
                }
            }
        }

        public void CanMove(bool canMove)
        {
            rvo.locked = !canMove;
        }

        public bool CanInteract(Interactable interactable)
        {
            if (interactable == null)
            {
                return false;
            }

            bool canInteract = false;
            float distance = Vector3.Distance(transform.position, interactable.gameObject.transform.position);

            if (distance <= 1f)
            {
                canInteract = true;
            }

            return canInteract;
        }

        public override bool CheckIsAlive()
        {
            return !pcData.isDead;
        }

        public override void Death()
        {
            destinationSetter.enabled = false;
            pathfinder.enabled = false;
            rvo.enabled = false;
            CanMove(false);
            target = null;
            pcData.isDead = true;
            onDeath();
            MessageSystem.instance.AddMessage(pcData.name.FirstName + " has died");
        }

        public override void Revive()
        {
            destinationSetter.enabled = true;
            pathfinder.enabled = true;
            rvo.enabled = true;
            CanMove(true);
            target = null;
            pcData.isDead = false;
            onRevive();
            MessageSystem.instance.AddMessage(pcData.name.FirstName + " has revived");
        }

        public override void Interact()
        {
            onInteract();
            //MessageSystem.instance.AddMessage(name.FirstName + " has interacting");
        }

        public override void Attack()
        {
            onAttack();
            //MessageSystem.instance.AddMessage(name.FirstName + " has attacking");
        }

        public void LevelUp()
        {
            onLevelUp();
            //Debug.Log(Name.FirstName + " has gained a level!");
        }

        public override void ModifyAttribute(AttributeType type, int attribute, int value)
        {
            pcData.attributes.ModifyAttribute(type, attribute, value);
        }
    }
}