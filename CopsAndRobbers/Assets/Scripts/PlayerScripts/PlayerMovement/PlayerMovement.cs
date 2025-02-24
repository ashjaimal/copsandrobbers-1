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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace Me.DerangedSenators.CopsAndRobbers
{
    /// <summary>
    /// Script that controls Player Movement
    /// </summary>
    /// @authors Nisath Mohamed Nasar
    /// @authors Hanzalah Ravat
    public class PlayerMovement : NetworkBehaviour
    {
        public Animator animator;       // reference the animator object within unity
        public float moveSpeed = 20f;    // Player speed variable 5
        public Rigidbody2D rigidBody;   // Variable for character model
        private Vector2 movement;       // Stores movement value
        public float runSpeed = 300f;    //40f
        private Vector2 touchOrigin = -Vector2.one;    //Used to store location of screen touch origin for mobile controls.
        public AudioClip movementClip; // The movement sound.
        private AudioSource movementAudioSource; // This audio source is to be assigned to with the movement sound
        private GameObject sfxHandler;

        //private float horizontalMove = 0f;
        [ClientCallback]
        void Update()
        {
            if (isLocalPlayer)
            {
                #if UNITY_STANDALONE || UNITY_WEBPLAYER
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
                #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
                movement.x = ControlContext.Instance.MovementStick.Horizontal;
                movement.y = ControlContext.Instance.MovementStick.Vertical;
                #endif //End of mobile platform dependendent compilation section started above with #elif
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Speed", movement.sqrMagnitude);
            }
        }

        /// Update on timer not frames
        [ClientCallback]
        void FixedUpdate()
        {
            //Movement
            if (isLocalPlayer)
            {
                rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
                

                if (movementAudioSource == null) //create audio source
                {
                    sfxHandler = GameObject.FindGameObjectWithTag("SFX");
                    movementAudioSource = sfxHandler.AddComponent<AudioSource>();
                    movementAudioSource.clip = movementClip;
                    movementAudioSource.enabled = false;
                    movementAudioSource.enabled = true; //re-enable for playonawake
                    //settings
                    movementAudioSource.loop = true;
                    movementAudioSource.volume = 0.2f;
                        
                }
                if (movement != Vector2.zero) // if there is movement
                {
                    movementAudioSource.UnPause();
                }
                else
                {
                    movementAudioSource.Pause();
                }
            }
        }

        public Vector3 GetPosition() 
        {
            return transform.position;
        }
    }
}
