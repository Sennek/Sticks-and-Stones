using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class grid : MonoBehaviour {

    public float width = 32f;
    public float height = 32f;
    public bool save;

    public Color color = Color.white;

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

    public void writeToTxt()
    {
        StreamWriter sw;
        sw = new StreamWriter("Assets/Resources/test.txt", true);
        sw.WriteLine("this is test");
        sw.Close();
    }

    //private void Update()
    //{
    //    sw.WriteLine("this is test");

    //    sw.Flush();
    //}

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
