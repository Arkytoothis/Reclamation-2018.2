using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Props;
using Reclamation.Gui;
using Reclamation.Misc;
using Pathfinding;
using Pathfinding.RVO;

namespace Reclamation.Characters
{
    public class NpcController : CharacterController
    {
        public float moveSpeed = 1f;
        public float turnSpeed = 1f;
        public float perceptionRadius = 10f;
        public float rangedDistance = 5f;
        public float meleeDistance = .11f;

        [SerializeField] GameObject target = null;

        [SerializeField] AIDestinationSetter destinationSetter;
        [SerializeField] RichAI pathfinder;
        [SerializeField] RVOController rvo;
        [SerializeField] Highlighter highlighter;
        [SerializeField] NpcAnimator animator;
        [SerializeField] new BoxCollider collider;

        private NpcData npcData;
        public NpcData NpcData { get { return npcData; } }
        public new AttributeManager Attributes { get { return npcData.attributeManager; } }

        [SerializeField] IAttack currentAttack;
        [SerializeField] IDamageable currentDefense;

        [SerializeField] NpcGui gui;

        new SkinnedMeshRenderer renderer;

        public event OnDeath onDeath;
        public event OnRevive onRevive;
        public event OnInteract onInteract;
        public event OnAttack onAttack;

        void Awake()
        {
            animator = GetComponent<NpcAnimator>();
            destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
            rvo = gameObject.GetComponent<RVOController>();
            pathfinder = gameObject.GetComponent<RichAI>();
            highlighter = gameObject.GetComponent<Highlighter>();
            collider = gameObject.GetComponent<BoxCollider>();

            renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

            currentAttack = gameObject.GetComponent<IAttack>();
            currentDefense = gameObject.GetComponent<IDamageable>();

            material = renderer.material;
        }

        void Start()
        {
            npcData = new NpcData(NpcGenerator.Generate(NpcType.Enemy, Species.Undead, 1));
            
            onDeath += animator.Death;
            onRevive += animator.Revive;
            onAttack += Attack;
            onInteract += Interact;

            gui = gameObject.GetComponentInChildren<NpcGui>();

            if (gui != null)
            {
                gui.SpawnHealthBar();
                gui.SetData(npcData);
            }

            currentDefense.SetController(npcData);
            currentAttack.SetController(npcData);
            npcData.attributeManager.controller = this;
        }

        void Update()
        {
            if (CheckIsAlive() == true && target != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
            else if (CheckIsAlive() == false)
            {
                //ProcessDissolve();
            }
        }

        void OnDrawGizmosSelected()
        {
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, perceptionRadius);

            //Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(transform.position, rangedDistance);

            //Gizmos.color = Color.cyan;
            //Gizmos.DrawWireSphere(transform.position, meleeDistance);
        }

        public void CanMove(bool canMove)
        {
            rvo.locked = !canMove;
        }

        public bool CanAttack(GameObject target)
        {
            bool canAttack = true;

            return canAttack;
        }

        public override bool CheckIsAlive()
        {
            return !npcData.isDead;
        }

        public float start = 0;
        public float destination = 1;
        public float threshold = 0.1f;
        public float speed = 5;
        float dissolve = 0;

        void ProcessDissolve()
        {
            Hashtable param = new Hashtable();
            param.Add("from", 0.0f);
            param.Add("to", 1.0f);
            param.Add("time", 5.0f);
            param.Add("onupdate", "TweenedSomeValue");
            iTween.ValueTo(gameObject, param);
        }

        public Material material;

        public void TweenedSomeValue(float val)
        {
            dissolve = val;
            material.SetFloat("_DissolveCutoff", dissolve);
        }

        public override void Death()
        {
            npcData.isDead = true;
            target = null;

            destinationSetter.enabled = false;
            pathfinder.enabled = false;
            rvo.enabled = false;
            highlighter.enabled = false;
            collider.enabled = false;

            gui.gameObject.SetActive(false);
            CanMove(false);
            ProcessDissolve();
            
            Destroy(this.gameObject, 5.1f);
            onDeath();

            MessageSystem.instance.AddMessage(npcData.name.FirstName + " has died");
        }

        public override void Revive()
        {
            npcData.isDead = false;

            destinationSetter.enabled = true;
            pathfinder.enabled = true;
            rvo.enabled = true;
            highlighter.enabled = true;
            collider.enabled = true;

            CanMove(true);
            onRevive();
            MessageSystem.instance.AddMessage(npcData.name.FirstName + " has revived");
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

        public override void ModifyAttribute(AttributeType type, int attribute, int value)
        {
            npcData.attributeManager.ModifyAttribute(type, attribute, value);
        }
    }
}