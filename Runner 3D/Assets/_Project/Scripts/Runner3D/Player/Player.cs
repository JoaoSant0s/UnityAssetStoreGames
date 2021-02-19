using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoaoSantos.Runner3D.WorldElement
{
    [Serializable]
    public struct PlayerMovementSettings
    {
        public float forwardForce;
        public float limitSpeed;
        public float jumpForce;

        public float oppositeForceProportion;
        public TransversalMovement transversalMovement;
    }

    [Serializable]
    public struct TransversalMovement
    {
        public float movementDistance;
        public Vector2 movementLimits;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private LayerMask platformLayerMask;

        [SerializeField]
        private PlayerMovementSettings movementSettings;

        #region Local Reference
        private Rigidbody rb;
        private float movementIndex;

        private TransversalMovement TransversalMovement
        {
            get { return this.movementSettings.transversalMovement; }
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            this.rb = GetComponent<Rigidbody>();
            this.movementIndex = 0;
        }

        private void FixedUpdate()
        {
            ApplyForwardMovement();
        }

        private void Update()
        {
            PlayerMovementTrigger();
        }

        #endregion

        #region  Private Methods

        private void ApplyForwardMovement()
        {
            if (this.rb.velocity.magnitude >= this.movementSettings.limitSpeed) return;

            this.rb.AddForce(0, 0, this.movementSettings.forwardForce * Time.deltaTime);
        }

        private void PlayerMovementTrigger()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                MoveRight();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Jump();
            }
        }

        private void MoveLeft()
        {
            if (this.movementIndex <= TransversalMovement.movementLimits.x) return;

            this.movementIndex--;
            transform.Translate(-TransversalMovement.movementDistance, 0, 0);
        }

        private void MoveRight()
        {
            if (this.movementIndex >= TransversalMovement.movementLimits.y) return;

            this.movementIndex++;
            transform.Translate(TransversalMovement.movementDistance, 0, 0);
        }

        private void Jump()
        {
            RaycastHit hit;

            if (!Physics.Raycast(transform.position, Vector3.down, out hit, 0.55f, this.platformLayerMask)) return;

            if (hit.collider == null) return;

            this.rb.AddForce(0, this.movementSettings.jumpForce, 0, ForceMode.Impulse);
            this.rb.AddForce(0, -this.movementSettings.jumpForce / this.movementSettings.oppositeForceProportion, 0);
        }
        #endregion
    }
}
