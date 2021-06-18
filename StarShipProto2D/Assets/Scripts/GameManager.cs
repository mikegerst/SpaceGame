using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Managers;

public class GameManager : MonoSingleton<GameManager>
{
    // Start is called before the first frame update

    public delegate void GameStart();
    public static event GameStart OnGameStart;
    private static GameState _gameState = GameState.Start;



    
    private static GameObject _player;

    public static GameObject Player => _player;

    [SerializeField] private Sprite mouseImage;

    private enum GameState
    {
        Playing,
        Paused,
        Start
    }

    private PlayerStats stats = new PlayerStats();


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
        SetGeneralSettings();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameState)
        {
            case GameState.Start:
                if (Input.GetKey(KeyCode.Return))
                {
                    _gameState = GameState.Playing;
                    if(OnGameStart != null)
                        OnGameStart();
                }
                break;
            default:
                break;


            
        }
        if (Player == null)
            SetPlayer();
    }

    
    public void SetPlayer()
    {
        _player = GameObject.Find("UFO");
        Debug.Log("Setting Player");
    }
    public Vector2 GetPlayerPosition()
    {
        return Player.gameObject.transform.position;
    }
    
    private void SetGeneralSettings()
    {
        Cursor.SetCursor(mouseImage.texture, Vector2.zero, CursorMode.Auto);
    }

    private class PlayerStats
    {
        private GameObject player;
        public float health;
        public float collectableScore;
    }
}
