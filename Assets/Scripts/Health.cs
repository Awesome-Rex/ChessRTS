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

            if (inSelectedDamageArea)
            {
                resetReducedHealth();
            }
        }
    }

    //Temporary
    private bool inSelectedDamageArea;
    private int damageTaken;
    
    private Selectable old_TargetedObject;
    private Vector3 old_TargetedObject_Position;

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
        inSelectedDamageArea = true;
        damageTaken = damage;

        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
        //transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(Matter_Comp.savedMinX, Matter_Comp.savedMaxX, 0.5f) + (((1f - ((float)health / (float)maxHealth)) - 0.5f) * (Mathf.Abs(Matter_Comp.savedMaxX - Matter_Comp.savedMinX) + 1f)), 0f, 0f);
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0)/*.GetComponent<RectTransform>()*/.localPosition = new Vector3((Matter_Comp.savedMinX - 0.5f) + (Mathf.Abs((Matter_Comp.savedMinX - 0.5f) - (Matter_Comp.savedMaxX + 0.5f)) * ((float)health / (float)maxHealth)), 0f, 0f) * 100f;

        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(((float)damage / (float)maxHealth) * (Mathf.Abs((Matter_Comp.savedMaxX + 0.5f) - (Matter_Comp.savedMinX - 0.5f))/* + 1f*/) * 100f, 12.5f);

        old_TargetedObject = SelectionManagement.selectionManagement.targetedObject;
        old_TargetedObject_Position = SelectionManagement.selectionManagement.targetedObject.transform.position;
    }

    public void resetReducedHealth ()
    {
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0)/*.GetComponent<RectTransform>()*/.localPosition = new Vector3((Matter_Comp.savedMinX - 0.5f) + (Mathf.Abs((Matter_Comp.savedMinX - 0.5f) - (Matter_Comp.savedMaxX + 0.5f)) * ((float)health / (float)maxHealth)), 0f, 0f) * 100f;

        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(((float)damageTaken / (float)maxHealth) * (Mathf.Abs((Matter_Comp.savedMaxX + 0.5f) - (Matter_Comp.savedMinX - 0.5f))/* + 1f*/) * 100f, 12.5f);
    }

    public void disabledReducedHealth ()
    {
        inSelectedDamageArea = false;
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Matter_Comp = GetComponent<Matter>();

        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(0).GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
        transform.Find("UI").Find("Canvas").Find("MaxHealth").GetChild(1).GetComponent<TextMeshProUGUI>().text = health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (inSelectedDamageArea) {
            if (GameplayControl.gameplayControl.visualUnitAbility != GameplayControl.VisualUnitAbility.Damage || !SelectionManagement.selectionManagement.objectSelected || SelectionManagement.selectionManagement.targetedObject != old_TargetedObject || SelectionManagement.selectionManagement.targetedObject.transform.position != old_TargetedObject_Position)
            {
                Debug.Log("disabled reduced health . . . ");
                disabledReducedHealth();
            }
            else
            {
                old_TargetedObject = SelectionManagement.selectionManagement.targetedObject;
                old_TargetedObject_Position = SelectionManagement.selectionManagement.targetedObject.transform.position;
            }
        }
    }
}
