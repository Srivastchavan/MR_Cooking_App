using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingredients : MonoBehaviour
{


    string[] ingredients;
    int pg=0;

    GameObject[] toggles = null;
    // Start is called before the first frame update
    void Awake()
    {

        toggles = GameObject.FindGameObjectsWithTag("Toggle");
        Debug.Log("Number of toggles found: " + toggles.Length);
        Array.Sort(toggles, CompareObNames);

    }

    public void SetIngredients(string[] list)
    {
        ingredients = list;
        if (toggles == null)
        {
            pg = 0;
            toggles = GameObject.FindGameObjectsWithTag("Toggle");
            Debug.Log("Number of toggles found: " + toggles.Length);
            Array.Sort(toggles, CompareObNames);
        }
        Debug.Log("The Length of ingredients: "+ingredients.Length);
        NextPage();
    }

    // Update is called once per frame
    int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    void FillToggles(string[] items) 
    {
        int max_len = Mathf.Min(items.Length, toggles.Length);
        for(int i=0; i<max_len; i++)
        {
            toggles[i].SetActive(true);
            GameObject txtObj = toggles[i].transform.Find("Label").gameObject;
            Text txt = txtObj.GetComponent<Text>();
            txt.text = items[i];
        }
        if (toggles.Length > max_len)
        {
            for(int i=max_len; i<toggles.Length; i++)
            {
                toggles[i].SetActive(false);
            }
        }
    }

    public void NextPage()
    {
        if (pg+1 <= Mathf.Ceil(ingredients.Length * 1.0f / toggles.Length))
        {
            pg++;
            List<string> list = new List<string>();
            for(int i=(pg-1)*toggles.Length; i< Mathf.Min(ingredients.Length, pg*toggles.Length); i++)
            {
                list.Add(ingredients[i]);
            }
            String[] items = list.ToArray();
            FillToggles(items);
        }
    }

    public void PrevPage()
    {
        if(pg > 1)
        {
            Debug.Log("Previous page");
            pg--;
            List<string> list = new List<string>();
            for (int i = (pg-1) * toggles.Length; i < Mathf.Min(ingredients.Length, pg * toggles.Length); i++)
            {
                list.Add(ingredients[i]);
            }
            String[] items = list.ToArray();
            FillToggles(items);
            
        }
    }
}
