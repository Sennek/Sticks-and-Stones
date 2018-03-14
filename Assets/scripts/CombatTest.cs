using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XmlLoader;
using System.Xml.Serialization;
using System.IO;

public class CombatTest : MonoBehaviour
{
    public InventoryGUI inventory;
    GameObject player;
    public List<GameObject> unitsAction = new List<GameObject>();
    public List<GameObject> templist = new List<GameObject>();
    public bool moveToTarget; public bool moveFromTarget;
    public Vector3 targetDestination;
    public bool spawnUnits;
    public bool showLoseScreen;
    public bool showWinScreen;
    public bool showPartySelectionScreen = true;
    public bool showEnemySpawnScreen = false;
    public List<ItemsMk2> loot = new List<ItemsMk2>();
    public List<int> lootStack = new List<int>();

    public bool newAction;
    public bool newRound;
    public int roundNo;
    public EnemyUnits enemyUnits;
    TextAsset xml;
    public int partyIdToSpawn;
    public int partySlotToSpawn = 1;
    public int enemyIdToSpawn;
    public int enemySlotToSpawn = 1;
    public int testID;
    public int testPOS = 1;
    public List<int> unitPositions = new List<int>(); //0,1,2 - party positions, 3,4,5 - enemy positions

    //test vars
    public int[] unitId;
    public int[] unitPos;
    public Texture2D[] spawnIcon;
    //test vars over

    void Start()
    {
        //test var
        spawnIcon = new Texture2D[4];
        unitId = new int[4];
        unitPos = new int[4];
        //test var end
        inventory = gameObject.GetComponent<InventoryGUI>();
        for (int i = 0; i < 6; i++)
        { unitPositions.Add(0); }
        XmlSerializer serializer = new XmlSerializer(typeof(EnemyUnits));
        xml = (TextAsset)Resources.Load("EnemyUnits", typeof(TextAsset));
        using (StringReader stream = new StringReader(xml.text))
        { enemyUnits = (EnemyUnits)serializer.Deserialize(stream); }
    }

    public IEnumerator SortActionOrder()
    {
        yield return new WaitForSecondsRealtime(0);
        roundNo++;
        unitsAction.Clear();
        GameObject tempObj;
        CombatUnit[] unitScripts = GameObject.FindObjectsOfType(typeof(CombatUnit)) as CombatUnit[];
        foreach (CombatUnit go in unitScripts)
        {
            unitsAction.Add(go.gameObject);
        }
        bool needToRepeat = true;
        while (needToRepeat)
        {
            bool swapped = false;
            for (int i = 0; i < unitsAction.Count; i++)
            {
                if (i > 0)
                {
                    if (unitsAction[i].GetComponent<CombatUnit>().speed > unitsAction[i - 1].GetComponent<CombatUnit>().speed)
                    {
                        tempObj = unitsAction[i - 1];
                        unitsAction[i - 1] = unitsAction[i];
                        unitsAction[i] = tempObj;
                        tempObj = null;
                        swapped = true;
                    }
                    if (unitsAction[i].GetComponent<CombatUnit>().speed == unitsAction[i - 1].GetComponent<CombatUnit>().speed)
                    {
                        int randomize = Random.Range(-10, 10);
                        if (randomize > 0)
                        {
                            tempObj = unitsAction[i - 1];
                            unitsAction[i - 1] = unitsAction[i];
                            unitsAction[i] = tempObj;
                            tempObj = null;
                        }
                    }
                }
            }
            if (!swapped)
            {
                needToRepeat = false;
            }
        }
        for (int i = 0; i < unitsAction.Count; i++)
        {
            unitsAction[i].GetComponent<CombatUnit>().actionComplete = false;
        }
        newAction = true;
        newRound = false;
        ReshufflePositions(false);
    }
    IEnumerator MoveObject(bool moveback, GameObject unit, Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            unit.GetComponent<Transform>().position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        unit.GetComponent<Transform>().position = target;
        if (moveback)
        {
            startTime = Time.time;
            while (Time.time < startTime + overTime)
            {
                unit.GetComponent<Transform>().position = Vector3.Lerp(target, source, (Time.time - startTime) / overTime);
                yield return null;
            }
            unit.GetComponent<Transform>().position = source;
        }
    }

