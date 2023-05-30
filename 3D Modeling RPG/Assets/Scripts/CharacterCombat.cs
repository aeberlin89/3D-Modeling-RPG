using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    //setup attack speed so that there's a cooldown time between attacks
    //for now, we're setting the value here, but we can derive it from the character's
    //stats once they get set up so that each character can have different attack speeds
    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    const float combatCooldown = 5f;
    float lastAttackTime;

    public float attackDelay = 0.6f;


    public bool InCombat { get; private set; }
    //make attack delegate to notify animator
    public event System.Action OnAttack;
    
    CharacterStats myStats;

    private void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if(Time.time - lastAttackTime > combatCooldown)
        {
            InCombat = false;
        }
    }

    //when this character attacks, the stats of it's target are passed in so the
    //damage can be determined and applied
    public void Attack(CharacterStats targetStats)
    {
        //this method will be called inside the interact method in the enemy
        // --which is a type of interactable--
        //so the enemy (or target) will pass in it's stats to take the damage.

        //similarly, when the player is attacked, this method will be called
        //and pass in the player's stats to take the damage.


        if (attackCooldown <= 0f)
        {
            //call take damage on the target's stats and pass in the damage value
            //from this character. The modifiers are dealt with inside takeDamage.
            StartCoroutine(DoDamage(targetStats, attackDelay));

            OnAttack?.Invoke();

            attackCooldown = 1 / attackSpeed;        //the greater the attack speed, the smaller the cooldown
            InCombat = true;
            lastAttackTime = Time.time;
        }

    }

    IEnumerator DoDamage(CharacterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);

        stats.TakeDamage(myStats.damage.GetValue());
        if(stats.currentHealth <= 0)
        {
            InCombat = false;
        }

    }
}
