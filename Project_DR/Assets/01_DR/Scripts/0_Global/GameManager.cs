using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Layer
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    // 3 == null,
    Water = 4,
    UI = 5,
    Grabbable = 9,
    Drill = 11,
    Monster = 12,
    MonsterWall = 13,
    Player = 14,
    MapObject = 19,
    BattleRoomFloor = 20
}       // Layer

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 생성 후 할당
                GameObject obj = new GameObject("GameManager");
                m_instance = obj.AddComponent<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수    


    [Header("Player Object")]
    public GameObject player;
    private ScreenFader fader;
    private InputBridge input;
    private ScreenText screenText;

    [Header("Game Over")]
    public string gameoverText;
    public string gameoverScene;

    private string _playerID; // SetPlayerID(string id) 메서드로 설정함
    public string PlayerID => _playerID;


    [Header("Dungeon")]
    // ----------------------------------------------- SG ------------------------------------------------


    public int nowFloor = 1;        // 현재 몇층인지 알려줄 변수

    public static List<bool> isClearRoomList;       // 모든 방의 클리어 여부를 관리해줄 List

    private bool isClear = false;

    public bool IsClear
    {
        get { return isClear; }
        set
        {
            if (isClear != value)
            {
                isClear = value;
                DoorControll(IsClear);
            }
        }
    }

    public event System.Action DoorOnEvent;
    public event System.Action DoorOffEvent;                



    // ----------------------------------------------- SG ------------------------------------------------

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (m_instance == null)
        {

            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AwakeInIt();
    }

    void Start()
    {
        // 데이터 가져오기
        GetData();

        // 플레이어 찾아오기
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            input = player.transform.parent.GetComponent<InputBridge>();
        }
        else
        {
            Debug.Log("플레이어를 찾지 못했습니다.");
        }



    }       // Start()    

    private void OnLevelWasLoaded()
    {
       // Debug.Log("객체의 첫 생성일때에도 이게 호출이 되나?");
        // 데이터 가져오기
        GetData();

        // 플레이어 찾아오기
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            input = player.transform.parent.GetComponent<InputBridge>();
        }
        else
        {
            Debug.Log("플레이어를 찾지 못했습니다.");
        }

    }


    /// <summary>
    /// Awake사이클에서 초기화해야하는것 초기화하는 함수
    /// </summary>
    private void AwakeInIt()
    {
        if(isClearRoomList == null || isClearRoomList == default)
        {
            isClearRoomList = new List<bool>();
        }

        

    }       // AwakeInIt()


    /// <summary>
    /// 문을 열고 닫는 함수를 하나로 묶은것
    /// </summary>
    /// <param name="_isDoorOn">문을 열지 닫을지 bool값</param>
    private void DoorControll(bool _isDoorOn)
    {
        if(_isDoorOn == true)
        {
            DoorOn();
        }
        else if(_isDoorOn == false)
        {
            DoorOff();
        }
    }       // DoorControll()

    private void DoorOn()
    {
        DoorOnEvent.Invoke();
    }       // DoorOn()

    private void DoorOff()
    {
        DoorOffEvent.Invoke();
    }       // DoorOff()



    // 게임오버
    public void GameOver()
    {
        // 스크린 페이더 가져오기
        if (Camera.main)
        {
            fader = Camera.main.transform.GetComponent<ScreenFader>();
        }

        fader.DoFadeIn();
        screenText = player.GetComponent<ScreenText>();
        screenText.OnScreenText(gameoverText);
        input.enabled = false;


        Invoke(nameof(GameOverScene),5f);
    }

    // 현재 씬 리셋
    public void ResetScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // 게임오버시 씬 이동
    public void GameOverScene()
    {
        SceneManager.LoadScene(gameoverScene);
    }
        

    // 데이터 가져오기
    public void GetData()
    {
        gameoverText = (string)DataManager.instance.GetData(1001, "GameOverText", typeof(string));
    }

    // 아이디 가져오기
    public void SetPlayerID(string id)
    {
        _playerID = id;
    }
}       // ClassEnd
