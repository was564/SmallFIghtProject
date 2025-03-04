﻿using UnityEngine;

namespace Character.CharacterPassiveState
{
    public class StoppingOnGroundState : PassiveStateInterface
    {
        public StoppingOnGroundState(GameObject characterRoot) : base(characterRoot)
        {
            _chatacter = characterRoot.GetComponent<PlayerCharacter>();
            _characterTransform = characterRoot.GetComponent<Rigidbody>();
        }

        private PlayerCharacter _chatacter;
        private Rigidbody _characterTransform;
        
        public override void EnterPassiveState()
        {
            
        }

        public override void UpdatePassiveState()
        {
            if (_chatacter.CurrentCharacterPositionState == PassiveStateEnumSet.CharacterPositionState.InAir) return;
            
            _characterTransform.velocity *= 0.8f;
            if (_characterTransform.velocity.sqrMagnitude <= 0.1f)
                _characterTransform.velocity = Vector3.zero;
        }

        public override void QuitPassiveState()
        {
            
        }
    }
}