using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable
{

    PlayerManager playerManager;

    CharacterStats myStats;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        myStats = GetComponent<CharacterStats>();
    }

    //when the player interacts with this enemy
    public override void Interact()
    {
        base.Interact();
        // attack
        // get reference to charcombat on player
        CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();

        if(playerCombat != null)
        {
            //the player attacks, and the enemy's stats are passed in so that they take damage
            playerCombat.Attack(myStats);
        }
    }
}
