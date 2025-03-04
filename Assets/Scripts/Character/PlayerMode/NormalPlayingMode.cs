﻿namespace Character.PlayerMode
{
    public class NormalPlayingMode : PlayerModeInterface
    {
        public NormalPlayingMode(PlayerCharacter character) 
            : base(PlayerModeManager.PlayerMode.NormalPlaying, character) { }
        
        public override void Update()
        {
            Character.DecideBehaviorByInput();
            Character.UpdatePassiveState();
            Character.StateManager.UpdateState();
            Character.ComboManagerInstance.UpdateComboManager();
        }
    }
}