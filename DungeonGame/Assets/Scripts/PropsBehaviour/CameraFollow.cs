using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 3.0f;
        
        [Header("Z-Axis Settings")]
        public float fixedZ = -10.0f;

        private Vector3 offset;
        private Vector3 targetPos;

        private void Start()
        {
            if (target)
            {
                offset = transform.position - target.position;
            }  
        }

        private void LateUpdate()
        {
            if (target)
            {
                targetPos = target.position + offset;
            
                targetPos.z = fixedZ;

                Vector3 nextPos = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
                
                nextPos.z = fixedZ;
                transform.position = nextPos;
            }
        }
    }
}