    public void SpawnUnit(bool isEnemy, int id, int pos)
    {
        if (!isEnemy)
        {
            unitPositions[pos - 1]++;
        }
        if (isEnemy)
        {
            unitPositions[2 + pos]++;
        }
        if (pos <= enemyUnits.units[id].slot)
        {
            GameObject unit = new GameObject();
            unit.AddComponent<SpriteRenderer>();
            SpriteRenderer unitSprite = unit.GetComponent<SpriteRenderer>();
            unitSprite.sprite = Resources.Load<Sprite>("unit" + id);

            GameObject hpBarEmpty = new GameObject();
            GameObject hpBarFull = new GameObject();
            hpBarFull.name = "hpbar";
            hpBarEmpty.GetComponent<Transform>().SetParent(unit.GetComponent<Transform>());
            hpBarEmpty.GetComponent<Transform>().position = new Vector2(0, 3.2f);
            hpBarFull.GetComponent<Transform>().SetParent(unit.GetComponent<Transform>());
            hpBarFull.GetComponent<Transform>().position = new Vector3(0, 3.2f, -1);
            hpBarEmpty.AddComponent<SpriteRenderer>();
            hpBarFull.AddComponent<SpriteRenderer>();
            hpBarEmpty.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("hpempty");
            hpBarFull.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("hpfull");

            unit.AddComponent<BoxCollider2D>();
            unit.GetComponent<BoxCollider2D>().isTrigger = true;
            unit.AddComponent<CombatUnit>();
            CombatUnit unitScript = unit.GetComponent<CombatUnit>();
            unit.GetComponent<CombatUnit>().unitPositionNumber = pos;
            if (isEnemy)
            {
                unit.GetComponent<SpriteRenderer>().flipX = true;
                unitScript.isEnemy = true;
            }

            for (int i = 0; i < enemyUnits.units.Count; i++)
            {
                if (i == id)
                {
                    unitScript.id = enemyUnits.units[i].id;
                    unit.name = enemyUnits.units[i].name;
                    unitScript.range = enemyUnits.units[i].range;
                    unitScript.maxhp = enemyUnits.units[i].hp;
                    unitScript.currenthp = enemyUnits.units[i].hp;
                    unitScript.minatk = enemyUnits.units[i].minatk;
                    unitScript.maxatk = enemyUnits.units[i].maxatk;
                    unitScript.def = enemyUnits.units[i].def;
                    if (isEnemy) { unitScript.isEnemy = true; }
                    unitScript.speed = enemyUnits.units[i].spd;
                }
            }
            if (((isEnemy) && (unitPositions[2 + pos] > 4)) || ((!isEnemy) && (unitPositions[pos - 1] > 4)))
            {
                if (isEnemy)
                { unitPositions[2 + pos]--; }
                if (!isEnemy)
                { unitPositions[pos - 1]--; }
                DestroyImmediate(unit);
            }
        }
        ReshufflePositions(true);
    }
    public void KillUnit(GameObject unit)
    {
        if (unit.GetComponent<CombatUnit>().isEnemy)
        {
            unitPositions[2 + unit.GetComponent<CombatUnit>().unitPositionNumber]--;
        }
        if (!unit.GetComponent<CombatUnit>().isEnemy)
        {
            unitPositions[unit.GetComponent<CombatUnit>().unitPositionNumber - 1]--;
        }
        unitsAction.Remove(unit);
        DestroyImmediate(unit);
        bool foundEnemyUnit = false;
        bool foundPlayerUnit = false;
        CombatUnit[] unitsAll = FindObjectsOfType(typeof(CombatUnit)) as CombatUnit[];
        for (int i = 0; i < unitsAll.Length; i++)
        {
            if (!unitsAll[i].isEnemy)
            {
                foundPlayerUnit = true;
            }
            if (unitsAll[i].isEnemy)
            {
                foundEnemyUnit = true;
            }
        }
        if (!foundEnemyUnit)
        { showWinScreen = true; }
        if (!foundPlayerUnit)
        { showLoseScreen = true; }
        ReshufflePositions(false);
    }
    public void ReshufflePositions(bool initialize)
    {
        List<CombatUnit> friendlyUnits1 = new List<CombatUnit>();
        List<CombatUnit> friendlyUnits2 = new List<CombatUnit>();
        List<CombatUnit> friendlyUnits3 = new List<CombatUnit>();
        List<CombatUnit> enemyUnits1 = new List<CombatUnit>();
        List<CombatUnit> enemyUnits2 = new List<CombatUnit>();
        List<CombatUnit> enemyUnits3 = new List<CombatUnit>();
        CombatUnit[] units = FindObjectsOfType(typeof(CombatUnit)) as CombatUnit[];
        for (int i = 0; i < units.Length; i++)
        {
            if ((units[i].isEnemy) && units[i].unitPositionNumber == 1)
            { enemyUnits1.Add(units[i]); }
            else if ((units[i].isEnemy) && units[i].unitPositionNumber == 2)
            { enemyUnits2.Add(units[i]); }
            else if ((units[i].isEnemy) && units[i].unitPositionNumber == 3)
            { enemyUnits3.Add(units[i]); }
            else if ((!units[i].isEnemy) && units[i].unitPositionNumber == 1)
            { friendlyUnits1.Add(units[i]); }
            else if ((!units[i].isEnemy) && units[i].unitPositionNumber == 2)
            { friendlyUnits2.Add(units[i]); }
            else if ((!units[i].isEnemy) && units[i].unitPositionNumber == 3)
            { friendlyUnits3.Add(units[i]); }
        }
        //party units
        for (int i = 1; i < friendlyUnits1.Count + 1; i++)
        {
            friendlyUnits1[i - 1].unitPosition = new Vector3(-2, -unitPositions[0] + 3 * (i - 1), 0.1f * i);
            StartCoroutine(MoveObject(false, friendlyUnits1[i - 1].gameObject, friendlyUnits1[i - 1].transform.position, friendlyUnits1[i - 1].unitPosition, 0.3f));
        }
        for (int i = 1; i < friendlyUnits2.Count + 1; i++)
        {
            friendlyUnits2[i - 1].unitPosition = new Vector3(-5, -unitPositions[1] + 3 * (i - 1), 0.1f * i);
            StartCoroutine(MoveObject(false, friendlyUnits2[i - 1].gameObject, friendlyUnits2[i - 1].transform.position, friendlyUnits2[i - 1].unitPosition, 0.3f));
        }
        for (int i = 1; i < friendlyUnits3.Count + 1; i++)
        {
            friendlyUnits3[i - 1].unitPosition = new Vector3(-8, -unitPositions[2] + 3 * (i - 1), 0.1f * i);
            StartCoroutine(MoveObject(false, friendlyUnits3[i - 1].gameObject, friendlyUnits3[i - 1].transform.position, friendlyUnits3[i - 1].unitPosition, 0.3f));
        }
        //enemy units
        for (int i = 1; i < enemyUnits1.Count + 1; i++)
        {
            enemyUnits1[i - 1].unitPosition = new Vector3(2, -unitPositions[3] + 3 * (i - 1), 0.1f * i);
            StartCoroutine(MoveObject(false, enemyUnits1[i - 1].gameObject, enemyUnits1[i - 1].transform.position, enemyUnits1[i - 1].unitPosition, 0.3f));
        }
        for (int i = 1; i < enemyUnits2.Count + 1; i++)
        {
            enemyUnits2[i - 1].unitPosition = new Vector3(5, -unitPositions[4] + 3 * (i - 1), 0.1f * i);
            StartCoroutine(MoveObject(false, enemyUnits2[i - 1].gameObject, enemyUnits2[i - 1].transform.position, enemyUnits2[i - 1].unitPosition, 0.3f));
        }
        for (int i = 1; i < enemyUnits3.Count + 1; i++)
        {
            enemyUnits3[i - 1].unitPosition = new Vector3(8, -unitPositions[5] + 3 * (i - 1), 0.1f * i);
            StartCoroutine(MoveObject(false, enemyUnits3[i - 1].gameObject, enemyUnits3[i - 1].transform.position, enemyUnits3[i - 1].unitPosition, 0.3f));
        }

        if (!initialize)
        {
            if ((friendlyUnits1.Count == 0) && (friendlyUnits2.Count != 0))
            {
                friendlyUnits1 = friendlyUnits2;
                friendlyUnits2 = null;
                for (int i = 0; i < friendlyUnits1.Count; i++)
                {
                    friendlyUnits1[i].unitPositionNumber = 1;
                    unitPositions[0]++;
                    unitPositions[1]--;
                }
            }
            else if ((friendlyUnits1.Count == 0) && (friendlyUnits2.Count == 0))
            {
                friendlyUnits1 = friendlyUnits3;
                friendlyUnits3 = null;
                for (int i = 0; i < friendlyUnits1.Count; i++)
                {
                    friendlyUnits1[i].unitPositionNumber = 1;
                    unitPositions[0]++;
                    unitPositions[2]--;
                }
            }
            if ((enemyUnits1.Count == 0) && (enemyUnits2.Count != 0))
            {
                enemyUnits1 = enemyUnits2;
                enemyUnits2 = null;
                for (int i = 0; i < enemyUnits1.Count; i++)
                {
                    enemyUnits1[i].unitPositionNumber = 1;
                    unitPositions[3]++;
                    unitPositions[4]--;
                }
            }
            else if ((enemyUnits1.Count == 0) && (enemyUnits2.Count == 0))
            {
                enemyUnits1 = enemyUnits3;
                enemyUnits3 = null;
                for (int i = 0; i < enemyUnits1.Count; i++)
                {
                    enemyUnits1[i].unitPositionNumber = 1;
                    unitPositions[3]++;
                    unitPositions[5]--;
                }
            }
            ReshufflePositions(true);
        }
    }

