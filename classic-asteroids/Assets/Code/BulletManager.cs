using System;
using System.Collections.Generic;
using Assets.Code.Structure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Code
{
    /// <summary>
    /// Bullet manager for spawning and tracking all of the game's bullets
    /// </summary>
    public class BulletManager : ISaveLoad
    {
        private readonly Transform _holder;

        /// <summary>
        /// Bullet prefab. Use GameObject.Instantiate with this to make a new bullet.
        /// </summary>
        private readonly Object _bullet;

        public BulletManager (Transform holder) {
            _holder = holder;
            _bullet = Resources.Load("Bullet");
        }

        // TODO fill me in
        public void ForceSpawn (Vector2 pos, Quaternion rotation, Vector2 velocity, float deathtime)
        {
            var bullet1 = (GameObject)Object.Instantiate(_bullet, pos, rotation, _holder);
            var init = bullet1.GetComponent<Bullet>();
            init.Initialize(velocity, deathtime);
        }

        #region saveload

        // TODO fill me in
        public GameData OnSave () {
            //throw new NotImplementedException();

            Bullet[] bullet_array = GameObject.FindObjectsOfType<Bullet>();
            
            //creating new instance
            BulletsData new_list_bullet = new BulletsData();
            
            new_list_bullet.Bullets = new List<BulletData>();

            foreach (Bullet i in bullet_array)
            {
                BulletData x = new BulletData();
                
                var y = i.GetComponent<Rigidbody2D>().position;
                x.Pos = y;

                x.Velocity = i.GetComponent<Rigidbody2D>().velocity;

                x.Rotation = i.GetComponent<Rigidbody2D>().rotation;
                
                new_list_bullet.Bullets.Add(x);
            }



            return new_list_bullet;

        }

        // TODO fill me in
        public void OnLoad (GameData data) {
            //throw new NotImplementedException();

            for (int i = 0; i < _holder.childCount; i++)
            {
                GameObject.Destroy(_holder.GetChild(i).gameObject);
            }
            
            
            
            foreach (BulletData x in ((BulletsData) data).Bullets)
            {
                ForceSpawn(x.Pos, Quaternion.Euler(0,0,x.Rotation), x.Velocity, Time.time + Bullet.Lifetime);
            }
            
            
        }

        #endregion

    }

    /// <summary>
    /// Save data for all bullets in game
    /// </summary>
    public class BulletsData : GameData
    {
        public List<BulletData> Bullets;
    }

    /// <summary>
    /// Save data for a single bullet
    /// </summary>
    public class BulletData
    {
        public Vector2 Pos;
        public Vector2 Velocity;
        public float Rotation;
    }
}