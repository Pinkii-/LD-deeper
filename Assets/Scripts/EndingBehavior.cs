using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingBehavior : MonoBehaviour
{
    public GameObject bodylight;
    public GameObject headlight;
    public GameObject treelight;
    public GameObject UI;
    public bool ended = false;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas").transform.Find("Ending").gameObject;  
    }

    // Update is called once per frame
    void Update()
    {
        if (ended)
        {
            if (Input.GetKeyUp("space"))
            {
                ended = false;
                SceneManager.LoadScene("MenuScene");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() != null) 
        {
            other.GetComponent<PlayerHealth>().isSafe = true;
            other.GetComponent<PlayerController>().enabled = false;
            bodylight.SetActive(true);
            headlight.SetActive(true);
            treelight.SetActive(true);
            other.GetComponentInChildren<Animator>().SetBool("isDead", true);
            StartCoroutine(ActivateUI());
         

        }

    }


    private IEnumerator ActivateUI()
    {
        yield return new WaitForSeconds(2);
        ended = true;
        UI.SetActive(true);
    }

}


