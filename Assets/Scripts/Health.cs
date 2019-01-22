using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;

    private int _health;
    public int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (value > maxHealth)
            {
                _health = maxHealth;
            }
        }
    }

    public void takeDamage (int damageValue)
    {
        health -= damageValue;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
