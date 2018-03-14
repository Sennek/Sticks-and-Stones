using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public int id;
    public int range;
    public int maxhp;
    public int currenthp;
    public int minatk;
    public int maxatk;
    public int def;
    public int speed;
    public bool isEnemy;
    public bool actionComplete;
    public CombatTest combatScript;
    public Vector3 unitPosition;
    public int unitPositionNumber;


    void Start()
    {
        combatScript = GameObject.Find("levelmanager").GetComponent<CombatTest>();
    }

    public void OnMouseDown()
    {
        if ((!combatScript.unitsAction[0].GetComponent<CombatUnit>().isEnemy) && (isEnemy) && (!combatScript.unitsAction[0].GetComponent<CombatUnit>().actionComplete))
        {
            combatScript.unitsAction[0].GetComponent<CombatUnit>().actionComplete = true;
            StartCoroutine(combatScript.Attack(gameObject));
        }
    }


    public void Update()
    {
      
    }

}
