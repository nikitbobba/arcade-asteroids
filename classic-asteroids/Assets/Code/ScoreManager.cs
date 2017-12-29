﻿using System;
using Assets.Code.Structure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    /// <summary>
    /// Keeps track of the player's current score.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoreManager : MonoBehaviour, ISaveLoad
    {
        public int CurrentScore { get; private set; }
        private static Text _scoreText;

        // ReSharper disable once UnusedMember.Global
        internal void Start () {
            _scoreText = GetComponent<Text>();
            CurrentScore = 0;
            UpdateScore();
        }

        public void AddScore (int value) {
            CurrentScore = Mathf.Max(0, CurrentScore + value);
            UpdateScore();
        }

        private void UpdateScore () {
            _scoreText.text = string.Format("{0}", CurrentScore).PadLeft(4, '0');
        }

        #region saveload
        public GameData OnSave () {
            // TODO fill me in

            //throw new NotImplementedException();
            
            ScoreData scoreData = new ScoreData();
            scoreData.Score = CurrentScore;
            return scoreData;
        }

        public void OnLoad (GameData data) {
            // TODO fill me in

            //throw new NotImplementedException();

            CurrentScore = ((ScoreData) data).Score;
            UpdateScore();
        }
        #endregion
    }

    /// <summary>
    /// The data from the ScoreManager to save in the save file.
    /// </summary>
    public class ScoreData : GameData
    {
        public int Score;
    }
}
