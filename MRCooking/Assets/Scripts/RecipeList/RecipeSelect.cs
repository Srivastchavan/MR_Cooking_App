using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class RecipeSelect : MonoBehaviour
{
    [SerializeField]
    private Material _normalMaterial;

    [SerializeField]
    private Material _overMaterial;

    [SerializeField]
    RecipesList recipesList;

    private Renderer _gameObjectRenderer;
    private GazeTarget _gazeTargetComponent;

    public UnityEvent CallFunction;

    bool triggered = true;

    void Awake()
    {
        _gazeTargetComponent = GetComponent<GazeTarget>();
        _gameObjectRenderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _gazeTargetComponent.GazeEntered += OnGazeEntered;
        //_gazeTargetComponent.GazeOut += OnGazeOut;
    }


    private void OnDisable()
    {
        _gazeTargetComponent.GazeEntered -= OnGazeEntered;
        //_gazeTargetComponent.GazeOut -= OnGazeOut;
    }

    //Handle the OnGazeEntered event
    public void OnGazeEntered(object sender, Vector3 impactPoint)
    {
        Debug.Log("Impact point: " + impactPoint);
        gameObject.transform.Find("Image").gameObject.SetActive(true);
        if (triggered)
        {
            CallFunction.Invoke();
            Debug.Log("This is called once?");
            UpdateSelectedRecipe();
            //GameObject.Find("Image").SetActive(true);
            triggered = false;
        }
        // Change material of GameObject's renderer to "over" material
        if (_gameObjectRenderer)
        {
            _gameObjectRenderer.material = _overMaterial;
        }

    }

    public void OnGazeEntered()
    {

        gameObject.transform.Find("Image").gameObject.SetActive(true);
        if (triggered)
        {
            CallFunction.Invoke();
            Debug.Log("This is called once?");
            UpdateSelectedRecipe();
            triggered = false;
        }
        Debug.Log("selected recipe : " + RecipesList.selectedRecipe);
        // Change material of GameObject's renderer to "over" material
        if (_gameObjectRenderer)
        {
            _gameObjectRenderer.material = _overMaterial;
        }

    }

    //Handle the OnGazeOut event
    public void OnGazeOut()
    {
        // Change material to "normal" material
        if (_gameObjectRenderer)
        {
            _gameObjectRenderer.material = _normalMaterial;
        }
        triggered = true;
        gameObject.transform.Find("Image").gameObject.SetActive(false);
    }

    public void UpdateSelectedRecipe()
    {
        string rname = gameObject.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text;
        int i;
        for (i = 0; i < recipesList.recipeList.Length; i++)
        {
            if (recipesList.recipeList[i].Name == rname)
                break;
        }
        if (i != recipesList.recipeList.Length)
        {
            if (RecipesList.mode == "CookMode")
                RecipesList.selectedRecipe = recipesList.db.CookMode[i];
            else
                RecipesList.selectedRecipe = recipesList.db.RecordMode[i];
        }
        
        Debug.Log(RecipesList.selectedRecipe);
    }

}
