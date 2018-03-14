using UnityEngine;
using XmlLoader;

public class playerscript : MonoBehaviour
{
    public Vector2 move;
    private Rigidbody2D rgBD;
    public PlayerData playerData;
    public float maxWeight;
    public float currentWeight;
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int maxStamina;
    public int currentStamina;
    public int maxExp;
    public int currentExp;
    public bool isCaster;
    public int level;
    public int statPoints;
    public string playerClass;
    public int speed;
    public float dodge;
    public float crit;
    public int minDef;
    public int maxDef;
    public int minAtk;
    public int maxAtk;
    public int strength;
    public int agility;
    public int intellect;
    public int charisma;
    public int vitality;

    private void Start()
    {
        strength = 5;
        agility = 5;
        intellect = 5;
        vitality = 5;
        charisma = 5;
        level = 1;
        currentExp = 0;
        maxExp = 100;
        minAtk = 1;
        maxAtk = 3;
        speed = 10;
        maxWeight = 60 + strength * 2;
        maxExp = level * 100;
        maxHealth = vitality * 15;
        maxMana = intellect * 10 + vitality * 3;
        maxStamina = agility * 10 + vitality * 3;
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
        rgBD = GetComponent<Rigidbody2D>();
        // SaveManager.LoadData(1);
    }


    public void AddExp(int expAmount)
    {
        int addAmount = expAmount;
        while (addAmount > 0)
        {
            //if add more than needed for levelup
            if (addAmount >= (maxExp - currentExp))
            {
                level++;
                statPoints += 5;
                addAmount -= (maxExp - currentExp);
                currentExp = 0;
                maxExp = level * 100;

            }
            //if add less than needed for levelup
            if (addAmount < (maxExp - currentExp))
            {
                currentExp += addAmount;
                addAmount = 0;
            }
        }
    }

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal") * 8;
        if (move.x < 0)
        { gameObject.GetComponent<SpriteRenderer>().flipX = true; }

        if (move.x > 0)
        { gameObject.GetComponent<SpriteRenderer>().flipX = false; }
    }

    void FixedUpdate()
    {
        maxHealth = vitality * 15;
        maxMana = intellect * 10 + vitality * 3;
        maxStamina = agility * 10 + vitality * 3;
        rgBD.velocity = move;
    }
}