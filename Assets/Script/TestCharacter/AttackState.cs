using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    private new PlayerMovement player;

    private List<float> comboAttackTime = new List<float>();
    private List<float> comboInTime = new List<float>();

    private string[] comboAnimation = { "openWeapon", "attack_1", "attack_2" };
    private KeyCode[] comboKey = { KeyCode.J, KeyCode.J, KeyCode.J };

    private int currentKeyIndex = 0;

    private float attackTime = 1f;
    private float comboConstrainTime = 0f;

    private bool readyToCombo1 = false;
    private bool readyToCombo2 = false;

    public AttackState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        for (int i = 0; i < comboAnimation.Length; i++)
        {
            comboAttackTime.Add(attackTime);
            comboInTime.Add(comboConstrainTime);
        }
    }

    public override void ExitState()
    {
        ResetCombo();
    }

    public override void FrameUpdate()
    {
        ComboFunction();
    }

    public override void PhysicsUpdate()
    {
        if (comboAttackTime[currentKeyIndex] <= 0f)
        {
            player.stateMachine.ChangeState(player.movementState);
        }
    }

    private void ComboFunction()
    {
        if (comboAttackTime[currentKeyIndex] > 0f)
        {
            comboAttackTime[currentKeyIndex] -= Time.deltaTime;
            comboInTime[currentKeyIndex] += Time.deltaTime;
            player.animator.Play(comboAnimation[currentKeyIndex]);
        }

        DetectKeyInput(comboAttackTime[currentKeyIndex]);
    }

    private void DetectKeyInput(float timer)
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(comboKey[currentKeyIndex]) && comboInTime[currentKeyIndex] <= 0.4f)
            {
                if (timer > 0f)
                {
                    timer = 0.4f - comboInTime[currentKeyIndex];
                }

                if (currentKeyIndex == 0)
                {
                    readyToCombo1 = true;
                }

                if (currentKeyIndex == 1)
                {
                    readyToCombo2 = true;
                }
            }
        }

        if (comboInTime[currentKeyIndex] >= 0.4f && currentKeyIndex < 2)
        {
            if (readyToCombo1 == true) currentKeyIndex = 1;
            if (readyToCombo2 == true) currentKeyIndex = 2;
        }
    }

    private void ResetCombo()
    {
        comboAttackTime.Clear();
        comboInTime.Clear();

        currentKeyIndex = 0;
        readyToCombo1 = false;
        readyToCombo2 = false;
    }
}
