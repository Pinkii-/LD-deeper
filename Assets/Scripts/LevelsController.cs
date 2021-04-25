using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    private static LevelsController _instance;

    [ReadOnly] public List<Transform> Levels;
    [ReadOnly] public int CurrentLevel = 0;

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
        foreach (Transform element in transform)
        {
            if (element.name.Contains("Level_"))
            {
                Levels.Add(element);
            }
        }
        
        RevealLevel(CurrentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GoLevelUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoLevelDown();
        }
    }

    public void GoLevelDown()
    {
        if (CurrentLevel < NumLevels - 1)
        {
            CameraController camCtrl = Camera.main.GetComponent<CameraController>();
            if (camCtrl)
            {
                HideLevel(CurrentLevel);
                CurrentLevel++;
                camCtrl.GoToLevel(CurrentLevel);
                RevealLevel(CurrentLevel);
            }
        }
    }
    
    public void GoLevelUp()
    {
        if (CurrentLevel > 0)
        {
            CameraController camCtrl = Camera.main.GetComponent<CameraController>();
            if (camCtrl)
            {
                HideLevel(CurrentLevel);
                CurrentLevel--;
                camCtrl.GoToLevel(CurrentLevel);
                RevealLevel(CurrentLevel);
            }
        }
    }

    public int NumLevels => Levels.Count;

    public float GetLevelY(int levelIndex)
    {
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
