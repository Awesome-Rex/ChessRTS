using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string name;

    public int addedGems;
    public int addedMoney;

    [TextArea] public string description;

    public void collect() {
        Game.currentGame.gems += addedGems;
        Game.currentGame.money += addedMoney;

        Destroy(gameObject);
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
