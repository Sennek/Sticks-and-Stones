using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using DialogueContainer;

public class xmltest : MonoBehaviour
{
    public Dialogue dia;

    public void Start()
    {
        string path = Path.Combine(Application.dataPath, "test.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            dia = (Dialogue)serializer.Deserialize(stream);
        }
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 500, 500));
        GUILayout.BeginVertical();
        if (GUILayout.Button("save", GUILayout.Width(100), GUILayout.Height(50)))
        {
            
        }
        if (GUILayout.Button("Print", GUILayout.Width(100), GUILayout.Height(50)))
        {
            
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }

}
