/* 
 *  Copyright (C) 2021 Deranged Senators
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *  
 *      http:www.apache.org/licenses/LICENSE-2.0
 *  
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Me.DerangedSenators.CopsAndRobbers
{
    /// <summary>
    /// This is a Singleton Class that can be used by other classes (Prefab Scripts) to access Mobile UI objects. It also dictates whether or not they are displayed on the screen
    /// </summary>
    /// @author Hanzalah Ravat
    public class ControlContext: MonoBehaviour
    {
        public Joystick MovementStick;
        public Joystick AttackCircleStick;
        public GameObject ControlCanvas;
        public MobileButton AttackButton;
        public MobileButton WeaponSwitchButton;
        private bool isActive;
        public bool Active => isActive;
        
        private static ControlContext _instance;
        public static ControlContext Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            isEnabled();
        }


#if UNITY_STANDALONE || UNITY_WEBPLAYER
        private void isEnabled()
        {
            if(ControlCanvas != null)
                ControlCanvas.SetActive(false);
            isActive = false;
        }
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private void isEnabled()
        {
            if(ControlCanvas != null)
                ControlCanvas.SetActive(true);
            isActive = true;
            Debug.Log("Game is in Mobile Mode");
        }
        #endif
    }
}