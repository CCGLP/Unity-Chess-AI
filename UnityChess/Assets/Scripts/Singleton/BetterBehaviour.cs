using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class BetterBehaviour : MonoBehaviour {

        Transform _transform = null;
    
        public Transform Transform {
            get {
                if (_transform == null) {
                    _transform = transform;
                }
                return _transform;
            }
        }
    }

