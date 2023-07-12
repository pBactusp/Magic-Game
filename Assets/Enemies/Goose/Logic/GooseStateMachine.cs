using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies.Goose
{
    [RequireComponent(typeof(SoundManager))]
    [RequireComponent(typeof(RagdollManager))]
    public class GooseStateMachine : EnemyStateMachine
    {
        [Header("Honk")]
        public float HonkCooldown;
        public float HonkRandomnessRange;

        [field: SerializeField] public MeleeWeapon Beak { get; private set; }
        public RagdollManager RagdollManager { get; private set; }
        public SoundManager SoundManager { get; private set; }
        [HideInInspector] public bool IsHonking;

        private float elapsedHonk;
        private float currentHonkCooldown;



        new private void Awake()
        {
            Beak = GetComponentInChildren<MeleeWeapon>();
            RagdollManager = GetComponent<RagdollManager>();
            SoundManager = GetComponent<SoundManager>();
            IsHonking = false;
            base.Awake();
        }

        new void Start()
        {
            base.Start();

            elapsedHonk = 0;
            currentHonkCooldown = HonkCooldown + Random.Range(0, HonkRandomnessRange);

            StartCoroutine(HonkLoop());

            SwitchState(new GooseIdleState(this));
        }


        public void FinishedAttacking()
        {
            OnFinishedAttacking.Invoke();
        }


        private IEnumerator HonkLoop()
        {
            while (true)
            {
                if (IsHonking)
                {
                    elapsedHonk += Time.deltaTime;

                    if (elapsedHonk >= currentHonkCooldown)
                    {
                        currentHonkCooldown = HonkCooldown + Random.Range(0, HonkRandomnessRange);
                        elapsedHonk = 0;

                        Honk();

                        yield return null;
                    }

                }

                yield return null;
            }
        }

        private void Honk()
        {
            SoundManager.PlayRandomSound();
            AnimationManager.Animator.SetTrigger("Honk");
        }

        protected override void Die()
        {
            Debug.Log(gameObject.name + " died");
            IsAlive = false;
            SwitchState(new GooseRagdollState(this, Vector3.zero));
        }
    }
}