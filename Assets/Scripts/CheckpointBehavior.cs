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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.GetComponent<PlayerHealth>() != null) 
           other.GetComponent<PlayerHealth>().isSafe = true;

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
}
