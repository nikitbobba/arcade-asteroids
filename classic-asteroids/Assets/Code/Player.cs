using System;
using Assets.Code.Structure;
using UnityEngine;
using UnityEngine.VR.WSA;

namespace Assets.Code
{
    /// <summary>
    /// Player controller class
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Player : MonoBehaviour, ISaveLoad
    {
        private static string _fireaxis;
        private Rigidbody2D _rb;
        private Gun _gun;

        // ReSharper disable once UnusedMember.Global
        internal void Start () {
            _rb = GetComponent<Rigidbody2D>();
            _gun = GetComponent<Gun>();

            _fireaxis = Platform.GetFireAxis();
        }

        // ReSharper disable once UnusedMember.Global
        internal void Update () {
            HandleInput();
        }

        /// <summary>
        /// Check the controller for player inputs and respond accordingly.
        /// </summary>
        private void HandleInput () {
            // TODO fill me in
            var horizontalVal = Input.GetAxis("Horizontal");
            Turn(horizontalVal);

            var verticalVal = Input.GetAxis("Vertical");
            Thrust(verticalVal);

            if (Input.GetAxis(_fireaxis) == 1)
            {
                Fire();
            }

        }

        private void Turn (float direction) {
            if (Mathf.Abs(direction) < 0.02f) { return; }
            _rb.AddTorque(direction * -0.05f);
        }

        private void Thrust (float intensity) {
            if (Mathf.Abs(intensity) < 0.02f) { return; }
            _rb.AddRelativeForce(Vector2.up * intensity);
        }

        private void Fire () {
            _gun.Fire();
        }

        #region saveload

        // TODO fill me in
        public GameData OnSave () {
            //throw new NotImplementedException();
            PlayerGameData playerGameData = new PlayerGameData();
            playerGameData.Pos = _rb.position;
            playerGameData.Rotation = _rb.rotation;
            playerGameData.Velocity = _rb.velocity;
            playerGameData.AngularVelocity = _rb.angularVelocity;
            return playerGameData;
        }

        // TODO fill me in
        public void OnLoad (GameData data) {
            //throw new NotImplementedException();

            _rb.position = ((PlayerGameData) data).Pos;
            _rb.rotation = ((PlayerGameData) data).Rotation;
            _rb.velocity = ((PlayerGameData) data).Velocity;
            _rb.angularVelocity = ((PlayerGameData) data).AngularVelocity * Mathf.Deg2Rad;
        }
        
        #endregion
    }

    public class PlayerGameData : GameData
    {
        public Vector2 Pos;
        public Vector2 Velocity;
        public float Rotation;
        public float AngularVelocity; // reaed as DEGREES but stored as RADIANS; COME ON UNITY
    }
}
