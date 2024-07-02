using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : BaseController
{
    protected float speed = 1.0f;

    public int HP { get; set; } = 100;
    public int maxHP { get; set; } = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDamaged(BaseController attacker, int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
    
    }
}
