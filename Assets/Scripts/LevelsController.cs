using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Object = System.Object;

public class LevelsController : MonoBehaviour
{
    public List<Transform> Levels;
    public int LevelsGoal = 0;
    
    [ReadOnly] private int NextLevel;
    [ReadOnly] private int NextLevelUnclamped;
    
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
        LevelsGoal = Math.Max(LevelsGoal, Levels.Count);
        Init();
    }

    private void Init()
    {
        NextLevel = 0;
        NextLevelUnclamped = 0;
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

        if (NextLevelUnclamped < LevelsGoal)
        {
            if (transform.childCount == 2)
            {
                var toDelete = transform.GetChild(0);
                toDelete.parent = null;
                StartCoroutine(DeferredDestroyChild(toDelete));
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
        else TriggerEndgame();
    }

    public void GoLevelDown()
    {
        LoadNextLevel();

        CameraController camCtrl = Camera.main.GetComponent<CameraController>();
        if (camCtrl)
        {
            int currentLevelIndex = NextLevelUnclamped - 1;
            float topOffset = transform.Find("Level_" + currentLevelIndex).GetComponent<LevelController>().cameraBottomOffset;
            
            //HideLevel(CurrentLevel);
            camCtrl.ScrollTo(GetLevelY(currentLevelIndex) + topOffset);

            // WIP for perfomance: reposition all objects to y=0
            /*Vector3 cameraPos = camCtrl.transform.position;
            float camDiff = cameraPos.y;
            camCtrl.transform.position = new Vector3(cameraPos.x, 0f, cameraPos.z);

            foreach (Transform level in transform)
            {
                level.position = new Vector3(level.position.x, camDiff, level.position.z);
            }*/
            
            //RevealLevel(CurrentLevel);
        }
    }

    private void TriggerEndgame()
    {
        Debug.Log("YOU WIN MADAFACA!");
        // Para hacer restart debería ser suficiente con llamar la función Init(). 
        // ...
    }

    public int NumLevels => Levels.Count;

    public float GetLevelY(int levelIndex)
    {
        return transform.Find("Level_" + levelIndex).position.y;
    }

    IEnumerator DeferredDestroyChild(Transform child)
    {
        yield return new WaitForSeconds(3f);
        Destroy(child.gameObject);
    }

    public void RevealLevel(int levelIndex)
    {
        var levelRevealer = Levels[GetClampedLevelIndex(levelIndex)].GetComponentInChildren<LevelController>();
        if (levelRevealer)
        {
            levelRevealer.Reveal();
        }
    }
    
    public void HideLevel(int levelIndex)
    {
        var levelRevealer = Levels[GetClampedLevelIndex(levelIndex)].GetComponentInChildren<LevelController>();
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
