using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UIElements;
using Object = System.Object;

public class LevelsController : MonoBehaviour
{
    public List<Transform> Levels;
    public GameObject Player;
    public Transform finalLevel;
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
        //DontDestroyOnLoad(gameObject);
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
            ;
            float currentTopOffset = GetLevelOffset(currentLevelIndex);

            float currentLevelY = GetLevelY(currentLevelIndex);
            float currentLevelH = GetLevelH(currentLevelIndex);

            camCtrl.ScrollTo(currentLevelY + currentTopOffset);
            camCtrl.ZoomTo(currentLevelH + currentTopOffset);
        }
    }

    private void TriggerEndgame()
    {
        float levelY = 0f;

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

        var level = Instantiate(finalLevel,
            new Vector3(0, -levelY, 0),
            Quaternion.identity,
            gameObject.transform);

        level.name = "Level_" + NextLevelUnclamped;
        
        // Disable level elements and platforms (VERY BAD DONE)
        transform.GetChild(0).Find("Elements").gameObject.SetActive(false);
        transform.GetChild(0).Find("Platforms").gameObject.SetActive(false);
        transform.GetChild(0).Find("Checkpoints").gameObject.SetActive(false);
        
        GameObject.Find("Slider").SetActive(false);
        
        Camera.main.GetComponent<CameraController>().ZoomTo(1f);

        StartCoroutine(GrowingLight());

        NextLevel = (NextLevel + 1) % NumLevels;
        NextLevelUnclamped++;
    }

    private IEnumerator GrowingLight()
    {
        float initialGlowRadius = Player.GetComponentInChildren<Light2D>().pointLightOuterRadius;
        float time = 0f;
        
        yield return new WaitForSeconds(4f);
        
        while (Player.GetComponentInChildren<Light2D>().pointLightOuterRadius < 10f)
        {
            Player.GetComponentInChildren<Light2D>().pointLightOuterRadius = Mathf.Lerp(initialGlowRadius, 20f, time / 3f);
            time += Time.deltaTime;
            
            yield return new WaitForSeconds(0);
        }
    }

    public int NumLevels => Levels.Count;

    public int CurrentLevel => NextLevelUnclamped - 1;

    public float GetLevelY(int levelIndex)
    {
        return transform.Find("Level_" + levelIndex).position.y;
    }

    public float GetLevelH(int levelIndex)
    {
        return transform.Find("Level_" + levelIndex).Find("Background").GetComponent<SpriteRenderer>().size.y;
    }

    public float GetLevelOffset(int levelIndex)
    {
        return transform.Find("Level_" + levelIndex).GetComponent<LevelController>().cameraBottomOffset;
    }

    IEnumerator DeferredDestroyChild(Transform child)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(child.gameObject);
    }
    
    public void RestoreLevel() {

        int currLevel = NextLevelUnclamped -1;
        Transform temp = transform.Find("Level_" + currLevel).Find("Platforms");
        for (int i = 0; i < temp.childCount; i++)
        {
            temp.GetChild(i).gameObject.SetActive(true);
        }

        temp = transform.Find("Level_" + currLevel).Find("Elements");
        for (int i = 0; i < temp.childCount; i++)
        {
            temp.GetChild(i).gameObject.SetActive(true);
        }
    }

    private int GetClampedLevelIndex(int levelIndex)
    {
        return Mathf.Clamp(levelIndex, 0, Levels.Count);
    }
}
