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
        public float fixedZ = -10.0f; // This will force the camera back

        private Vector3 offset;
        private Vector3 targetPos;

        private void Start()
        {
            if (target == null) return;

            // Calculate initial offset
            offset = transform.position - target.position;
        }

        private void LateUpdate() // Changed to LateUpdate for smoother tracking
        {
            if (target == null) return;

            targetPos = target.position + offset;
            
            // Force the targetPos Z to be our fixedZ before we move
            targetPos.z = fixedZ;

            // Interpolate position
            Vector3 nextPos = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
            
            // Apply position, again ensuring Z is locked
            nextPos.z = fixedZ;
            transform.position = nextPos;
        }
    }
}