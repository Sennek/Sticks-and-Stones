using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XmlLoader;

public class CompanionScript : MonoBehaviour
{
    public Vector2 move;
    //misc
    public int id;
    public bool isCaster;
    public float addWeight;
    //res points
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int maxStamina;
    public int currentStamina;
    //level
    public int maxExp;
    public int currentExp;
    public int level;
    public int statPoints;
    public string playerClass;
    //fight stats
    public int speed;
    public float dodge;
    public float crit;
    public int minDef;
    public int maxDef;
    public int minAtk;
    public int maxAtk;
    //stats
    public int strength;
    public int agility;
    public int intellect;
    public int charisma;
    public int vitality;
    //party vars
    public bool isInParty;
    public int partyPos;
    public playerscript player;
    public PartyScript partyScript;

    public float dist;
    public void Start()
    {
        isInParty = false;
        player = GameObject.Find("Player").GetComponent<playerscript>();
        partyScript = GameObject.Find("Player").GetComponent<PartyScript>();
        addWeight = 60 + strength * 2;
    }

    // public void OnMouseDown()
    //{
    //    if (isInParty)
    //     {
    //        partyScript.RemoveFromParty(gameObject);
    //     }
    //   else 
    //   {
    //       partyScript.AddToParty(gameObject);
    //   }
    // }
    public void Update()
    {
        dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > 2 * partyPos && isInParty)
        {
            if (!player.gameObject.GetComponent<SpriteRenderer>().flipX && partyPos != 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                transform.position = player.transform.position + new Vector3(-2 * partyPos, 0, 0);
            }
            if (player.gameObject.GetComponent<SpriteRenderer>().flipX && partyPos != 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                transform.position = player.transform.position + new Vector3(2 * partyPos, 0, 0);
            }
        }
    }
}
