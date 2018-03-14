using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using XmlLoader;

public class xmltest : MonoBehaviour
{
    public Dialogue dia;
    public GameObject player;
    public Rigidbody2D rgb;
    public Vector2 moveDirection;
    public Dialogue dialogue;
    public int nodeToDisplay;
    public bool showDialogueWindow;
    private float spawnPoint;

    Rect dialogueWindowRect = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500);

    public void Start()
    {
        player = GameObject.Find("Player");
        rgb = GetComponent<Rigidbody2D>();
        StartCoroutine(Wander());
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue));
        TextAsset test = (TextAsset)Resources.Load("testr", typeof(TextAsset));
        using (StringReader stream = new StringReader(test.text))
        { dialogue = (Dialogue)serializer.Deserialize(stream); }
        spawnPoint = transform.position.x;
    }

    public IEnumerator Wander()
    {
        while (true)
        {
            if (((spawnPoint - transform.position.x) > 4) && (transform.position.x < spawnPoint))
            {
                moveDirection.x = 1;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSecondsRealtime(Random.Range(2, 4));
            }
            else if (((spawnPoint - transform.position.x) > 4) && (transform.position.x > spawnPoint))
            {
                moveDirection.x = -1;
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                yield return new WaitForSecondsRealtime(Random.Range(2, 4));
            }
            else
            {
                int k = Random.Range(-2, 2);
                moveDirection.x = k;
                if (k < 0)
                { gameObject.GetComponent<SpriteRenderer>().flipX = false; }
                else if (k > 0)
                { gameObject.GetComponent<SpriteRenderer>().flipX = true; }
                yield return new WaitForSecondsRealtime(Random.Range(2, 4));
                moveDirection.x = 0;
                yield return new WaitForSecondsRealtime(Random.Range(2, 4));
            }
        }
    }

    void DialogueWindowMethod(int windowId)
    {

        GUILayout.BeginArea(new Rect(0, 0, 500, 500));
        GUILayout.BeginVertical();

        if (nodeToDisplay >= 0)
        {
            GUILayout.Box(dialogue.nodes[nodeToDisplay].text, GUILayout.Width(500), GUILayout.Height(150));
            for (int i = 0; i < dialogue.nodes[nodeToDisplay].options.Count; i++)
            {
                if (GUILayout.Button(dialogue.nodes[nodeToDisplay].options[i].text, GUILayout.Width(500), GUILayout.Height(50)))
                {
                    nodeToDisplay = dialogue.nodes[nodeToDisplay].options[i].id;

                    if (nodeToDisplay < 0)
                    {
                        showDialogueWindow = false;
                        break;
                    }
                    if(nodeToDisplay==13)
                    {
                        player.GetComponent<PartyScript>().AddToParty(gameObject);
                        showDialogueWindow = false;
                        break;
                    }
                }
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    public void OnGUI()
    {
        if (showDialogueWindow)
        { dialogueWindowRect = GUI.Window(1, dialogueWindowRect, DialogueWindowMethod, ""); }
    }

    public void Update()
    {
        if (!showDialogueWindow)
        { rgb.velocity = moveDirection * Random.Range(0, 2); }
    }

    void OnMouseDown()
    {
        nodeToDisplay = 0;
        showDialogueWindow = true;
    }


}