    public IEnumerator SpawnFloatingText(GameObject unit, string text, int x, int y, float time)
    {
        float startTime = Time.time;
        GameObject floatText = new GameObject();
        floatText.GetComponent<Transform>().position = unit.GetComponent<Transform>().position + new Vector3(0, 1.5f, 0);
        floatText.AddComponent<TextMesh>();
        TextMesh textScript = floatText.GetComponent<TextMesh>();
        textScript.text = text;
        textScript.characterSize = 0.3f;
        textScript.fontSize = 45;
        textScript.color = Color.red;
        while (Time.time < startTime + time)
        {
            floatText.GetComponent<Transform>().position += new Vector3(x * 0.2f, y * 0.1f, 0);
            yield return null;
        }
        Destroy(floatText);
    }

    public IEnumerator Attack(GameObject target)
    {
        StartCoroutine(MoveObject(true, unitsAction[0], unitsAction[0].GetComponent<Transform>().position, target.GetComponent<Transform>().position, 0.3f));
        yield return new WaitForSecondsRealtime(0.3f);
        int damage = Random.Range(unitsAction[0].GetComponent<CombatUnit>().minatk - 1, unitsAction[0].GetComponent<CombatUnit>().maxatk + 1) - target.GetComponent<CombatUnit>().def;
        StartCoroutine(SpawnFloatingText(target, damage.ToString(), 0, 1, 0.5f));        //apply damage
        if ((target.GetComponent<CombatUnit>().currenthp - damage) < 0)
        { target.GetComponent<CombatUnit>().currenthp = 0; }
        else { target.GetComponent<CombatUnit>().currenthp -= damage; } //damage unit
        target.transform.Find("hpbar").GetComponent<Transform>().localScale = new Vector3((float)target.GetComponent<CombatUnit>().currenthp / (float)target.GetComponent<CombatUnit>().maxhp, 1, 1);
        yield return new WaitForSecondsRealtime(1);
        if (target.GetComponent<CombatUnit>().currenthp <= 0)
        { KillUnit(target); }        //if target dies
        unitsAction.RemoveAt(0);
        if (unitsAction.Count > 0)
        { newAction = true; }
        else
        {
            newRound = true;
            StartCoroutine(SortActionOrder());
        }
    }

