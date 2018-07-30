using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Characters;
using Reclamation.Encounter;
using Pathfinding;
using Pathfinding.RVO;

public class EncounterNpcController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float turnSpeed = 1f;
    public float perceptionRadius = 10f;
    public float rangedDistance = 5f;
    public float meleeDistance = .11f;

    public GameObject target = null;

    public AIDestinationSetter destinationSetter;
    public RVOController rvo;

    [SerializeField]
    private PcAnimator animator;
    private NPC npcData;

    void Start()
    {
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        rvo = gameObject.GetComponent<RVOController>();

        //InvokeRepeating("FindTarget", 2f, 0.2f);
        //InvokeRepeating("UpdateAi", 2f, 0.1f);
    }

    public void SetPcData(NPC npc)
    {
        npcData = npc;
        npcData.onDeath += animator.Death;
        npcData.onRevive += animator.Revive;
        npcData.onLevelUp += animator.LevelUp;
    }

    //public void FindTarget()
    //{
    //    for (int i = 0; i < EncounterManager.instance.pcs.Count; i++)
    //    {
    //        GameObject pc = EncounterManager.instance.pcs[i];
    //        float distance = Vector3.Distance(transform.position, pc.transform.position);
    //        if (distance <= perceptionRadius)
    //        {
    //            Debug.Log("I found a target");
    //            target = pc;
    //        }
    //    }

    //    if (target != null)
    //    {
    //        CancelInvoke("FindTarget");
    //        destinationSetter.target = target.transform;
    //        rvo.locked = false;
    //    }
    //}

    //void UpdateAi()
    //{
    //    if (target == null) return;

    //    float distance = Vector3.Distance(transform.position, target.transform.position);

    //    if (distance > perceptionRadius)
    //    {
    //        Debug.Log("I lost my target");
    //        target = null;
    //        rvo.locked = true;

    //        InvokeRepeating("FindTarget", 2f, 0.25f);
    //    }
    //    else
    //    {
    //        rvo.locked = false;

    //        if (distance < meleeDistance)
    //        {
    //            Debug.Log("melee attack");
    //            rvo.locked = true;
    //        }
    //        else if (distance < rangedDistance)
    //        {
    //            Debug.Log("ranged attack");
    //            rvo.locked = true;
    //        }
    //    }

    //    if (target != null)
    //    {
    //    }
    //}

    void Update()
    {
        if (target == null) return;

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

    public bool CanAttack(GameObject target)
    {
        bool canAttack = true;

        return canAttack;
    }
}