using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyScript : MonoBehaviour
{
    public int[] partyPos;
    public GameObject[] partyMember;
    public CompanionScript companionScript;
    public playerscript playerScript;
    void Start()
    {
        partyPos = new int[4];
        for (int i = 0; i < partyPos.Length; i++)
        {
            partyPos[i] = i;
        }
        partyMember = new GameObject[4];
        partyMember[0] = gameObject;
        playerScript = gameObject.GetComponent<playerscript>();
    }

    public void AddToParty(GameObject unit)
    {
        Debug.Log("adding");
        for (int i = 1; i < partyMember.Length; i++)
        {
            if (partyMember[i] == null)
            {
                partyMember[i] = unit;
                unit.GetComponent<CompanionScript>().partyPos = partyPos[i];
                unit.GetComponent<CompanionScript>().isInParty = true;
                playerScript.maxWeight += unit.GetComponent<CompanionScript>().addWeight;
                break;
            }
        }
    }

    public void ReshuffleParty()
    {
        for (int i = 1; i < partyMember.Length; i++)
        {
            if ((partyMember[i] = null) && (partyMember[i + 1] != null))
            {
                partyMember[i] = partyMember[i + 1];
            }
        }
    }

    public void RemoveFromParty(GameObject unit)
    {
        for (int i = 1; i < partyMember.Length; i++)
        {
            if (partyMember[i] == unit)
            {
                unit.GetComponent<CompanionScript>().isInParty = false;
                partyMember[i] = null;
                ReshuffleParty();
            }
        }
    }

    void Update()
    {

    }
}
