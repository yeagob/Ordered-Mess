using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    public class ProfileControl
    {
        public static PlayerProfile playerProfile; 

        public static void Save()
        {
            string json = JsonUtility.ToJson(playerProfile);
            PlayerPrefs.SetString("PlayerProfile", json);
            Debug.Log("Player Data Saved! " + json);
        }

        public static void Load()
        {
            if (PlayerPrefs.HasKey("PlayerProfile"))
            {
                string json = PlayerPrefs.GetString("PlayerProfile");
                playerProfile = JsonUtility.FromJson<PlayerProfile>(json);
                Debug.Log("Player Data Loaded: " + json);
            }
            else
                playerProfile = new PlayerProfile();
        }
    
    }
