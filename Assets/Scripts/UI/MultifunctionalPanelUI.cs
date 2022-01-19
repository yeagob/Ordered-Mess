using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using TMPro;

public class MultifunctionalPanelUI : MonoBehaviour {

    [Header("Texts")]
    [SerializeField]
    TextMeshProUGUI messageText;
    [SerializeField]
    TextMeshProUGUI tittleText;

    [Header("Panels")]
    [SerializeField]
    GameObject inputNamePanel;
    [SerializeField]
    GameObject modalPanel;
    [SerializeField]
    GameObject sandClockPanel;

    [Header("Buttons")]
    [SerializeField]
    Button yesButton;
    [SerializeField]
    Button noButton;
    [SerializeField]
    Button errorButton;

    Animator _anim;
    Animator anim
    {
        set { _anim = value; }
        get
        {
            if (_anim == null)
                _anim = GetComponentInChildren<Animator>();

            return _anim;
        }
    }

    //Input Field
    public InputField inputField;

    private readonly string DEFAULT_TITTLE = "INFORMATION";

    public void ShowModalMode (string message, int time = 0)
    {
        DisableAll();
        StartCoroutine(ActivatePopup(false));
        messageText.text = message;

        //Close By Time, no buttons
        if (time != 0)
        {
            StartCoroutine(Close(time));        
        }
        else
        {
            //Close by button Ok
            yesButton.onClick.AddListener(HideModal);
            ActiveAndChangeTextButton(yesButton, "Ok");        
        }
    }

    public void ShowModalMode(string message, UnityAction callBackActionOk)
    {
        ShowModalMode(message);     
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(callBackActionOk);
        yesButton.onClick.AddListener(HideModal);
    }

    public void ShowYesNoMode(string message, UnityAction callBackActionYes, float delay)
    {        

        StartCoroutine(ShowYesNoModeCo(message,callBackActionYes, delay));
    }

    IEnumerator ShowYesNoModeCo (string message, UnityAction callBackActionYes, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowYesNoMode(message, callBackActionYes);
    }


    public void ShowYesNoMode(string message, UnityAction callBackActionYes, UnityAction callBackActionNo)
    {
        ShowYesNoMode(message);
        yesButton.onClick.AddListener(callBackActionNo);
        noButton.onClick.AddListener(callBackActionNo);
    }   

    public void ShowYesNoMode(string message, UnityAction callBackActionYes)
    {
        ShowYesNoMode(message);
        yesButton.onClick.AddListener(callBackActionYes);        
        noButton.onClick.AddListener(HideModal);

    }

    /// <summary>
    /// Base method to show Yes/No Modal
    /// </summary>
    /// <param name="message"></param>
    private void ShowYesNoMode(string message)
    {
        DisableAll();
        StartCoroutine(ActivatePopup(false));
        messageText.text = message;

        //Yes/no buttons
        ActiveAndChangeTextButton(yesButton, "Yes");
        ActiveAndChangeTextButton(noButton, "No");
    }

    public void ShowInputNameMode(UnityAction callBackActionYes)
    {
        ShowInputNameMode("Enter Name:", callBackActionYes);

    }

    public void ShowInputNameMode(string tittle, UnityAction callBackActionYes, bool showCancel = true)
    {
        DisableAll();
        StartCoroutine(ActivatePopup(true));
        tittleText.text = tittle;
        

        //Save config
        ActiveAndChangeTextButton(yesButton, "Save");
        ActiveAndChangeTextButton(noButton, "Cancel");
        
        if (!showCancel)
            noButton.gameObject.SetActive(false);

        yesButton.onClick.AddListener(callBackActionYes);
        yesButton.onClick.AddListener(HideModal);
        noButton.onClick.AddListener(HideModal);

    }

    void DisableAll ()
    {
        //Delete old corroutines
        StopAllCoroutines();

        //Remove listeners
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        //Disble elements
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        modalPanel.gameObject.SetActive(false);
        inputNamePanel.gameObject.SetActive(false);
        sandClockPanel.gameObject.SetActive(false);
        errorButton.gameObject.SetActive(false);

        //Default Text
        tittleText.text = DEFAULT_TITTLE;
    }
    IEnumerator ActivatePopup(bool inputMode)
    {
        yield return new WaitForSeconds(0.1f);

        //Show panel
        anim.Play("Fade-in");
              

        if (inputMode)
            inputNamePanel.gameObject.SetActive(true);
        else
            modalPanel.gameObject.SetActive(true);


    }

    IEnumerator Close (int time)
    {
        sandClockPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        HideModal();
    }

    public void HideModal()
    {
        //Show panel
        anim.Play("Fade-out");
    }

    void ActiveAndChangeTextButton(Button btn, string txt)
    {
        btn.gameObject.SetActive(true);
        foreach (TextMeshProUGUI tmp in btn.GetComponentsInChildren<TextMeshProUGUI>())
            tmp.text = txt;
    }

    internal void ShowNameExist()
    {
        errorButton.gameObject.SetActive(true);
    }
}
