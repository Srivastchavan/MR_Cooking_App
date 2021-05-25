using NatCorder.Clocks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordSteps : MonoBehaviour
{
    // Start is called before the first frame update
    public Recipe recipe;
    public Recipe newRecipe;
    int pos = 0;

    [SerializeField]
    public TMPro.TextMeshProUGUI Text;

    [SerializeField]
    public string curr = "Pasta";
    [SerializeField]
    public string newRecipeName = "New Recipe";

    TextAsset jsonTextFile;
    List<string> stepList = new List<string>();
    List<float> timeStampList = new List<float>();
    RealtimeClock recordingClock;
    // Start is called before the first frame update
    void Awake()
    {
        recipe = RecipesList.jsonfile;
        Debug.Log("Recipe Name: " + recipe.Name);
        newRecipe.Name = newRecipeName;
        recordingClock = new RealtimeClock();
        if (recipe.Steps.Length > 0)
        {
            Text.SetText(recipe.Steps[pos]);
            stepList.Add("Gear up!");
            timeStampList.Add(recordingClock.Timestamp);
            stepList.Add(recipe.Steps[pos]);
        }

    }

    public void Next()
    {
        if (pos < recipe.Steps.Length - 1)
        {
            Text.SetText(recipe.Steps[pos + 1]);
            stepList.Add(recipe.Steps[pos + 1]);
            timeStampList.Add(recordingClock.Timestamp);
            pos += 1;
        }
    }

    // Not used
    public void Previous()
    {
        if (pos > 0)
        {
            Text.SetText(recipe.Steps[pos - 1]);
            pos -= 1;
        }
    }

    public void FinishRecording()
    {
        newRecipe.Steps = stepList.ToArray();
        newRecipe.timeSteps = timeStampList.ToArray();
        // Debug.Log(timeStampList.ToArray()[2]);
    }

    public void goBack(){
        print("goBack called");
        Debug.Log("goBack called");
        SceneManager.LoadScene("RecipeList");
    }
}