    void Update()
    {
        if ((!newRound) && (newAction) && (unitsAction[0].GetComponent<CombatUnit>().isEnemy))
        {
            CombatUnit[] unitsAll = FindObjectsOfType(typeof(CombatUnit)) as CombatUnit[];
            for (int i = 0; i < unitsAll.Length; i++)
            {
                if ((unitsAll[i].isEnemy == false) && (unitsAction[0].GetComponent<CombatUnit>().range >= unitsAll[i].unitPositionNumber))
                {
                    newAction = false;
                    StartCoroutine(Attack(unitsAll[i].gameObject));
                    break;
                }
            }
        }
    }
    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < unitsAction.Count; i++)
        {
            Texture2D icon = Resources.Load<Texture2D>("unit" + unitsAction[i].GetComponent<CombatUnit>().id);
            GUI.DrawTexture(new Rect(50 * i, 0, 50, 50), icon, ScaleMode.ScaleToFit);
            GUILayout.BeginArea(new Rect(50 * i, 50, 50, 50));
            GUILayout.BeginVertical();
            GUILayout.Box(unitsAction[i].GetComponent<CombatUnit>().currenthp + "/" + unitsAction[i].GetComponent<CombatUnit>().maxhp);
            if (unitsAction[i].GetComponent<CombatUnit>().isEnemy)
            { GUILayout.Box("Enemy"); }
            else { GUILayout.Box("Friendly"); }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        GUILayout.EndHorizontal();
        //if (spawnUnits)
        // {
        //     GUI.Window(1, new Rect(Screen.width - 700, Screen.height - 700, 700, 700), SpawnUnitsGUI, "Select units to spawn:");
        //}
        if (showLoseScreen)
        {
            GUI.Window(1, new Rect(Screen.width - 700, Screen.height - 700, 700, 700), LoseScreen, "");
        }
        if (showWinScreen)
        {
            GUI.Window(1, new Rect(Screen.width - 700, Screen.height - 700, 700, 700), WinScreen, "");
        }
        if (showPartySelectionScreen)
        {
            GUI.Window(1, new Rect(50, 50, 700, 700), partyPlacement, "");
        }
        if (showEnemySpawnScreen)
        {
            GUI.Window(1, new Rect(50, 50, 700, 700), enemySpawn, "");
        }
    }

    public void WinScreen(int windowid)
    {
        GUILayout.BeginArea(new Rect(0, 0, 250, 50));
        GUILayout.Box("You won!", GUILayout.Width(100), GUILayout.Height(25));
        GUILayout.EndArea();
    }
    public void LoseScreen(int windowid)
    {
        GUILayout.BeginArea(new Rect(0, 0, 250, 50));
        GUILayout.Box("You lost!", GUILayout.Width(100), GUILayout.Height(25));
        GUILayout.EndArea();
    }
    public void SpawnUnitsGUI(int windowid)
    {
        GUILayout.BeginArea(new Rect(0, 20, 350, 700));
        GUILayout.BeginVertical();
        GUILayout.Box("Spawn party units", GUILayout.Width(350), GUILayout.Height(25));
        GUILayout.BeginHorizontal();
        GUILayout.Box("Unit ID", GUILayout.Width(100), GUILayout.Height(50));
        int.TryParse(GUILayout.TextField(partyIdToSpawn.ToString()), out partyIdToSpawn);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("Spawn position", GUILayout.Width(100), GUILayout.Height(50));
        int.TryParse(GUILayout.TextField(partySlotToSpawn.ToString()), out partySlotToSpawn);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Spawn", GUILayout.Width(100), GUILayout.Height(50)))
        { SpawnUnit(false, partyIdToSpawn, partySlotToSpawn); }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(350, 20, 350, 700));
        GUILayout.Box("Spawn enemy units", GUILayout.Width(350), GUILayout.Height(25));
        GUILayout.BeginHorizontal();
        GUILayout.Box("Unit ID", GUILayout.Width(100), GUILayout.Height(50));
        int.TryParse(GUILayout.TextField(enemyIdToSpawn.ToString()), out enemyIdToSpawn);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("Spawn position", GUILayout.Width(100), GUILayout.Height(50));
        int.TryParse(GUILayout.TextField(enemySlotToSpawn.ToString()), out enemySlotToSpawn);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Spawn", GUILayout.Width(100), GUILayout.Height(50)))
        { SpawnUnit(true, enemyIdToSpawn, enemySlotToSpawn); }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(250, 500, 200, 75));
        if (GUILayout.Button("Begin combat", GUILayout.Width(200), GUILayout.Height(50)))
        {
            spawnUnits = false;
            StartCoroutine(SortActionOrder());
        }
        GUILayout.EndArea();
    }
    public void partyPlacement(int windowid)
    {
        GUILayout.BeginArea(new Rect((Screen.width / 2) - 200, 20, Screen.width, Screen.height));
        GUILayout.Box("Select your party", GUILayout.Width(150), GUILayout.Height(20));
        GUILayout.EndArea();

        for (int i = 0; i < 4; i++)
        {
            //reset inappropriate input
            if (unitId[i] < enemyUnits.units.Count)
            { spawnIcon[i] = Resources.Load<Texture2D>("unit" + unitId[i]); }
            else
            {
                unitId[i] = 0;
            }
            if (unitPos[i] < 1 || unitPos[i] > 3)
            { unitPos[i] = 1; }
            //end reset

            GUILayout.BeginArea(new Rect(150 * i, 100, 150, 700));

            GUILayout.BeginVertical();

            GUILayout.Box(spawnIcon[i], GUILayout.Width(150), GUILayout.Height(150));

            GUILayout.BeginHorizontal();
            GUILayout.Box("MaxPos=" + enemyUnits.units[unitId[i]].slot, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.Box("MaxRange=" + enemyUnits.units[unitId[i]].range, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Unit ID", GUILayout.Width(75), GUILayout.Height(20));
            int.TryParse(GUILayout.TextField(unitId[i].ToString(), 2), out unitId[i]);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Unit Pos", GUILayout.Width(75), GUILayout.Height(20));
            int.TryParse(GUILayout.TextField(unitPos[i].ToString(), 2), out unitPos[i]);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        GUILayout.BeginArea(new Rect((Screen.width / 2) - 200, 400, 200, 50));
        GUILayout.BeginVertical();
        if (GUILayout.Button("Spawn Party", GUILayout.Width(200), GUILayout.Height(25)))
        {
            for (int i = 0; i < 4; i++)
            {
                SpawnUnit(false, unitId[i], unitPos[i]);
                showPartySelectionScreen = false;
                showEnemySpawnScreen = true;

            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    public void enemySpawn(int windowid)
    {
        GUILayout.BeginArea(new Rect((Screen.width / 2) - 200, 20, Screen.width, Screen.height));
        GUILayout.Box("Spawn Enemy", GUILayout.Width(150), GUILayout.Height(20));
        GUILayout.EndArea();

        //reset inappropriate input
        if (unitId[0] < enemyUnits.units.Count)
        { spawnIcon[0] = Resources.Load<Texture2D>("unit" + unitId[0]); }
        else
        {
            unitId[0] = 0;
        }
        if (unitPos[0] < 1 || unitPos[0] > 3)
        { unitPos[0] = 1; }
        //end reset

        GUILayout.BeginArea(new Rect(150, 100, 150, 700));

        GUILayout.BeginVertical();

        GUILayout.Box(spawnIcon[0], GUILayout.Width(150), GUILayout.Height(150));

        GUILayout.BeginHorizontal();
        GUILayout.Box("MaxPos=" + enemyUnits.units[unitId[0]].slot, GUILayout.Width(75), GUILayout.Height(20));
        GUILayout.Box("MaxRange=" + enemyUnits.units[unitId[0]].range, GUILayout.Width(75), GUILayout.Height(20));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("Unit ID", GUILayout.Width(75), GUILayout.Height(20));
        int.TryParse(GUILayout.TextField(unitId[0].ToString(), 2), out unitId[0]);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("Unit Pos", GUILayout.Width(75), GUILayout.Height(20));
        int.TryParse(GUILayout.TextField(unitPos[0].ToString(), 2), out unitPos[0]);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect((Screen.width / 2) - 200, 400, 200, 50));
        GUILayout.BeginVertical();
        if (GUILayout.Button("Spawn Enemy", GUILayout.Width(200), GUILayout.Height(25)))
        {
            SpawnUnit(true, unitId[0], unitPos[0]);
        }
        if (GUILayout.Button("Begin battle", GUILayout.Width(200), GUILayout.Height(25)))
        {
            showEnemySpawnScreen = false;
            StartCoroutine(SortActionOrder());
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();


    }
}
