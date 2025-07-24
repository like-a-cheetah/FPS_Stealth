using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFapManager : MonoBehaviour
{
    public LoginPan logPan;
    public RegisterPan regisPan;

    private void Start()
    {
        logPan.LoginCan.enabled = true;
        regisPan.RegisterCan.enabled = false;
    }

    public void LoginBtnActive()
    {
        logPan.condition.fontSize = 36;
        logPan.condition.text = "로그인 시도";
        var request = new LoginWithEmailAddressRequest { Email = logPan.IDInput.text, Password = logPan.PWInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        logPan.condition.fontSize = 33;
        logPan.condition.text = "로그인에 성공하셨습니다.";
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        logPan.condition.text = "로그인에 실패했습니다. 다시 입력해주세요";
        logPan.condition.fontSize = 20;
        StartCoroutine(ChangeText());
    }
    
    private IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(2f);
        logPan.condition.fontSize = 26;
        logPan.condition.text = "아이디와 패스워드를 입력하시오.";
    }

    public void ActivateRegisterPan()
    {
        logPan.LoginCan.enabled = false;
        regisPan.RegisterCan.enabled = true;
    }

    public void ActivateLoginPan()
    {
        regisPan.RegisterCan.enabled = false;
        logPan.LoginCan.enabled = true;
    }

    public void RegisterBtnActive()
    {
        regisPan.condition.fontSize = 36;
        regisPan.condition.text = "회원가입 시도";
        var request = new RegisterPlayFabUserRequest { Email = regisPan.IDInput.text, Password = regisPan.PWInput.text, Username = regisPan.NameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result) => regisPan.condition.text = "회원가입 성공";

    void OnRegisterFailure(PlayFabError erro) => regisPan.condition.text = "회원가입 실패";


    [Serializable]
    public class LoginPan
    {
        public Canvas LoginCan;
        public TMP_InputField IDInput, PWInput;
        public Text condition;
        public Button RegisterBtn, LoginBtn;
    }

    [Serializable]
    public class RegisterPan
    {
        public Canvas RegisterCan;
        public TMP_InputField IDInput, PWInput, NameInput;
        public Text condition;
        public Button RegisterBtn, BackBtn;
    }
}
