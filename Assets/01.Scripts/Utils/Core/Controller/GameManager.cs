using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public static GameState GameState { get; private set; } = GameState.Play;

    public UIManager UIManager { get; private set; }
    public DataManager Data { get; private set; }


    public static int currentChapter { get; set; } = 1;
    public static int currentStage { get; set; } = 3;

    private GameObject currentStagePrefab;

    private List<Item> currentItems;
    public List<Item> CurrentItems { get => currentItems; }

    private List<Character> currentCharacters;
    public List<Character> CurrentCharacters { get => currentCharacters; }

    public Vector3 PlayerSpawnPosition
    {
        get => currentStagePrefab.transform.GetChild(0).position;
    }
    
    void Awake()
    {
        UIManager = FindObjectOfType<UIManager>();
        Data = FindObjectOfType<DataManager>();

        EventManager.StartListening(Constant.START_PLAY_EVENT, StartPlay);
        EventManager.StartListening(Constant.GET_STAR_EVENT, ClearStage);

    }
    private void Start()
    {
        ClearStage();
    }

    private void StartPlay()
    {
        Debug.Log("Play Start");
        GameState = GameState.Play;
    }

    private void ClearStage()
    {
        if (GameState != GameState.Play) return;

        if (currentStagePrefab != null)
        {
            Destroy(currentStagePrefab);
        }

        GameObject prefab = Data.LoadStage();
        currentStagePrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        currentItems = new List<Item>();
        currentCharacters = new List<Character>();

        EventManager.TriggerEvent(Constant.CLEAR_STAGE_EVENT);

        ResetStage();

        Debug.Log("Clear Stage");
    }

    public void ResetStage()
    {
        Debug.Log("Reset Stage");
        EventManager.TriggerEvent(Constant.RESET_GAME_EVENT);
        GameState = GameState.Ready;

        RegisterCurrentItem();
        UIManager.ChangeStage(currentStage);
    }

    public void RegisterCurrentItem()
    {
        for (int i = 0; i < currentItems.Count; i++)
        {
            currentItems[i].verbPairs = new Dictionary<Character, VerbType>();

            for (int j = 0; j < currentCharacters.Count; j++)
            {
                if (currentCharacters[j].characterName == currentItems[i].Name) continue;
                currentItems[i].verbPairs.Add(currentCharacters[j], VerbType.None);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Constant.START_PLAY_EVENT, StartPlay);
        EventManager.StopListening(Constant.GET_STAR_EVENT, ClearStage);
    }
}