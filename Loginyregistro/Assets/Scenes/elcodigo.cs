using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;



public class AuthHandle : MonoBehaviour
{
    public TMP_InputField UsernameInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField ScoreInputField;
    public string URL = "https://sid-restapi.onrender.com/api/";



    private string token;
    private string username;
    // Start is called before the first frame update
    void Start()
    {
        token = PlayerPrefs.GetString("token");



        if (string.IsNullOrEmpty(token))
        {
            Debug.Log("D");
        }
        else
        {
            username = PlayerPrefs.GetString("username");
            StartCoroutine(GetPerfil(username));
        }
    }



    // Update is called once per frame
    void Update()
    {

    }



    public void Register()
    {
        Data data = new Data();



        data.username = UsernameInputField.text;
        data.password = PasswordInputField.text;



        string json = JsonUtility.ToJson(data);



        StartCoroutine(SendRegister(json));
    }
    public void Login()
    {
        Data data = new Data();



        data.username = UsernameInputField.text;
        data.password = PasswordInputField.text;



        string json = JsonUtility.ToJson(data);



        StartCoroutine(SendLogin(json));
        StartCoroutine(Getviejousuario());
    }
    public void Score()
    {
        UserData data = new UserData();


        data.username = username;
        data.data = new DataUserSUperBien();
        data.data.score = int.Parse(ScoreInputField.text);

        string json = JsonUtility.ToJson(data);

        StartCoroutine(SetScore(json));


    }



    IEnumerator SendRegister(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(URL + "usuarios", json);
        request.SetRequestHeader("Content-Type", "application/json");
        request.method = "POST";
        yield return request.SendWebRequest();



        if (request.isNetworkError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);
                Debug.Log("Se Registro usuario con id " + data.usuario.id);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }
    IEnumerator SendLogin(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(URL + "auth/login", json);
        request.SetRequestHeader("Content-Type", "application/json");
        request.method = "POST";
        yield return request.SendWebRequest();



        if (request.isNetworkError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);
                Debug.Log("Inicio sesion usuario  " + data.usuario.username);
                PlayerPrefs.SetString("token", data.token);
                PlayerPrefs.SetString("username", data.usuario.username);
                username = data.usuario.username;
                token = data.token;
                Debug.Log(data.token);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }



    IEnumerator GetPerfil(string username)
    {
        UnityWebRequest request = UnityWebRequest.Get(URL + "usuarios/" + username);
        request.SetRequestHeader("x-token", token);
        yield return request.SendWebRequest();



        if (request.isNetworkError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);
            Debug.Log("El Usuario " + data.usuario.username + " es Gay");
            Debug.Log(data.usuario.data.score);




        }
    }

    IEnumerator SetScore(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(URL + "usuarios", json);
        request.SetRequestHeader("Content-Type", "application/json");

        request.method = "PATCH";
        request.SetRequestHeader("x-token", token);
        yield return request.SendWebRequest();



        if (request.isNetworkError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }


    IEnumerator Getviejousuario()
    {
        UnityWebRequest request = UnityWebRequest.Get(URL + "usuarios");
        request.SetRequestHeader("x-token", token);
        yield return request.SendWebRequest();



        if (request.isNetworkError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);
            




        }
    }

}



[System.Serializable]
public class Data
{
    public string username;
    public string password;
    public UserData usuario;
    public string token;
    public UserData[] usuarios;
}





[System.Serializable]
public class UserData
{
    public string id;
    public string username;
    public bool estado;
    public DataUserSUperBien data;
}



[System.Serializable]
public class DataUserSUperBien
{
    public int score;
}
