using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabAuth : MonoBehaviour
{
    #region Attributes
    [Header("InputField Login")]
	[Space(10)]
	public InputField inputLoginUserName;
	public InputField inputLoginPassword;

	[Header("InputField Register")]
	[Space(10)]
	public InputField inputRegisterUserName;
	public InputField inputRegisterPassword;
	public InputField inputRegisterEmail;

	[Header("InputField ForgetPassword")]
	[Space(10)]
	public InputField inputForgetPassword;

	[Header("EULA variables")]
	[Space(10)]
	public GameObject scrollPanelEulaConditions;
	public Toggle toggleEula;
	public Button buttonEulaConditions;
	public GameObject buttonReadEula;

	[Header("Account recovery")]
	[Space(10)]
	public Text textResetPassword;
	public Button buttonResetPassword;

	[Header("Text Alerts")]
	[Space(10)]
	public TextMeshProUGUI TextNeedAcceptEula;
	public Text TextAlertLogin;
	public TextMeshProUGUI TextAlertRegister;

	[Header("FlowEventSystem")]
	[Space(10)]
	[HideInInspector] public bool inputFocus;
	[HideInInspector] public Selectable next;



	[Header("Reset Password Users")]
	public string PlayFab_TitleId_ResetPassword;
	public static string static_TitleId_ResetPassword;
	
	private LoginWithPlayFabRequest loginRequest;
	private LoginWithCustomIDRequest loginCustomID;

	public static string myName;

	public static PlayFabAuth instance;

	public GameObject loadingPanel;


	private EventSystem system;

    #endregion

    #region Unity Callbacks
    void Start()
    {
		system = EventSystem.current;// EventSystemManager.currentSystem;
		static_TitleId_ResetPassword = PlayFab_TitleId_ResetPassword;
		buttonResetPassword.onClick.AddListener(() => { OnForgetPasswordClicked(inputForgetPassword.text); });
	}

	void Update()
	{
		FlowEventSystem();
	}

	public void FlowEventSystem()
	{
		//esta funcion hace que se pueda tabular entre inputs y pulsar enter para confirmar
		if (inputRegisterEmail.gameObject.activeInHierarchy || inputForgetPassword.gameObject.activeInHierarchy)
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (inputLoginUserName.isFocused || inputRegisterUserName.isFocused)
				{
					inputFocus = true;
				}
				else if (inputRegisterEmail.isFocused)
				{
					inputFocus = false;
				}
			}

			if (!buttonResetPassword.gameObject.activeInHierarchy)
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					RegisterConfirm();
				}
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					OnForgetPasswordClicked(inputForgetPassword.text);
				}
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (inputLoginUserName.isFocused || inputRegisterUserName.isFocused)
				{
					inputFocus = true;
				}
				else if (inputLoginPassword.isFocused || inputRegisterPassword.isFocused)
				{
					inputFocus = false;
				}
			}


			if (Input.GetKeyDown(KeyCode.Return))
			{
					ConfirmLogin();
			}
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (inputFocus == true)
			{
				next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			}
			else
			{
				next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
			}


			if (next != null)
			{
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null)
					inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
		}

		if (toggleEula.isOn == true)
		{
			TextNeedAcceptEula.gameObject.SetActive(false);
		}
	}

    #endregion

    #region Methods

    public void ConfirmLogin()
	{
		loginRequest = new LoginWithPlayFabRequest();
		loginRequest.Username = inputLoginUserName.text;
		loginRequest.Password = inputLoginPassword.text;
		myName = inputLoginUserName.text;

		//StartCoroutine(GetDataBaseCheckUserConnected(DBQuery.instance.CheckUserConnectedURL + "/?checkUser=" + inputLoginUserName.text.ToLower()));
	}

    #region Get if User is conected in oher device
	/*
    IEnumerator GetDataBaseCheckUserConnected(string uri)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError)
			{
				Debug.Log("NETWORK ERROR");
			}
			else 
			{
				if (uri.Contains("CheckUsersConnected.php"))
				{
					DBQuery.instance.resultCheckUser = webRequest.downloadHandler.text;

					descryptedResultCheckUserConnected =  encryptedScript.DecryptString(DBQuery.instance.resultCheckUser, encryptedScript.key, encryptedScript.iv);

					if (descryptedResultCheckUserConnected == "No_Exist")
					{
						PlayFabClientAPI.LoginWithPlayFab(loginRequest, result => {

							TextAlertLogin.text = "";
							myName = inputLoginUserName.text;
							if (!BanList.Contains(myName.ToLower()))
							{
								PanelBanInfo.SetActive(false);

								//if the acount is found
								Debug.Log("You are now logged in!");
								loadingPanel.SetActive(true);
								Invoke(nameof(LoadNextScene), 1);
							}
							else
							{
								//panel de info cuenta baneada
								PanelBanInfo.SetActive(true);
							}

						}, error => {

							if (TextAlertLogin.text != null)
							{
								TextAlertLogin.gameObject.SetActive(true);

								if (error.ToString().Contains("User not found"))
								{
									TextAlertLogin.text = alertLogin.SituationalTextSelection(0);
								}

								if (error.ToString().Contains("Invalid username or password"))
								{
									TextAlertLogin.text = alertLogin.SituationalTextSelection(1);
								}

								if (error.ToString().Contains("The Password field is required"))
								{
									TextAlertLogin.text = alertLogin.SituationalTextSelection(2);
								}

								if (error.ToString().Contains("Username must be between 3 and 20 characters"))
								{
									TextAlertLogin.text = alertLogin.SituationalTextSelection(3);
								}
							}
							Debug.Log(error.GenerateErrorReport());

						}, null);
					}
					else if (descryptedResultCheckUserConnected == "Exist")
					{
						if (TextAlertLogin.text != null)
						{
							TextAlertLogin.gameObject.SetActive(true);
							TextAlertLogin.text = "Este cuenta esta actualmente en uso en estos momentos.";
						}
					}
				}
			}
		}
    }
	*/
	#endregion

	void LoadNextScene()
	{
		//SceneManager.LoadScene(nextScene.ToString());
	}

	void RegisterConfirm()
	{
		if (toggleEula.isOn == true)
		{
			RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
			request.Email = inputRegisterEmail.text;
			request.RequireBothUsernameAndEmail = false;
			request.Username = inputRegisterUserName.text;
			request.Password = inputRegisterPassword.text;
			request.DisplayName = inputRegisterUserName.text;
			myName = inputRegisterUserName.text;


			PlayFabClientAPI.RegisterPlayFabUser(request, result =>
			{
				TextAlertRegister.gameObject.SetActive(true);

				TextAlertRegister.text = "Conecting...";
				loadingPanel.SetActive(true);
				Invoke(nameof(LoadNextScene), 1);
				

			}, error =>
			{
				Debug.Log(error.GenerateErrorReport());
				TextAlertRegister.gameObject.SetActive(true);

				if (error.ToString().Contains("Invalid input parameters"))
				{
					TextAlertRegister.text = "Invalid input parameters";
				}

				if (error.ToString().Contains("Password must be between 6 and 100 characters"))
				{
					TextAlertRegister.text = "Password must be between 6 and 100 characters";
				}

				if (error.ToString().Contains("Email address not available"))
				{
					TextAlertRegister.text = "Email address not available";
				}

				if (error.ToString().Contains("Username not available"))
				{
					TextAlertRegister.text = "Username not available";
				}

				if (error.ToString().Contains("Username must be between 3 and 20 characters"))
				{
					TextAlertRegister.text = "Username must be between 3 and 20 characters";
				}

				if (error.ToString().Contains("Username contains invalid characters."))
				{
					TextAlertRegister.text = "Username contains invalid characters.";
				}

				if (error.ToString().Contains("The display name entered is not available."))
				{
					TextAlertRegister.text = "The display name entered is not available.";
				}
			});
		}
		else
		{
			TextNeedAcceptEula.gameObject.SetActive(true);
		}
	}

	void ClearFields()
	{
		inputLoginUserName.text = "";
		inputLoginPassword.text = "";

		inputRegisterUserName.text = "";
		inputRegisterPassword.text = "";
		inputRegisterEmail.text = "";

		inputForgetPassword.text = "";
	}

	void OnForgetPasswordClicked(string emailText)
	{
		SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest()
		{
			Email = emailText,
			TitleId = static_TitleId_ResetPassword
		};

		PlayFabClientAPI.SendAccountRecoveryEmail(request,
			(result) =>
				{
					Debug.Log("Success. ");
					textResetPassword.gameObject.SetActive(true);
					textResetPassword.text = "Hemos enviado un reset de tu contraseña a tu correo electrónico!";
				},
			(error) =>
				{
					Debug.Log("Error. " + error.GenerateErrorReport());
					textResetPassword.gameObject.SetActive(true);
					textResetPassword.text = "El correo electrónico introducido no existe!";

					
				}
			);
	}
    #endregion
}

