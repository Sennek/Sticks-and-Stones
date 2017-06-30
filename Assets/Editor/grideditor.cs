using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(grid))]
public class grideditor : Editor {
    grid grid;
   
    private void OnEnable()
    {
        
        grid = (grid)target;

    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        grid.width = createSlider("Width", grid.width); 
        grid.height = createSlider("Height", grid.height);

       if(GUILayout.Button("Save"))
        {
            Queue objectsAll = new Queue(GameObject.FindObjectsOfType<GameObject>());

            for (int count = objectsAll.Count; count >= 1;)
            {
                if ((objectsAll.Peek() == GameObject.Find("levelEditor")) || (objectsAll.Peek() == GameObject.Find("Main Camera")))
                {
                    objectsAll.Dequeue();
                }
                else if ((objectsAll.Peek() != GameObject.Find("levelEditor")) || ((objectsAll.Peek() != GameObject.Find("Main Camera"))))
                {
                    Debug.Log("Pitooh left " + objectsAll.Count);
                    DestroyImmediate((GameObject)objectsAll.Dequeue());
                }
            }
        }

        if (GUILayout.Button("Write"))
        {
            grid.writeToTxt();
        }
    }

    private float createSlider(string labelName, float sliderPosition)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid " + labelName);
        sliderPosition = EditorGUILayout.Slider(sliderPosition, 1f, 100f, null);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }
}
