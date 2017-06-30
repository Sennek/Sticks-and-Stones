using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelmanager : MonoBehaviour
{
    //backgrounds
    GameObject bg1;
    private Vector3 bg1v = new Vector3(-36, -0.7f, 0);
    GameObject bg2;
    private Vector3 bg2v = new Vector3(-27, -0.7f, 0);
    GameObject bg3;
    private Vector3 bg3v = new Vector3(-18, -0.7f, 0);
    GameObject bg4;
    private Vector3 bg4v = new Vector3(-9, -0.7f, 0);
    GameObject bg5;
    private Vector3 bg5v = new Vector3(0, -0.7f, 0);
    GameObject bg6;
    private Vector3 bg6v = new Vector3(9, -0.7f, 0);
    GameObject bg7;
    private Vector3 bg7v = new Vector3(18, -0.7f, 0);
    GameObject bg8;
    private Vector3 bg8v = new Vector3(27, -0.7f, 0);
    GameObject bg9;
    private Vector3 bg9v = new Vector3(36, -0.7f, 0);

    //player object
    GameObject player;

    //structures
    GameObject gateW;
    GameObject gateE;
    //GameObject house1;
    //GameObject house2;
    //GameObject well;
    //GameObject loh;

    private Vector3 gateWv = new Vector3(-13.5f,0,-2);
    private Vector3 gateEv = new Vector3(4.5f,0,-2);
    private Vector3 house1v = new Vector3(-8.5f,0,-1);
    private Vector3 house2v = new Vector3(0.3f, 0,-1);
    private Vector3 wellv = new Vector3(-4.2f,0,-1);
    private Vector3 lohv = new Vector3(-2.22f,-1.95f,-1);

    //misc objects
    GameObject test1;
    private Vector3 test1v = new Vector3(12,0,-1);
    GameObject test2;
    private Vector3 test2v = new Vector3(4, 0, -1);
    GameObject test3;
    private Vector3 test3v = new Vector3(-8, 0, -1);
    GameObject test4;
    private Vector3 test4v = new Vector3(-24, 0, -1);
    GameObject test5;
    private Vector3 test5v = new Vector3(9, 0, -1);

    //misc var
    private float scSize = 10;

    private void Start()
    {
        player = GameObject.Find("player");
    }

    // queue wat
    private Vector3 heading;
    public void distance()
    {
        Queue<Vector3> distancev = new Queue<Vector3>();
        distancev.Enqueue(test1v);
        distancev.Enqueue(test2v);
        distancev.Enqueue(test3v);
        distancev.Enqueue(test4v);
        distancev.Enqueue(test5v);
    }

    // Update is called once per frame
    void Update()
    {
        //spawn structures

        if (Vector3.Distance(gateWv, player.GetComponent<Transform>().position) < scSize && GameObject.Find("gateWest") == null)
        {
            gateW = new GameObject("gateWest");
            SpriteRenderer texture = gateW.AddComponent<SpriteRenderer>();
            GameObject.Find("gateWest").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("gates");
            GameObject.Find("gateWest").transform.position = gateWv;
        }
        if (Vector3.Distance(gateWv, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(gateW);
        }

        if (Vector3.Distance(gateEv, player.GetComponent<Transform>().position) < scSize && GameObject.Find("gateEast") == null)
        {
            gateE = new GameObject("gateEast");
            SpriteRenderer texture = gateE.AddComponent<SpriteRenderer>();
            GameObject.Find("gateEast").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("gates");
            GameObject.Find("gateEast").transform.position = gateEv;
            GameObject.Find("gateEast").GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Vector3.Distance(gateEv, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(gateE);
        }

        //if (Vector3.Distance(test1v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("test1") == null)
        //{
        //    test1 = new GameObject("test1");
        //    SpriteRenderer texture = test1.AddComponent<SpriteRenderer>();
        //    GameObject.Find("test1").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("test");
        //    GameObject.Find("test1").transform.position = test1v;
        //}
        //if (Vector3.Distance(test1v, player.GetComponent<Transform>().position) > scSize)
        //{
        //    DestroyObject(test1);
        //}

        //if (Vector3.Distance(test1v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("test1") == null)
        //{
        //    test1 = new GameObject("test1");
        //    SpriteRenderer texture = test1.AddComponent<SpriteRenderer>();
        //    GameObject.Find("test1").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("test");
        //    GameObject.Find("test1").transform.position = test1v;
        //}
        //if (Vector3.Distance(test1v, player.GetComponent<Transform>().position) > scSize)
        //{
        //    DestroyObject(test1);
        //}

        //if (Vector3.Distance(test1v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("test1") == null)
        //{
        //    test1 = new GameObject("test1");
        //    SpriteRenderer texture = test1.AddComponent<SpriteRenderer>();
        //    GameObject.Find("test1").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("test");
        //    GameObject.Find("test1").transform.position = test1v;
        //}
        //if (Vector3.Distance(test1v, player.GetComponent<Transform>().position) > scSize)
        //{
        //    DestroyObject(test1);
        //}


        //spawntest
        if (Vector3.Distance(test1v,player.GetComponent<Transform>().position) < scSize && GameObject.Find("test1") == null)
        {
            test1 = new GameObject("test1");
            SpriteRenderer texture = test1.AddComponent<SpriteRenderer>();
            GameObject.Find("test1").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("test");
            GameObject.Find("test1").transform.position = test1v;
        }
        if (Vector3.Distance(test1v, player.GetComponent<Transform>().position) > scSize)
         {
            DestroyObject(test1);
        }

        //spawn bg

        if (Vector3.Distance(bg1v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg1") == null)
        {
            bg1 = new GameObject("bg1");
            SpriteRenderer texture = bg1.AddComponent<SpriteRenderer>();
            GameObject.Find("bg1").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg01");
            GameObject.Find("bg1").transform.position = bg1v;
        }
        if (Vector3.Distance(bg1v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg1);
        }

        if (Vector3.Distance(bg2v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg2") == null)
        {
            bg2 = new GameObject("bg2");
            SpriteRenderer texture = bg2.AddComponent<SpriteRenderer>();
            GameObject.Find("bg2").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg02");
            GameObject.Find("bg2").transform.position = bg2v;
        }
        if (Vector3.Distance(bg2v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg2);
        }

        if (Vector3.Distance(bg3v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg3") == null)
        {
            bg3 = new GameObject("bg3");
            SpriteRenderer texture = bg3.AddComponent<SpriteRenderer>();
            GameObject.Find("bg3").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg03");
            GameObject.Find("bg3").transform.position = bg3v;
        }
        if (Vector3.Distance(bg3v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg3);
        }

        if (Vector3.Distance(bg4v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg4") == null)
        {
            bg4 = new GameObject("bg4");
            SpriteRenderer texture = bg4.AddComponent<SpriteRenderer>();
            GameObject.Find("bg4").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg04");
            GameObject.Find("bg4").transform.position = bg4v;
        }
        if (Vector3.Distance(bg4v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg4);
        }

        if (Vector3.Distance(bg5v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg5") == null)
        {
            bg5 = new GameObject("bg5");
            SpriteRenderer texture = bg5.AddComponent<SpriteRenderer>();
            GameObject.Find("bg5").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg05");
            GameObject.Find("bg5").transform.position = bg5v;
        }
        if (Vector3.Distance(bg5v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg5);
        }

        if (Vector3.Distance(bg6v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg6") == null)
        {
            bg6 = new GameObject("bg6");
            SpriteRenderer texture = bg6.AddComponent<SpriteRenderer>();
            GameObject.Find("bg6").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg06");
            GameObject.Find("bg6").transform.position = bg6v;
        }
        if (Vector3.Distance(bg6v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg6);
        }

        if (Vector3.Distance(bg7v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg7") == null)
        {
            bg7 = new GameObject("bg7");
            SpriteRenderer texture = bg7.AddComponent<SpriteRenderer>();
            GameObject.Find("bg7").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg07");
            GameObject.Find("bg7").transform.position = bg7v;
        }
        if (Vector3.Distance(bg7v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg7);
        }

        if (Vector3.Distance(bg8v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg8") == null)
        {
            bg8 = new GameObject("bg8");
            SpriteRenderer texture = bg8.AddComponent<SpriteRenderer>();
            GameObject.Find("bg8").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg08");
            GameObject.Find("bg8").transform.position = bg8v;
        }
        if (Vector3.Distance(bg8v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg8);
        }

        if (Vector3.Distance(bg9v, player.GetComponent<Transform>().position) < scSize && GameObject.Find("bg9") == null)
        {
            bg9 = new GameObject("bg9");
            SpriteRenderer texture = bg9.AddComponent<SpriteRenderer>();
            GameObject.Find("bg9").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bg09");
            GameObject.Find("bg9").transform.position = bg9v;
        }
        if (Vector3.Distance(bg9v, player.GetComponent<Transform>().position) > scSize)
        {
            DestroyObject(bg9);
        }

    }
}