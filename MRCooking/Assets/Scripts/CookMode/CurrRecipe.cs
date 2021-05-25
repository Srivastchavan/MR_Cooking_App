using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[Serializable]
public class Recipe
{
    public string Name;
    public string[] Steps;
    public float[] timeSteps;
    public string link;
    public string s3link;
    public string[] ingredients;
    public string image;
    public float frameRate;
}

public class CurrRecipe : MonoBehaviour
{
    public Recipe recipe;
    public int pos = 0;
    TextMeshPro Text;

    [SerializeField]
    Ingredients items = null;

    [SerializeField]
    MyVideoPlayer vidPlayer;

    TextAsset jsonTextFile;
    // Start is called before the first frame update
    void Awake()
    {
        recipe = RecipesList.jsonfile;
        Debug.Log("Recipe Name: "+recipe.Name);
        Text = GetComponent<TextMeshPro>();
        if (recipe.Steps.Length > 0)
        {
            Text.SetText(recipe.Steps[pos]);
        }
        items.SetIngredients(recipe.ingredients);
        //vidPlayer.initializeVidPlayer();
        vidPlayer.GetComponent<VideoPlayer>().url = recipe.s3link;
        vidPlayer.videoIsPlaying = true;
        vidPlayer.GetComponent<VideoPlayer>().Play();
    }

    public void Next()
    {
        if (pos < recipe.Steps.Length - 1)
        {
            Text.SetText(recipe.Steps[pos + 1]);
            var frame = (recipe.timeSteps[pos+1]*recipe.frameRate);
            vidPlayer.GetComponent<VideoPlayer>().frame = (long)frame;
            pos += 1;

        }

    }

    public void Previous()
    {
        if (pos > 0)
        {
            Text.SetText(recipe.Steps[pos - 1]);
            var frame = (recipe.timeSteps[pos - 1] * recipe.frameRate);
            vidPlayer.GetComponent<VideoPlayer>().frame = (long)frame;
            pos -= 1;
        }
    }

    public void goBack(){
        print("goBack called");
        Debug.Log("goBack called");
        SceneManager.LoadScene("RecipeList");
    }
}
