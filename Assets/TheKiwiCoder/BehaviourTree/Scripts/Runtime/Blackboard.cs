using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TheKiwiCoder
{

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard
    {
        public Vector3 moveToPosition;
        public Vector3 CharacterPosition = Vector3.zero;
        public Vector3 LastPosition = Vector3.zero;
        public bool canSeeTarget = false;

        public Blackboard()
        {
            CharacterPosition = Vector3.zero;
            LastPosition = Vector3.zero;
        }

        public bool SetLastPosition()
        {
            if (CharacterPosition != Vector3.positiveInfinity)
            {
                LastPosition = CharacterPosition;
                return true;
            }

            return false;
        }

        public bool ChaseLastPosition()
        {
            if (LastPosition != Vector3.zero)
            {
                moveToPosition = LastPosition;
                return true;
            }
            
            return false;
        }

        public bool ChaseCurrentPosition()
        {
            if(CharacterPosition != Vector3.zero)
            {
                Debug.Log("Proshel current");
                moveToPosition = CharacterPosition;
                return true;
            }

            Debug.Log(CharacterPosition);
            Debug.Log("Ne proshel current");
            return false;
        }
    }
}