using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class PlayerDataManager
{
    /*************************************************
     *                 Public Fields
     *************************************************/
    #region [+]
    /***********************
     *      Properties
     **********************/
    public static string PlayerID => _id;
    public static float HP => _hp;
    public static int Gold => _gold;
    public static float Exp => _exp;
    public static float GoldIncrease => _gold_increase;
    public static float ExpIncrease => _exp_increase;
    public static float WeaponAtk => _weapon_atk;
    public static float WeaponCriRate => _weapon_cri_rate;
    public static float WeaponCriDamage => _weapon_cri_damage;
    public static float WeaponAtkRate => _weapon_atk_rate;
    public static float WeaponExp => _weapon_exp;
    public static int SkillLevel1 => _skill_level_1;
    public static int SkillLevel2 => _skill_level_2;
    public static int SkillLevel3 => _skill_level_3;
    public static int SkillLevel4 => _skill_level_4;
    public static string QuestMain => _quest_main;
    public static int ClearCount => _clear_count;
    public static string ClearMBTIValue => _clear_mbti_value;
    public static string ClearTime => _clear_time;

    #endregion
    /*************************************************
     *                 Private Fields
     *************************************************/
    #region [+]
    [Header("Player")]
    private static string _id = "";           // 플레이어의 ID
    private static float _hp;                 // 플레이어의 체력
    private static int _gold;                 // 플레이어의 소지금
    private static float _exp;                // 플레이어의 경험치
    private static float _gold_increase;      // 경험치 증가량
    private static float _exp_increase;       // 골드 증가량

    [Header("Weapon")]
    private static float _weapon_atk;         // 무기 공격력
    private static float _weapon_cri_rate;    // 무기 치명타 확률
    private static float _weapon_cri_damage;  // 무기 치명타 공격력
    private static float _weapon_atk_rate;    // 무기 공격 간격
    private static float _weapon_exp;         // 무기 경험치

    [Header("Skill")]
    private static int _skill_level_1;        // 스킬 테라드릴 레벨
    private static int _skill_level_2;        // 스킬 드릴연마 레벨
    private static int _skill_level_3;        // 스킬 그릴분쇄 레벨
    private static int _skill_level_4;        // 스킬 그린랜딩 레벨

    [Header("ClearData")]
    private static string _quest_main;        // 메인 퀘스트 진행도(직렬화 데이터)
    private static int _clear_count;          // 게임 클리어 횟수
    private static string _clear_mbti_value;  // 저장된 MBTI 수치
    private static string _clear_time;        // 클리어 한 날짜 및 시간

    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    /// <summary>
    /// GameManager를 트리거로 비동기로 코루틴을 실행 후, 
    /// <br></br>DB에서 데이터를 호출하고 PlayerDataManager의 데이터를 갱신한다.
    /// </summary>
    public static void Update(bool isUserDataManagerUpdate = false)
    {
        // 게임 매니저에 코루틴을 요청해서 UpdateCoroutine을 실행
        UserDataManager.Instance.StartCoroutine(UpdateCoroutine(isUserDataManagerUpdate));
    }
    /// <summary>
    /// GameManager를 트리거로 비동기로 코루틴을 실행 후, DB에 데이터를 
    /// 저장한다. <br></br>평균적으로 값이 업데이트 되는데 1~3초가 걸린다.
    /// <br></br>사용 예제는 다음과 같다
    /// <br></br>PlayerDataManager.Save("gold", "2000");
    /// <br></br>PlayerDataManager.Save("weapon_cri_rate", "300.5");
    /// </summary>
    public static void Save(string column, string value, bool isUpdate = false)
    {
        // 게임 매니저에 코루틴을 요청해서 SaveCoroutine을 실행
        UserDataManager.Instance.StartCoroutine(SaveCoroutine(column, value, isUpdate));
    }
    public static void Save(string column, object value, bool isUpdate = false)
    {
        // 게임 매니저에 코루틴을 요청해서 SaveCoroutine을 실행
        UserDataManager.Instance.StartCoroutine(SaveCoroutine(column, value.ToString(), isUpdate));
    }

    /// <summary>
    /// 플레이어의 ID를 설정한다.
    /// </summary>
    public static void SetID(string id)
    {
        _id = id;
    }
    #endregion
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    // 폼을 생성 후 반환한다.
    private static WWWForm MakeForm(string command, string id,
        string column = "", string value = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id", id);
        form.AddField("column", column);
        form.AddField("value", value);

        return form;
    }

    // PlayerDataManager의 데이터를 갱신한다.
    private static void UpdatePlayerDataManager(
        float hp, int gold, float exp, float goldIncrease, float expIncrease,
        float weaponAtk, float weaponCriRate, float weaponCriDamage, float weaponAtkRate,
        float weaponExp, int skillLevel1, int skillLevel2, int skillLevel3, int skillLevel4,
        string questMain, int clearCount, string clearMBTIValue, string clearTime
        )
    {
        // [Player]
        _hp = hp;                              // 플레이어의 체력
        _gold = gold;                          // 플레이어의 소지금
        _exp = exp;                            // 플레이어의 경험치
        _gold_increase = goldIncrease;         // 경험치 증가량
        _exp_increase = expIncrease;           // 골드 증가량

        // [Weapon]
        _weapon_atk = weaponAtk;               // 무기 공격력
        _weapon_cri_rate = weaponCriRate;      // 무기 치명타 확률
        _weapon_cri_damage = weaponCriDamage;  // 무기 치명타 공격력
        _weapon_atk_rate = weaponAtkRate;      // 무기 공격 간격
        _weapon_exp = weaponExp;               // 무기 경험치

        // [Skill]
        _skill_level_1 = skillLevel1;          // 스킬 테라드릴 레벨
        _skill_level_2 = skillLevel2;          // 스킬 드릴연마 레벨
        _skill_level_3 = skillLevel3;          // 스킬 그릴분쇄 레벨
        _skill_level_4 = skillLevel4;          // 스킬 그린랜딩 레벨

        // [ClearData]
////// TODO: 클리어 데이터의 직렬화를 변환하는 함수를 구현 및
////// 연동하기
        _quest_main = questMain;               // 메인 퀘스트 진행도(직렬화 데이터)
        _clear_count = clearCount;             // 게임 클리어 횟수
        _clear_mbti_value = clearMBTIValue;    // 저장된 MBTI 수치(직렬화 데이터)
        _clear_time = clearTime;               // 클리어 한 날짜 및 시간(직렬화 데이터)
    }

    // Data를 PlayerData에 맞게 변환한다.
    private static List<PlayerData> ConvertDataToPlayerData(string data)
    {
        // DB에서 받아온 데이터를 객체화
        string jsonData = "{\"items\": "+ data +"}";

        // PlayerData 리스트에 배열을 파싱
        List<PlayerData> playerDataList = ParseJsonArray(jsonData);

        return playerDataList;
    }

    // JSON 배열을 파싱한다.
    private static List<PlayerData> ParseJsonArray(string json)
    {
        return JsonUtility.FromJson<ListWrapper>(json).items;
    }

    // JSON 배열을 감싸는 클래스
    [Serializable]
    private class ListWrapper
    {
        public List<PlayerData> items;
    }

    // 파싱된 데이터를 PlayerDataManager에 업데이트 한다.
    private static void AssignPlayerDataToManager(
        List<PlayerData> playerDataList)
    {
        foreach (var playerData in playerDataList)
        {
            // PlayerDataManager의 정보를 업데이트
            UpdatePlayerDataManager(playerData.hp, playerData.gold, playerData.exp,
                playerData.gold_increase, playerData.exp_increase, playerData.weapon_atk,
                playerData.weapon_cri_rate, playerData.weapon_cri_damage, playerData.weapon_atk_rate,
                playerData.weapon_exp, playerData.skill_level_1, playerData.skill_level_2, playerData.skill_level_3,
                playerData.skill_level_4, playerData.quest_main, playerData.clear_count, playerData.clear_mbti_value,
                playerData.clear_time);
        }
    }

    #endregion
    /*************************************************
     *                  Coroutines
     *************************************************/
    #region [+]
    // DB에서 데이터를 가져오기 위한 비동기 코루틴
    private static IEnumerator UpdateCoroutine(bool isUserDataManagerUpdate)
    {
        // 폼 생성
        WWWForm form = MakeForm("search_all", PlayerID);

        string url = SecureURLHandler.GetURL();
        // using문을 사용하여 메모리 누수를 해결
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            // 에러가 발생했을 경우
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogWarning(www.downloadHandler.text);
            }

            // 정상일 경우
            else
            {
                // 가져온 데이터를 파싱한다.
                List<PlayerData> playerDataList = 
                    ConvertDataToPlayerData(www.downloadHandler.text);

                // 파싱된 데이터를 PlayerDataManager에 넣는다.
                AssignPlayerDataToManager(playerDataList);

                // isUserDataManagerUpdate == true
                if (isUserDataManagerUpdate)
                {
                    UserDataManager.Instance.GetDataFromDB();
                }

                // 디버그
                DebugData();

            }

            // using문을 사용해도 메모리 누수가 발생하여
            // 추가로 Dipose()함수를 호출해서 할당 해제
            www.Dispose();
        }
    }

    // DB에서 데이터를 저장하기 위한 비동기 코루틴
    private static IEnumerator SaveCoroutine(string column, string value,
        bool isUpdate)
    {
        // 폼 생성
        WWWForm form = MakeForm("add", PlayerID, column, value);

        string url = SecureURLHandler.GetURL();
        // using문을 사용하여 메모리 누수를 해결
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            // 에러가 발생했을 경우
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogWarning(www.downloadHandler.text);
            }

            // 정상적으로 처리되었을 경우
            else
            {
                Debug.Log("정상적으로 DB에 데이터가 저장되었습니다.");

                // 업데이트 요청을 받았을 경우
                if (isUpdate)
                {
                    Update();
                }
            }

            // using문을 사용해도 메모리 누수가 발생하여
            // 추가로 Dipose()함수를 호출해서 할당 해제
            www.Dispose();
        }
    }

    #endregion
    /*************************************************
     *                    Debugs
     *************************************************/
    #region [+]
    public static void DebugData()
    {
        Debug.Log($"id: {PlayerDataManager.PlayerID}, hp: {PlayerDataManager.HP}, gold: {PlayerDataManager.Gold}, " +
            $"exp: {PlayerDataManager.Exp}, gold_increase: {PlayerDataManager.GoldIncrease}, exp_increase: {PlayerDataManager.ExpIncrease}, " +
            $"weapon_atk: {PlayerDataManager.WeaponAtk}, weapon_cri_rate: {PlayerDataManager.WeaponCriRate}, weapon_cri_damage: {PlayerDataManager.WeaponCriDamage}, " +
            $"weapon_atk_rate: {PlayerDataManager.WeaponAtkRate}, weapon_exp: {PlayerDataManager.WeaponExp}, skill_level_1: {PlayerDataManager.SkillLevel1}, skill_level_2: {PlayerDataManager.SkillLevel2}, " +
            $"skill_level_3: {PlayerDataManager.SkillLevel3}, skill_level_4: {PlayerDataManager.SkillLevel4}, quest_main: {PlayerDataManager.QuestMain}, " +
            $"clear_count: {PlayerDataManager.ClearCount}, clear_mbti_value: {PlayerDataManager.ClearMBTIValue}, clear_time: {PlayerDataManager.ClearTime}");
    }

    #endregion
}
