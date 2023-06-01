using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;

    public event System.Action<int, int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }


    private void Update()
    {
        if(Keyboard.current.tKey.wasPressedThisFrame)
        //if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        //deal with modifiers to get final damage taken
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);


        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        OnHealthChanged?.Invoke(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Die in some way
        //This method is meant to be overwritten
        Debug.Log(transform.name + " died.");
    }
}
