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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Me.DerangedSenators.CopsAndRobbers;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


namespace Me.DerangedSenators.CopsAndRobbers
{
    /// @authors Nisath Mohamed Nasar, Piotr Krawiec and Hanzalah Ravat
    /// <summary>
    /// Detect round and move player to set map locations
    /// </summary>
    public class RoundManager : MonoBehaviour
    {
        [SerializeField] private Text currentRoundTextUI;
        private Rigidbody2D localPlayerRB;
        private int localIndex = -1;

        private RoundSpawnPosition[] _spawnPositions;

        private bool initializedSpawnPositions;
        
        public enum Round
        {
            FREEZE,
            ROUND1, 
            ROUND2, 
            ROUND3, 
            ENDED
        }

        private Round _currentRound;

        void Start()
        {
            _currentRound = Round.FREEZE;
            UpdateRoundTextView(1);

            _spawnPositions = new RoundSpawnPosition[3];
            
            
            
        }
        
        private void FixedUpdate() {
            if (localPlayerRB == null)
            {
                Debug.Log("localPlayerRB is null, trying to Player.localPlayer.GetComponent<Rigidbody2D>();");
                localPlayerRB = Player.localPlayer.GetComponent<Rigidbody2D>();
            }

            if (localIndex == -1)
            {
                localIndex = Player.localPlayer.playerIndex;
            }

            if (!initializedSpawnPositions && localIndex != -1)
            {
                InitializeSpawnPositions();
            }
        }
        
        /// <summary>
        /// This method, checks current round and loads next round accordingly.
        /// </summary>
        public void LoadRound()
        {
            switch (_currentRound)
            {
                case Round.FREEZE :
                    TransformPlayers(1);
                    UpdateRoundTextView(1);
                    _currentRound = Round.ROUND1;
                    break;
                case Round.ROUND1 :
                    TransformPlayers(2);
                    UpdateRoundTextView(2);
                    _currentRound = Round.ROUND2;
                    break;
                case Round.ROUND2 :
                    UpdateRoundTextView(3);
                    _currentRound = Round.ROUND3;
                    TransformPlayers(3);
                    break;
                case Round.ROUND3 :
                    _currentRound = Round.ENDED;
                    break;
            }
            
        }


        public void LoadFreezeRound()
        {
            //disable player components
            
        }

        /// <summary>
        /// Populate the list with spawn positions for each round
        /// </summary>
        private void InitializeSpawnPositions()
        {
            _spawnPositions[0] = new RoundSpawnPosition(new Vector2(47 + localIndex, 52), new Vector2(1 + localIndex, -56));
            _spawnPositions[1] =  new RoundSpawnPosition(new Vector2(241+localIndex,-62), new Vector2(90+localIndex,-31));
            _spawnPositions[2] =  new RoundSpawnPosition(new Vector2(-105+localIndex,-6.6f), new Vector2(-172 + localIndex, -29));

            initializedSpawnPositions = true;
        }

        private struct RoundSpawnPosition
        {
            public Vector2 CopPosition;
            public Vector2 RobberPosition;

            public RoundSpawnPosition(Vector2 copPosition, Vector2 robberPosition)
            {
                this.CopPosition = copPosition;
                this.RobberPosition = robberPosition;
            }
        }

        public void TransformPlayers(int roundNumber)
        {
            /*
            switch (roundNumber)
            {
              case 1:
                  if (Player.localPlayer.teamId == 1)
                  {
                      localPlayerRB.position = _round1SpawnPosition.CopPosition;
                  }
                  else
                  {
                      localPlayerRB.position = _round1SpawnPosition.RobberPosition;
                  }
                  break;
              case 2:
                  if (Player.localPlayer.teamId == 1)
                  {
                      localPlayerRB.position = _round2SpawnPosition.CopPosition;
                  }
                  else
                  {
                      localPlayerRB.position = _round2SpawnPosition.RobberPosition;
                  }
                  break;
              case 3:
                  if (Player.localPlayer.teamId == 1)
                  {
                      localPlayerRB.position = _round3SpawnPosition.CopPosition;
                  }
                  else
                  {
                      localPlayerRB.position = _round3SpawnPosition.RobberPosition;
                  }

                  break;
            }
            */
            if (Player.localPlayer.teamId == 1)
            {
                localPlayerRB.position = _spawnPositions[roundNumber-1].CopPosition;
            }
            else
            {
                localPlayerRB.position = _spawnPositions[roundNumber-1].RobberPosition;
            }
            
        }
        

        /// <summary>
        /// returns current round
        /// </summary>
        /// <returns></returns>
        public Round GetCurrentRound()
        {
            return _currentRound;
        }

        /// <summary>
        /// sets current round.
        /// </summary>
        /// <param name="targetRound"></param>
        public void SetRound(Round targetRound)
        {
            _currentRound = targetRound;
        }

        /// <summary>
        /// displays round text
        /// </summary>
        /// <param name="roundNumber"></param>
        private void UpdateRoundTextView(int roundNumber)
        {
            currentRoundTextUI.text = $"Round {roundNumber}/3";
        }


    }
}