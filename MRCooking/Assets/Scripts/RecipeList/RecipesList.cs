using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.XR;
using UnityEngine.XR.Management;

[Serializable]
public class DB
{
    public string[] CookMode;
    public string[] RecordMode;
}
public class RecipesList : MonoBehaviour
{
    TextAsset jsonTextFile;
    public DB db, Exdb, Rdb;
    int pg;
    public Recipe[] recipeList;

    GameObject[] buttons;

    [SerializeField]
    public GameObject recCust;

    [SerializeField]
    public static string mode =  "CookMode";

    [SerializeField]
    public static Recipe jsonfile;

    [SerializeField]
    public TextMeshProUGUI heading;

    public static string selectedRecipe;


    void Awake()
    {
        pg = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        jsonTextFile = Resources.Load<TextAsset>("DB");
        Rdb = JsonUtility.FromJson<DB>(jsonTextFile.text);
        var exDataPath = Application.persistentDataPath;
#if !UNITY_EDITOR
        var exDBPath = Path.Combine(exDataPath, "DB.json");
#endif
#if UNITY_EDITOR
        var exDBPath = exDataPath + "/DB.json";
#endif
        Debug.Log("ExDBpath: "+exDBPath);
        Debug.Log("status: " + System.IO.File.Exists(exDBPath));
        if (!System.IO.File.Exists(exDBPath))
        {
            Debug.Log("Json file created");
            DB tempDB = new DB();
            string writeDB = JsonUtility.ToJson(tempDB);
            File.WriteAllText(exDBPath, writeDB);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        string dbFile = File.ReadAllText(exDBPath);
        Exdb = JsonUtility.FromJson<DB>(dbFile);
        List<Recipe> list = new List<Recipe>();
        mode = modeselect.mode;
        //print('choose Recipe called');
        Debug.Log("Mode : "+mode);
        db = new DB();
        if (mode == "CookMode")
        {
            heading.SetText("Cooking Recipe List");
            List<string> fnames = new List<string>();
            foreach(string f in Rdb.CookMode)
            {
                jsonTextFile = Resources.Load<TextAsset>(f);
                Recipe recipe = JsonUtility.FromJson<Recipe>(jsonTextFile.text);
                list.Add(recipe);
                fnames.Add(f);
            }
            foreach(string f in Exdb.CookMode)
            {
                var fpath = Path.Combine(exDBPath, f);
                string fdata = File.ReadAllText(fpath);
                Recipe recipe = JsonUtility.FromJson<Recipe>(fdata);
                list.Add(recipe);
                fnames.Add(f);
            }
            selectedRecipe = Rdb.CookMode[0];
            db.CookMode = fnames.ToArray();

        }
        else
        {
            recCust.SetActive(true);
            heading.SetText("Recording Recipe List");
            List<string> fnames = new List<string>();
            foreach (string f in Rdb.RecordMode)
            {
                jsonTextFile = Resources.Load<TextAsset>(f);
                Recipe recipe = JsonUtility.FromJson<Recipe>(jsonTextFile.text);
                list.Add(recipe);
                fnames.Add(f);
            }
            foreach (string f in Exdb.RecordMode)
            {
                var fpath = Path.Combine(exDBPath, f);
                string fdata = File.ReadAllText(fpath);
                Recipe recipe = JsonUtility.FromJson<Recipe>(fdata);
                list.Add(recipe);
                fnames.Add(f);
            }
            selectedRecipe = Rdb.RecordMode[0];
            db.RecordMode = fnames.ToArray();
        }
        recipeList = list.ToArray();
        GameObject toggle = GameObject.FindGameObjectWithTag("RecipeList");
        List<GameObject> temp = new List<GameObject>();
        foreach(Transform g in toggle.transform)
        {
            temp.Add(g.gameObject);
        }
        buttons = temp.ToArray();
        //Debug.Log(buttons[0].transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>);
        Array.Sort(buttons, CompareObNames);
        NextPage();
        buttons[0].GetComponent<RecipeSelect>().OnGazeEntered();
    }
    int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    void FillToggles(Recipe[] items)
    {
        int max_len = Mathf.Min(items.Length, buttons.Length);
        for (int i = 0; i < max_len; i++)
        {
            buttons[i].SetActive(true);
            GameObject txtObj = buttons[i].transform.Find("Text (TMP)").gameObject;
            TextMeshProUGUI Text = txtObj.GetComponent<TextMeshProUGUI>();
            //Debug.Log(Text.text);
            Text.SetText(items[i].Name);
            GameObject imgObj = buttons[i].transform.Find("Image").gameObject;
            RawImage img = imgObj.GetComponent<RawImage>();
            Texture2D srcImg = Resources.Load<Texture2D>(items[i].image);
            img.texture = srcImg;
            if (i == 0)
            {
                buttons[i].GetComponent<RecipeSelect>().OnGazeEntered();
                //selectedRecipe = items[i].Name;
                //imgObj.SetActive(true);
            }
        }
        if (buttons.Length > max_len)
        {
            for (int i = max_len; i < buttons.Length; i++)
            {
                buttons[i].SetActive(false);
            }
        }

    }

    public void NextPage()
    {
        if (pg + 1 <= Mathf.Ceil(recipeList.Length * 1.0f / buttons.Length))
        {
            pg++;
            List<Recipe> list = new List<Recipe>();
            for (int i = (pg - 1) * buttons.Length; i < Mathf.Min(recipeList.Length, pg * buttons.Length); i++)
            {
                list.Add(recipeList[i]);
            }
            Recipe[] items = list.ToArray();
            FillToggles(items);
        }
    }

    public void PrevPage()
    {
        if (pg > 1)
        {
            Debug.Log("Previous page");
            pg--;
            List<Recipe> list = new List<Recipe>();
            for (int i = (pg - 1) * buttons.Length; i < Mathf.Min(recipeList.Length, pg * buttons.Length); i++)
            {
                list.Add(recipeList[i]);
            }
            Recipe[] items = list.ToArray();
            FillToggles(items);

        }
    }

    public void chooseRecipe(){

        int i;
        string[] getPos;
        if (mode == "CookMode")
            getPos = db.CookMode;
        else
            getPos = db.RecordMode;
        for(i=0; i<getPos.Length; i++)
        {
            if (String.Equals(getPos[i], selectedRecipe))
                break;
        }
        jsonfile = recipeList[i];

        print("choose Recipe called");
        Debug.Log("choose Recipe called");

        Debug.Log("selected Recipe :"+selectedRecipe);

        if(mode == "CookMode")
            SceneManager.LoadScene("CookMode");
        else if (mode =="RecordMode")
        {
            StopXR();
            SceneManager.LoadScene("Record");

        }
            
    }

    public void CreateRecipe()
    {
        StopXR();
        SceneManager.LoadScene("CustomRecipe");
    }

    public void goBack(){
        Debug.Log("goBack called");
        SceneManager.LoadScene("SelectMode");
    }

    public void StopXR()
    {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Debug.Log("XR stopped.");
        Debug.Log("Deinitializing XR...");
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR deinitialized.");
    }

    // Update is called once per frame

}
