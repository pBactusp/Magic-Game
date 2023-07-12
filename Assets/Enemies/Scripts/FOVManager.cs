using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class FOVManager : MonoBehaviour
    {
        public float ViewDistance;
        [Range(0, 360)]
        public float ViewAngle;
        public float AwarenessRadius;

        [Header("Performance")]
        public float Delay;


        [HideInInspector] public Transform EyesPosition;
        public bool ShowGui { get { return EyesPosition != null; } }

        private bool _canSeePlayer;
        public bool CanSeePlayer
        {
            get { return _canSeePlayer; }
            set
            {
                if (CanSeePlayer && !value)
                {
                    _canSeePlayer = value;
                    PlayerLeftFOV();
                }
                else if (!CanSeePlayer && value)
                {
                    _canSeePlayer = value;
                    PlayerEnteredFOV();
                }
            }
        }

        public bool IsAwareOfPlayer { get; private set; }

        public Action OnPlayerEnteredFOV;
        public Action OnPlayerLeftFOV;




        void Start()
        {
            _canSeePlayer = false;
            IsAwareOfPlayer = false;
            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(Delay);

            while (true)
            {
                FieldOfViewCheck();
                yield return wait;

            }
        }

        private void FieldOfViewCheck()
        {
            if (IsAwareOfPlayer)
            {
                float sqrDistance = (PlayerData.TargetForEnemies.position - EyesPosition.position).sqrMagnitude;

                if (sqrDistance <= AwarenessRadius * AwarenessRadius)
                {
                    CanSeePlayer = true;
                    return;
                }
            }

            Collider[] rangeCheck = Physics.OverlapSphere(EyesPosition.position, ViewDistance, LayerMasks.Player);

            if (rangeCheck.Length > 0)
            {
                Transform player = rangeCheck[0].transform;
                Vector3 directionToPlayer = (player.position - EyesPosition.position).normalized;

                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                if (angle < ViewAngle / 2f)
                {
                    float distanceToPlayer = Vector3.Distance(EyesPosition.position, PlayerData.TargetForEnemies.position);

                    if (!VisionIsObstructed(directionToPlayer, distanceToPlayer))
                    {
                        CanSeePlayer = true;
                    }
                    else
                        CanSeePlayer = false;
                }
                else
                    CanSeePlayer = false;


            }
            else if (CanSeePlayer)
                CanSeePlayer = false;
        }

        public bool VisionIsObstructed(Vector3 directionToPlayer, float distanceToPlayer)
        {
            Ray ray = new Ray(EyesPosition.position, directionToPlayer);

            if (Physics.Raycast(ray, distanceToPlayer, LayerMasks.Obstruction))
                return true;

            return false;
        }


        private void PlayerEnteredFOV()
        {
            IsAwareOfPlayer = true;
            OnPlayerEnteredFOV?.Invoke();
        }

        private void PlayerLeftFOV()
        {
            IsAwareOfPlayer = false;
            OnPlayerLeftFOV?.Invoke();
        }


    }


}