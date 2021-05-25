using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.SceneManagement;

public class ParseRecipe : MonoBehaviour
{

    string enteredText;
    // Update is called once per frame
    public void parseText()
    {
        enteredText = GetComponent<TMP_InputField>().text;
        Recipe newRecipe = new Recipe();
        newRecipe.Steps = enteredText.Split(';');
        RecipesList.jsonfile = newRecipe;
        SceneManager.LoadScene("Record");
    }
}
