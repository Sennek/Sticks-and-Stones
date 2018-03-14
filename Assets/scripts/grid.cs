using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class grid : MonoBehaviour {

    public float width = 32f;
    public float height = 32f;
    public bool save;
    
    public Color color = Color.white;

        public void SaveVectors()
    {
        StreamWriter sw;
        sw = new StreamWriter("Assets/Resources/level.txt", true);

       Queue objects = new Queue(GameObject.FindObjectsOfType<GameObject>());
        foreach (GameObject t in objects)
        {
            if ((t.name == "levelEditor") || (t.name == "Main Camera"))
            {
                objects.Dequeue();
            }
           else if ((t.name != "levelEditor") || (t.name != "Main Camera"))
            {
                Debug.Log(t.name);
                objects.Dequeue();
            }
        }

    //    GameObject[] objects = GameObject.FindObjectsOfType<GameObject>() as GameObject[];       
    //    foreach (GameObject t in objects)
    //    {
    //        if ((t.name != "levelEditor") || (t.name != "Main Camera"))
    //        sw.WriteLine(t.name + " " + t.transform.position + " " + t.transform.localScale + " " + t.GetComponent<SpriteRenderer>().flipX + " " + t.GetComponent<SpriteRenderer>().sprite);
    //        //Debug.Log(t.GetComponent<SpriteRenderer>().flipX); // Debugging
    //    }
    //    sw.Close();
    }

   void Update()
    {
        if (save)
            ButtonSave();
        save = false;
    }

    public void ButtonSave()
    {
        print("saved!");
    }

    void OnDrawGizmos()
    {
        Vector3 pos = Camera.current.transform.position;
        Gizmos.color = this.color;

        for (float y = pos.y - 1080f;
            y < pos.y + 1080f;
            y += this.height)
        {
            Gizmos.DrawLine(new Vector3(-1000000f, Mathf.Floor(y / height) * height, 0f), 
                            new Vector3(1000000000f, Mathf.Floor(y / height) * height, 0f));
        }

        for (float x = pos.x - 1920f;
            x < pos.x + 1820f;
            x += this.width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, -1000000, 0), 
                            new Vector3(Mathf.Floor(x / width) * width, 1000000, 0));
        }

    }
}
