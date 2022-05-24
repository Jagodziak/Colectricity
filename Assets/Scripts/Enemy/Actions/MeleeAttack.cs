using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleAI))]
public class MeleeAttack : ActionBase
{
    private SimpleAI sai;

    private float sinceLast;

    void Start()
    {
        sai = GetComponent<SimpleAI>();
    }

    void OnEnable()
    {
        sinceLast = timeBetween;
    }

    void Update()
    {
        sinceLast -= Time.deltaTime;
        sinceLast = sinceLast < 0 ? 0 : sinceLast;

        if (sai.target == null)
        {
            return;
        }

        if (sinceLast > 0)
        {
            return;
        }

        if (Vector3.Distance(sai.target.transform.position, transform.position) > radius)
        {
            return;
        }

        Attack();
    }

    void Attack()
    {
        sinceLast = timeBetween;
        sai.modelAnimator.SetTrigger("attacking");
        sai.target.ReceiveDamage(units);
    }
}
