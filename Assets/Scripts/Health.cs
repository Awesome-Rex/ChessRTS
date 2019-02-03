using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth;

    private Matter Matter_Comp;

    [SerializeField]
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

            transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetComponent<Image>().fillAmount = value / maxHealth;
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
        Matter_Comp = GetComponent<Matter>();

        if (GetComponent<Matter>() != null)
        {
            transform.Find("UI").Find("Canvas").GetComponent<RectTransform>().position = new Vector3(Matter_Comp.savedMinX, Matter_Comp.savedMaxY, 0f);
            Rect canvasRect = transform.Find("UI").Find("Canvas").GetComponent<RectTransform>().rect;
            canvasRect.size = new Vector2(Matter_Comp.savedMaxX - Matter_Comp.savedMinX, Matter_Comp.savedMaxY - Matter_Comp.savedMinY);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
