using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("OBJECTS")]
        public Transform loadingBar;
        public Transform textPercent;

        //[Header("VARIABLES (IN-GAME)")]
        //public bool isOn;
        //public bool restart;
        //[Range(0, 1)] public float currentPercent;
        //[Range(0, 100)] public int speed;

        [Header("SPECIFIED PERCENT")]
        //public bool enableSpecified;
        //public bool enableLoop;
        [Range(0, 1)] public float specifiedValue;

        Image _percentImage;
        Image percentImage
        {
            set { _percentImage = value; }
            get
            {
                if (_percentImage == null)
                    _percentImage = loadingBar.GetComponent<Image>();

                return _percentImage;
            }
        }
        TextMeshProUGUI _text;
        TextMeshProUGUI text
        {
            set { _text = value; }
            get
            {
                if (_text == null)
                    _text = textPercent.GetComponent<TextMeshProUGUI>();

                return _text;
            }
        }

        public void Refresh()
        {
            percentImage.fillAmount = specifiedValue;
            text.text = string.Format("{0}%", (int)(specifiedValue * 100));
            text.color = specifiedValue > 0.5f ? Color.white : Color.gray;
        }
        void Update()
        {
            //if (currentPercent <= 1 && isOn == true && enableSpecified == false)
            //{
            //    currentPercent += speed * Time.deltaTime;
            //}

            //if (currentPercent <= 1 && isOn == true && enableSpecified == true)
            //{
            //    if (currentPercent <= specifiedValue)
            //    {
            //        currentPercent += speed * Time.deltaTime;
            //    }

            //    if (enableLoop == true && currentPercent >= specifiedValue)
            //    {
            //        currentPercent = 0;
            //    }
            //}

            //if (currentPercent == 1 || currentPercent >= 1 && restart == true)
            //{
            //    currentPercent = 0;
            //}

            //if (enableSpecified == true && specifiedValue == 0)
            //{
            //    currentPercent = 0;
            //}

            
        }
    }
}