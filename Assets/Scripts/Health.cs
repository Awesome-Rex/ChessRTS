using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxHealth;

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

            transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetComponent<Image>().fillAmount = (float)_health / (float)maxHealth;
            transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(1).GetComponent<TextMeshProUGUI>().text = _health.ToString();
        }
    }

    private Matter Matter_Comp;

    public void takeDamage (int damageValue)
    {
        health -= damageValue;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void showReducedHealth (int damage)
    {
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(Matter_Comp.savedMinX, Matter_Comp.savedMaxX, 0.5f) + (((1f - ((float)health / (float)maxHealth)) - 0.5f) * ((Matter_Comp.savedMaxX - Matter_Comp.savedMinX) + 1f)), 0f, 0f);

        /*Rect reducedHealthRect = transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<RectTransform>().rect;
        reducedHealthRect.size = new Vector2((float)damage / (float)maxHealth, 12)*/
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((((float)damage / (float)maxHealth) * 100f) * ((Matter_Comp.savedMaxX - Matter_Comp.savedMinX) + 1f), 12.5f);
    }
    public void disabledReducedHealth ()
    {
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Matter_Comp = GetComponent<Matter>();

        if (GetComponent<Matter>() != null)
        {
            transform.Find("UI").Find("Canvas").GetComponent<RectTransform>().localPosition = new Vector3(Matter_Comp.savedMinX - 0.5f, Matter_Comp.savedMaxY + 0.5f, 0f);
            Rect canvasRect = transform.Find("UI").Find("Canvas").GetComponent<RectTransform>().rect;
            canvasRect.size = new Vector2((Matter_Comp.savedMaxX - Matter_Comp.savedMinX) + 0.5f, (Matter_Comp.savedMaxY - Matter_Comp.savedMinY) + 0.5f);
        }

        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(1).GetComponent<TextMeshProUGUI>().text = health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
