using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;

public class AccountManager : MonoBehaviour
{

    [SerializeField] InputField idInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] InputField infoInput;
    [SerializeField] TMP_Text description;

    [SerializeField] string sceneName;

    public void LoginClick() => StartCoroutine(AccountCo("login"));

    public void RegisterClick() => StartCoroutine(AccountCo("register"));

    public void SaveClick() => StartCoroutine(AccountCo("save"));

////// 기존
    //IEnumerator AccountCo(string command)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("command", command);
    //    form.AddField("id", idInput.text);
    //    form.AddField("password", passwordInput.text);
    //    //form.AddField("mbti", infoInput.text);

    //    UnityWebRequest www = UnityWebRequest.Post(url, form);

    //    yield return www.SendWebRequest();
    //    description.text = www.downloadHandler.text;
    //    print(www.downloadHandler.text);

    //    switch (www.downloadHandler.text)
    //    {
    //        case "Login Complete":
    //            SceneManager.LoadScene("JH MainScene");
    //            break;
    //    }
      
    //}
    IEnumerator AccountCo(string command)
    {
        
        string id = idInput.text;
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id", idInput.text);
        form.AddField("password", passwordInput.text);
        form.AddField("mbti", infoInput.text);

        string url = SecureURLHandler.GetURL();

        // using문을 사용하여 메모리 누수 해결
        // 사용이 끝난 후 할당 해제
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            print(www.downloadHandler.text);

            switch (www.downloadHandler.text)
            {
                // 로그인 성송기
                case "Login Complete":
                    description.text = string.Format("로그인 성공");
                    PlayerDataManager.SetID(id);
                    SceneManager.LoadScene(sceneName);
                    break; 
                case "Fail to login":
                    description.text = string.Format("로그인 실패");
                    break; 
                case "Fail to register":
                    description.text = string.Format("계정 생성 실패");
                    break;                
                case "Register complete":
                    description.text = string.Format("계정 생성 성공");
                    break;
            }

            // using문을 사용해도 메모리 누수가 발생하여
            // 추가로 Dipose()함수를 호출해서 할당 해제
            www.Dispose();
        }
    }

    //IEnumerator AccountCo(string command)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("command", command);
    //    form.AddField("id", idInput.text);
    //    form.AddField("password", passwordInput.text);
    //    form.AddField("mbti", infoInput.text);
    //    //form.AddField("password", passwordInput.text);
    //    //form.AddField("info", infoInput.text);

    //    // using문을 사용하여 메모리 누수 해결
    //    // 사용이 끝난 후 할당 해제
    //    using (UnityWebRequest www = UnityWebRequest.Post(url, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            print(www.downloadHandler.text);
    //        }

    //        // using문을 사용해도 메모리 누수가 발생하여
    //        // 추가로 Dipose()함수를 호출해서 할당 해제
    //        www.Dispose();
    //    }
    //}
}


