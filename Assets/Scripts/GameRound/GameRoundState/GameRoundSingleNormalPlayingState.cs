﻿using UnityEngine;

namespace GameRound
{
    public class GameRoundSingleNormalPlayingState : GameRoundStateInterface
    {
        public GameRoundSingleNormalPlayingState(GameRoundStateManager manager)
            : base(manager, GameRoundManager.GameState.SingleNormalPlay) { }

        public override void Enter()
        {
            //Debug.Log(FrameManager.CurrentFrame);
            FrameManager.IsFramePaused = false;
            RoundManager.ApplySettingInStateByPausing(false);
        }
        
        public override void Update()
        {
            //Debug.Log(PlayersControlManager.GetDistanceBetweenPlayers());
            
            if (RoundManager.IsGameEnded)
                RoundStateManager.ChangeState(GameRoundManager.GameState.Wait);
            
            if(InputManager.IsPressedMenuKey())
                RoundStateManager.ChangeState(GameRoundManager.GameState.Pause);
            
            RoundManager.DecreaseRemainTimePerFrame(1);
        }
        
        public override void Quit()
        {
            FrameManager.IsFramePaused = true;
        }
    }
}