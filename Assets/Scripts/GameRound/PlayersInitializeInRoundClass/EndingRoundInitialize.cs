﻿using System.Collections.Generic;
using Character.PlayerMode;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class EndingRoundInitialize : PlayersInitializeInRoundFactory
    {
        public EndingRoundInitialize(Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> players)
            : base(players) {}
        
        protected override void InitializePlayersPosition()
        {
            
        }

        protected override void InitializePlayersInitState()
        {
            PlayerCharacter losePlayer = null, winPlayer = null;

            // reference : https://stackoverflow.com/questions/804706/swap-two-variables-without-using-a-temporary-variable
            (losePlayer, winPlayer) = (Players[PlayerCharacter.CharacterIndex.Player], Players[PlayerCharacter.CharacterIndex.Enemy]);

            if (losePlayer.Hp > winPlayer.Hp)
                (losePlayer, winPlayer) = (winPlayer, losePlayer);

            if (winPlayer.Hp <= 0) {}
            else if (losePlayer.Hp > 0)
            {
                winPlayer.StateManager.ChangeState(BehaviorEnumSet.State.OutroPose);
                losePlayer.StateManager.ChangeState(BehaviorEnumSet.State.OutroPose);
            }
            else
            {
                winPlayer.StateManager.ChangeState(BehaviorEnumSet.State.OutroPose);
            }
        }

        protected override void InitializePlayersMode()
        {
            foreach (var player in Players)
            {
                player.Value.SetPlayerMode(PlayerModeManager.PlayerMode.NormalPlaying);
                player.Value.IsAcceptArtificialInput = false;
            }
        }
    }
}