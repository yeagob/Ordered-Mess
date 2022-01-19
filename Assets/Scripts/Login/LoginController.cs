using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;


namespace Mannaz
{
    public class LoginController : MonoBehaviour
    {
        #region Attributes

        [Header("InputFields")]
        //Login
        public InputField inputLoginUserName;
        public InputField inputLoginPassword;
        //Register
        public InputField inputRegisterUserName;
        public InputField inputRegisterPassword;
        public InputField inputRegisterEmail;
        //Forget Pasword
        public InputField inputForgetPassword;
        [Space(10)]

        [Header("Panels")]
        [SerializeField] private GameObject loginPanel;
        [SerializeField] private GameObject registerPanel;
        [SerializeField] private GameObject forgotPanel;
        [Space(10)]

        [Header("Buttons")]
        [SerializeField] private Button loginButton;
        [SerializeField] private Button registerNowButton;
        [SerializeField] private Button forgotPassButton;
        [SerializeField] private Button registerButton;
        [SerializeField] private Button cancelRegisterButton;
        [SerializeField] private Button sendForgotPassButton;
        [SerializeField] private Button cancelForgotButton;

        [Header("Errors texts")]
        [SerializeField] private TextMeshProUGUI loginErrorTxt;
        [SerializeField] private TextMeshProUGUI registerErrorTxt;
        [SerializeField] private TextMeshProUGUI forgotPassErrorTxt;
        [SerializeField] private TextMeshProUGUI needAcceptEulaTxt;
        [Space(10)]

        //EULA
        [Header("EULA variables")]
        public GameObject scrollPanelEulaConditions;
        public Toggle toggleEula;
        public Button buttonEulaConditions;
        public GameObject buttonReadEula;
        [Space(10)]

        [Header("Reset Password Users")]
        public string PlayFab_TitleId_ResetPassword;
        public static string static_TitleId_ResetPassword;

        private LoginWithPlayFabRequest loginRequest;
        private LoginWithCustomIDRequest loginCustomID;

        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            loginButton.onClick.AddListener(() => DoLogin());
            registerNowButton.onClick.AddListener(() => ShowRegistration(true));
            forgotPassButton.onClick.AddListener(() => ShowForgotPass(true));
            registerButton.onClick.AddListener(() => DoRegistration());
            sendForgotPassButton.onClick.AddListener(() => SendForgotPassword());
            cancelRegisterButton.onClick.AddListener(() => ShowRegistration(false));
            cancelForgotButton.onClick.AddListener(() => ShowForgotPass(false));
        }
        #endregion

        #region Methods
        private void DoLogin()
        {
            bool haveErrors = false;
            //TODO: Check login data and connect

            if (!haveErrors)
            {
                //TODO: Load lobby scene
                Debug.Log(">> LOG IN");
            }
            else
            {
                Debug.Log(">> Login error");
            }
        }

        private void DoRegistration()
        {
            bool haveErrors = false;
            //TODO: Check data and make registration of the user
            if (!haveErrors)
                ShowRegistration(false);
        }

        private void SendForgotPassword()
        {
            //TODO: Check user and send email to reset password
            ShowForgotPass(false);
        }

        private void ShowRegistration(bool isVisible)
        {
            loginPanel.SetActive(!isVisible);
            registerPanel.SetActive(isVisible);
        }

        private void ShowForgotPass(bool isVisible)
        {
            loginPanel.SetActive(!isVisible);
            forgotPanel.SetActive(isVisible);
        }

        #endregion
    }
}