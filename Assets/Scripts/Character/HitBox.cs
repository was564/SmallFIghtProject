using System;
using System.Collections;
using System.Collections.Generic;
using Character.CharacterFSM;
using UnityEngine;
using Character.CharacterFSM.KohakuState;

public class HitBox : MonoBehaviour
{
    // FSM 매니저로 가게끔 만들기
    //private CharacterAnimator _animator;
    private BehaviorStateManager _stateManager;
    private PlayerCharacter _playerCharacter;
    private PlayerCharacter _enemyCharacter;
    
    private BoxCollider _hitBox;

    private FrameManager _gameManager;

    private Rigidbody _rigidbody;
    
    private float _backMoveSpeedByAttack = 2.0f;
    private int _blockingFrameForGuard = 12;
    
    public delegate void HitBoxEvent(PlayerCharacter.CharacterIndex characterIndex);
    public event HitBoxEvent OnHitBoxTriggered = delegate {  };
    
    // Start is called before the first frame update
    void Start()
    {
        //_animator = this.GetComponentInParent<CharacterAnimator>();
        _playerCharacter = this.transform.root.GetComponent<PlayerCharacter>();
        _enemyCharacter = _playerCharacter.EnemyObject.GetComponent<PlayerCharacter>();
        _hitBox = this.GetComponent<BoxCollider>();
        _gameManager = GameObject.FindObjectOfType<FrameManager>();
        _rigidbody = this.transform.root.GetComponent<Rigidbody>();
        
        _stateManager = _playerCharacter.StateManager;
    }

    public void RegisterNotifyObserver(HitBoxEvent notifyObserver)
    {
        OnHitBoxTriggered += notifyObserver;
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals(this.tag))
        {
            AttackBox attackInfo = col.GetComponent<AttackBox>();
            _playerCharacter.IsHitContinuous = true;
            _enemyCharacter.IncreaseSkillGauge(4);
            attackInfo.DisableAttackBox();
            
            _rigidbody.velocity = Vector3.left * (this.transform.forward.x < 0.0f ? -1.0f : 1.0f) * _backMoveSpeedByAttack;
            
            if (CheckGuardSuccess(attackInfo))
                ReactGuardAction(attackInfo);
            else attackInfo.ReactWhenHitByAttack(_playerCharacter, _stateManager, _rigidbody);
            
            _gameManager.PauseAllCharactersInFrame(FrameManager.PauseFrameWhenHit);
            
            OnHitBoxTriggered(_playerCharacter.PlayerUniqueIndex);
        }
    }

    public void EnableHitBox()
    {
        _hitBox.enabled = true;
    }
    
    public void DisableHitBox()
    {
        _hitBox.enabled = false;
    }

    private bool CheckGuardSuccess(AttackBox attackInfo)
    {
        if (!_playerCharacter.IsGuarded) return false;
        
        BehaviorEnumSet.State currentState = _stateManager.CurrentState.StateName;
        bool isSucceededGuard = false;
            
        
        if (currentState == BehaviorEnumSet.State.StandingGuard)
        {
            switch (attackInfo.AttackPosition)
            {
                case BehaviorEnumSet.AttackPosition.Air:
                    isSucceededGuard = true;
                    break;
                case BehaviorEnumSet.AttackPosition.Crouch:
                    isSucceededGuard = false;
                    break;
                case BehaviorEnumSet.AttackPosition.Stand:
                    isSucceededGuard = true;
                    break;
            }
        }
        else if (currentState == BehaviorEnumSet.State.CrouchGuard)
        {
            switch (attackInfo.AttackPosition)
            {
                case BehaviorEnumSet.AttackPosition.Air:
                    isSucceededGuard = false;
                    break;
                case BehaviorEnumSet.AttackPosition.Crouch:
                    isSucceededGuard = true;
                    break;
                case BehaviorEnumSet.AttackPosition.Stand:
                    isSucceededGuard = true;
                    break;
            }
        }
        else if(_playerCharacter.IsGuarded) isSucceededGuard = true; // CurrentState = GuardPoint

        return isSucceededGuard;
    }
    
    private void ReactGuardAction(AttackBox attackInfo)
    {
        BehaviorEnumSet.State currentState = _stateManager.CurrentState.StateName;
        if (currentState == BehaviorEnumSet.State.CrouchGuard || currentState == BehaviorEnumSet.State.StandingGuard)
        {
            GuardState currentGuardState = _stateManager.CurrentState as GuardState;
            currentGuardState.ContinuousFrameByBlockAttack = _blockingFrameForGuard;
            _playerCharacter.IsGuarded = true;
            attackInfo.PlayGuardEffect();
        }
        else
        {
            attackInfo.PlayGuardEffect();
        }
    }
}
