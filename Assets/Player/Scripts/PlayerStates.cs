using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    #region Movement
    public class PlayerMoveState : PlayerBaseState
    {
        float currentSpeed;

        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            player.Velocity.y = Physics.gravity.y;

            if (player.Input.IsSprinting)
                StartedSprinting();
            else
                StoppedSprinting();

            player.Input.OnJumpPerformed += SwitchToJumpState;
            player.Input.OnStartedSprinting += StartedSprinting;
            player.Input.OnStoppedSprinting += StoppedSprinting;
            player.Input.OnCastingSpell += CastingSpell;
        }

        public override void Tick()
        {
            if (!player.Controller.isGrounded)
            {
                player.SwitchState(new PlayerFallState(player));
            }

            HandleMovement(currentSpeed);
        }

        public override void Exit()
        {
            StoppedSprinting();

            player.Input.OnJumpPerformed -= SwitchToJumpState;
            player.Input.OnStartedSprinting -= StartedSprinting;
            player.Input.OnStoppedSprinting -= StoppedSprinting;
            player.Input.OnCastingSpell -= CastingSpell;
        }


        private void SwitchToJumpState()
        {
            player.SwitchState(new PlayerJumpState(player));
        }

        private void StartedSprinting()
        {
            currentSpeed = player.SprintSpeed;
            player.Animator.SetSprinting(true);
        }
        private void StoppedSprinting()
        {
            currentSpeed = player.WalkSpeed;
            player.Animator.SetSprinting(false);
        }

        private void CastingSpell(int index)
        {
            player.SwitchState(new PlayerCastSpellState(player, player.Spells[index]));
        }
    }

    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter() { }
        public override void Tick() { }
        public override void Exit() { }
    }


    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            player.Velocity = new Vector3(player.Velocity.x, player.JumpForce, player.Velocity.z);

            player.Animator.Jump();
        }

        public override void Tick()
        {
            ApplyGravity();

            if (player.Velocity.y <= 0f)
            {
                player.SwitchState(new PlayerFallState(player));
            }

            Move();
        }

        public override void Exit() { }
    }

    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            player.Velocity.y = 0f;

            player.Animator.Fall(true);
        }

        public override void Tick()
        {
            ApplyGravity();
            Move();

            if (player.Controller.isGrounded)
            {
                player.SwitchState(new PlayerMoveState(player));
            }
        }

        public override void Exit()
        {
            player.Animator.Fall(false);
        }
    }
    #endregion

    public class PlayerCastSpellState : PlayerBaseState
    {
        private SpellCastingData spellData;
        private Transform spellOrigin;
        private Coroutine casting;
        private Spell spell;
        float currentMovementSpeed;


        public PlayerCastSpellState(PlayerStateMachine stateMachine, SpellCastingData spellData) : base(stateMachine)
        {
            this.spellData = spellData;
            currentMovementSpeed = player.WalkSpeed;
        }


        public override void Enter()
        {
            player.Animator.SetOverride(spellData.Animator);
            player.Animator.CastSpell(spellData.CastTime);

            casting = player.StartCoroutine(CastSpell());

            switch (spellData.Origin)
            {
                case SpellOrigin.RightHand: spellOrigin = player.RightHand; break;
                case SpellOrigin.LeftHand: spellOrigin = player.LeftHand; break;
                case SpellOrigin.Chest: spellOrigin = player.Chest; break;
                default: break;
            }

            player.Animator.OnSpawnSpell += SpawnSpell;
            player.Animator.OnLaunchSpell += LaunchSpell;
        }

        public override void Tick()
        {

        }

        public override void Exit()
        {
            player.Animator.SetMovementSpeed(1);

            player.Animator.OnSpawnSpell -= SpawnSpell;
            player.Animator.OnLaunchSpell -= LaunchSpell;
        }

        private void Canceled()
        {
            player.StopCoroutine(casting);
            player.SwitchState(new PlayerMoveState(player));
        }

        private void SpawnSpell()
        {
            var spellObject = GameObject.Instantiate(spellData.Spell);
            spell = spellObject.GetComponent<Spell>();            

            spell.Spawn(spellOrigin);
        }

        private void LaunchSpell()
        {
            if (spell == null)
                return;

            var args = new SpellInitializationArguments()
            {
                Origin = spellOrigin,
                Direction = GameManager.Instance.MainCamera.transform.forward,
                Target = null
            };

            spell.Launch(args);
        }

        private IEnumerator CastSpell()
        {
            float time = 0;
            bool reachedTargetSpeed = false;

            while (time < spellData.CastTime)
            {
                // Handle slowing down
                if (time < spellData.PlayerSlowdownTime)
                {
                    currentMovementSpeed = Mathf.Lerp(player.WalkSpeed, spellData.MovementSpeedWhileCasting, time / spellData.PlayerSlowdownTime);
                    player.Animator.SetMovementSpeed(currentMovementSpeed / player.WalkSpeed);
                }
                else if (!reachedTargetSpeed)
                {
                    reachedTargetSpeed = true;
                    currentMovementSpeed = spellData.MovementSpeedWhileCasting;
                    player.Animator.SetMovementSpeed(currentMovementSpeed / player.WalkSpeed);
                }

                HandleMovement(currentMovementSpeed, false);
                RotateTowardsCameraView();

                time += Time.deltaTime;
                yield return null;
            }

            player.SwitchState(new PlayerMoveState(player));
        }


    }



    public class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            player.Animator.Die();
        }

        public override void Tick()
        {

        }

        public override void Exit()
        {

        }
    }


}