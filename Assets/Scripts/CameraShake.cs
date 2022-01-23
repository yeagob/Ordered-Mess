using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

    public class CameraShake : MonoBehaviour
    {
        private void Start()
        {
        }

        private void OnDestroy()
        {
        }

        public void Shake()
        {
            print("CameraShake!");
            transform.DOShakeRotation(1, 5, 3);
        }
    }
