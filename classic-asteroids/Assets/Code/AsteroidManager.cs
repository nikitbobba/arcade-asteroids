using System;
using System.Collections.Generic;
using Assets.Code.Structure;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Code
{
    /// <inheritdoc><cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Manager class for spawning and tracking all of the game's asteroids
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AsteroidManager : MonoBehaviour, ISaveLoad
    {
        private const float SpawnTime = 3f;
        private const int MaxAsteroidCount = 8;
        private static Object _asteroidPrefab;
        private float _lastspawn;
        private Transform _holder;

        // ReSharper disable once UnusedMember.Global
        internal void Start () {
            _asteroidPrefab = Resources.Load("Asteroid");
            _holder = transform;
            Asteroid.Manager = this;
        }

        // ReSharper disable once UnusedMember.Global
        internal void Update () {
            if ((Time.time - _lastspawn) < SpawnTime) return;
            _lastspawn = Time.time;
            Spawn();
        }

        private void Spawn () {
            if (_holder.childCount >= MaxAsteroidCount) { return; }

            var pos = BoundsChecker.GetRandomPos();
            var vel = BoundsChecker.GetRandomVelocity();
            int size = Random.Range(2, Asteroid.AsteroidTypes); // don't spawn tinies

            ForceSpawn(pos, vel, size);
        }

        // TODO fill me in
        public void ForceSpawn (Vector2 pos, Vector2 velocity, int size, Quaternion rotation = new Quaternion())
        {
            var asteroids1 = (GameObject) Object.Instantiate(_asteroidPrefab, pos, rotation, _holder);
            var init1 = asteroids1.GetComponent<Asteroid>();
            init1.Initialize(velocity, size);
        }

        #region saveload

        // TODO fill me in
        public GameData OnSave () {
            //throw new NotImplementedException();

            Asteroid[] asteroid_array = GameObject.FindObjectsOfType<Asteroid>();
            AsteroidsData new_list_asteroid = new AsteroidsData();
            
            foreach (Asteroid i in asteroid_array)
            {
                AsteroidData x = new AsteroidData();

                x.Size = i.Size;

                x.Pos = i.GetComponent<Rigidbody2D>().position;

                x.Velocity = i.GetComponent<Rigidbody2D>().velocity;
                
                new_list_asteroid.Asteroids.Add(x);

            }

            return new_list_asteroid;


        }

        // TODO fill me in
        public void OnLoad (GameData data) {
            //throw new NotImplementedException();

            for (int j = 0; j < _holder.childCount; j++)
            {
                GameObject.Destroy(_holder.GetChild(j).gameObject);
            }

            foreach (AsteroidData x in ((AsteroidsData) data).Asteroids)
            {
                ForceSpawn(x.Pos, x.Velocity, x.Size);
            }
            
            
        }

        #endregion
    }

    /// <summary>
    /// The save data for all the asteroids
    /// </summary>
    public class AsteroidsData : GameData
    {
        public List<AsteroidData> Asteroids = new List<AsteroidData>();
    }

    /// <summary>
    /// The save data for one asteroid
    /// </summary>
    public class AsteroidData
    {
        public int Size;
        public Vector2 Pos;
        public Vector2 Velocity;
    }
}
