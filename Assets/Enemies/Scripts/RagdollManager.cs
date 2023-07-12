using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(StateMachine))]
    public class RagdollManager : MonoBehaviour
    {
        public Rigidbody Root;
        public Collider[] CollidersNotForRagdoll;
        public Rigidbody[] RigidbodiesNotForRagdoll;

        private Animator animator;
        new private Rigidbody rigidbody;
        private StateMachine stateMachine;

        private List<Rigidbody> childrenRigidbodies;
        private List<Collider> childrenColliders;

        private bool _isActive;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            stateMachine = GetComponent<StateMachine>();

            childrenRigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
            childrenColliders = new List<Collider>(GetComponentsInChildren<Collider>());

            for (int i = 0; i < CollidersNotForRagdoll.Length; i++)
                childrenColliders.Remove(CollidersNotForRagdoll[i]);

            for (int i = 0; i < RigidbodiesNotForRagdoll.Length; i++)
                childrenRigidbodies.Remove(RigidbodiesNotForRagdoll[i]);


            _isActive = true; // Important for disabling collisions on children
            SetRagdoll(false);
        }


        public void SetRagdoll(bool active)
        {
            if (active == _isActive)
                return;

            _isActive = active;

            animator.enabled = !active;
            stateMachine.enabled = !active;

            for (int i = 0; i < childrenRigidbodies.Count; i++)
            {
                childrenRigidbodies[i].isKinematic = !active;
                childrenRigidbodies[i].useGravity = active;
                childrenRigidbodies[i].detectCollisions = active;
            }

            for (int i = 0; i < childrenColliders.Count; i++)
            {
                childrenColliders[i].enabled = active;
            }


            rigidbody.detectCollisions = !active;
            rigidbody.isKinematic = true; ;

            //for (int i = 0; i < CollidersNotForRagdoll.Length; i++)
            //{
            //    CollidersNotForRagdoll[i].enabled = !active;
            //}

            //for (int i = 0; i < RigidbodiesNotForRagdoll.Length; i++)
            //{
            //    RigidbodiesNotForRagdoll[i].isKinematic = true;
            //    RigidbodiesNotForRagdoll[i].useGravity = false;
            //    RigidbodiesNotForRagdoll[i].detectCollisions = !active;
            //}
            
        }

        public void Addforce(Vector3 force, ForceMode mode)
        {
            Root.AddForce(force, mode);
            Debug.DrawRay(Root.position, force, Color.red, 5f);
        }
    }


}