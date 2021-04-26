using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    public float timeToGoDown = 1f;
    
    public enum CheckpointType
    {
        Default,
        NextLevel
    }

    public CheckpointType type;
    public bool isLit = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLit && type == CheckpointType.NextLevel)
        {
            StartCoroutine(DeferredGoLevelDown());
        }
        
        if (other.GetComponent<PlayerHealth>() != null) 
        {
            other.GetComponent<PlayerHealth>().lastCheckpoint = this.gameObject; 
            other.GetComponent<PlayerHealth>().isSafe = true;
            LitCheckpoint();
        }
    }

    private IEnumerator DeferredGoLevelDown()
    {
        yield return new WaitForSeconds(timeToGoDown);
        LevelsController.Instance.GoLevelDown();
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
        AudioManager.audioManagerRef.PlaySoundWithRandomPitch("fireLight");
        AudioManager.audioManagerRef.PlaySoundWithRandomPitch("fireLoop");
    }

}
