using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    public enum CheckpointType
    {
        Default,
        NextLevel,
        PrevLevel
    }

    public CheckpointType type;
    public bool isLit = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() != null) 
        {
            other.GetComponent<PlayerHealth>().lastCheckpoint = this.gameObject; 
            other.GetComponent<PlayerHealth>().isSafe = true;
            LitCheckpoint();
        }


        switch (type)
       {
           case CheckpointType.NextLevel:
               LevelsController.Instance.GoLevelDown();
               break;
           
           case CheckpointType.PrevLevel:
               LevelsController.Instance.GoLevelUp();
               break;
       }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() != null) 
            other.GetComponent<PlayerHealth>().isSafe = false;
    }


    private void LitCheckpoint() 
    {
        isLit = true;
        Transform lamp = transform.Find("lit_lamp");
        lamp.gameObject.SetActive(true); 
    }

}
