using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class savelevel : MonoBehaviour {

    
    [MenuItem("Tools/Write file")]
	static void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            string path = "Assets/Resources/textfile.txt";

            //write text to txt
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine("abs123");
            writer.Close();
        }
        if (Input.GetKey(KeyCode.E))
        {
            string path = "Assets/Resources/textfile.txt";

            //read from txt

            StreamReader reader = new StreamReader(path);
            Debug.Log(reader.ReadToEnd());
            reader.Close();
        }
        ////reimport txt to update the reference

        //AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load("textfile");

        ////print text
        //Debug.Log(asset.text);
    }

   // [MenuItem("Tools/Read file")]
    //static void ReadTxt()
    //{
    //    string path = "Assets/Resources/textfile.txt";

    //    //read from txt

    //    StreamReader reader = new StreamReader(path);
    //    Debug.Log(reader.ReadToEnd());
    //    reader.Close();
    //}
}
