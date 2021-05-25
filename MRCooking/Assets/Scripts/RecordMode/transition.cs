using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transition : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Record", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
