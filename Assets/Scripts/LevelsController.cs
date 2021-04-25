using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Object = System.Object;

public class LevelsController : MonoBehaviour
{
    public List<Transform> Levels;
    [ReadOnly] private int NextLevel = 0;
    [ReadOnly] private int NextLevelUnclamped = 0;
    
    private static LevelsController _instance;
    
    public static LevelsController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelsController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadNextLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoLevelDown();
        }
    }

    public void LoadNextLevel()
    {
        float levelY = 0f;

        if (transform.childCount == 2)
        {
            var toDelete = transform.GetChild(0);
            toDelete.parent = null;
            Destroy(toDelete.gameObject);
        }
        
        if (transform.childCount == 1)
        {
            var prevLevelY = Mathf.Abs(transform.GetChild(0).position.y);
            var prevLevelH = transform.GetChild(0).Find("Background").GetComponent<SpriteRenderer>().size.y;
            levelY = prevLevelY + prevLevelH;
        }

        var level = Instantiate(Levels[NextLevel], 
            new Vector3(0, -levelY, 0), 
            Quaternion.identity, 
            gameObject.transform);

        level.name = "Level_" + NextLevelUnclamped;
        
        NextLevel = (NextLevel + 1) % NumLevels;
        NextLevelUnclamped++;
    }

    public void GoLevelDown()
    {
        LoadNextLevel();

        CameraController camCtrl = Camera.main.GetComponent<CameraController>();
        if (camCtrl)
        {
            //HideLevel(CurrentLevel);
            camCtrl.ScrollTo(GetLevelY(NextLevelUnclamped - 1) + 5);
            //RevealLevel(CurrentLevel);
        }
    }

    public int NumLevels => Levels.Count;

    public float GetLevelY(int levelIndex)
    {
        return transform.Find("Level_" + levelIndex).position.y;
        return Levels[GetClampedLevelIndex(levelIndex)].position.y;
    }

    public void RevealLevel(int levelIndex)
    {
        var levelRevealer = Levels[GetClampedLevelIndex(levelIndex)].GetComponentInChildren<LevelRevealer>();
        if (levelRevealer)
        {
            levelRevealer.Reveal();
        }
    }
    
    public void HideLevel(int levelIndex)
    {
        var levelRevealer = Levels[GetClampedLevelIndex(levelIndex)].GetComponentInChildren<LevelRevealer>();
        if (levelRevealer)
        {
            levelRevealer.Hide();
        }
    }

    private int GetClampedLevelIndex(int levelIndex)
    {
        return Mathf.Clamp(levelIndex, 0, Levels.Count);
    }
}
