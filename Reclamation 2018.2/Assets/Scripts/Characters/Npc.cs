using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Props;
using Reclamation.Gui;
using Pathfinding;
using Pathfinding.RVO;

namespace Reclamation.Characters
{
    public class Npc : CharacterController
    {
        public float moveSpeed = 1f;
        public float turnSpeed = 1f;
        public float perceptionRadius = 10f;
        public float rangedDistance = 5f;
        public float meleeDistance = .11f;

        public GameObject target = null;

        public AIDestinationSetter destinationSetter;
        public RichAI pathfinder;
        public RVOController rvo;
        public Container container;

        [SerializeField]
        private NpcAnimator animator;
        private NpcData npcData;
        public NpcData NpcData { get { return npcData; } }

        [SerializeField] IAttack currentAttack;
        [SerializeField] IDamageable currentDefense;

        [SerializeField] NpcGui gui;

        void Awake()
        {
            animator = GetComponent<NpcAnimator>();
            destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
            rvo = gameObject.GetComponent<RVOController>();
            pathfinder = gameObject.GetComponent<RichAI>();
            currentAttack = gameObject.GetComponent<IAttack>();
            currentDefense = gameObject.GetComponent<IDamageable>();
            container = gameObject.GetComponent<Container>();
            container.enabled = false;

            if (currentAttack == null)
            {
                Debug.Log("currentAttack == null");
            }
            if (currentDefense == null)
            {
                Debug.Log("currentDefense == null");
            }

            NpcData npc = NpcGenerator.Generate(NpcType.Enemy, Species.Undead, 1);
            SetNpcData(npc);
        }

        void Start()
        {
            //InvokeRepeating("FindTarget", 2f, 0.2f);
            //InvokeRepeating("UpdateAi", 2f, 0.1f);
        }

        public void SetNpcData(NpcData npc)
        {
            npcData = npc;
            npcData.onDeath += animator.Death;
            npcData.onDeath += OnDeath;
            npcData.onRevive += animator.Revive;
            npcData.onAttack += animator.Attack;

            currentDefense.SetCharacterData(this.npcData);
            currentAttack.SetCharacterData(this.npcData);

            gui = gameObject.GetComponentInChildren<NpcGui>();
            if (gui != null)
            {
                gui.SpawnHealthBar();
                gui.SetData(ref npcData);
            }
        }

        void Update()
        {
            if (CheckIsAlive() == false || target == null) return;

            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
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

        public void OnDeath()
        {
            Debug.Log(npcData.name.FirstName + " has died");
            destinationSetter.enabled = false;
            pathfinder.enabled = false;
            rvo.enabled = false;
            CanMove(false);
            Destroy(gui.barInstance);
            target = null;
            container.enabled = true;
        }

        public override bool CheckIsAlive()
        {
            return !npcData.isDead;
        }
    }
}