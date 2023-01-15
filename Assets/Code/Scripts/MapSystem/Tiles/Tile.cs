using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.MapSystem
{
    public class Tile : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("Tile Created");
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public virtual bool IsEnterable()
        {
            return false;
        }

        public virtual void OnTileEntered()
        {
            Debug.Log("Character Entered Tile"+gameObject.name);
        }
    }
}