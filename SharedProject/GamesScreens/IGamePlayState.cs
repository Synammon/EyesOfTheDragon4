using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.GamesScreens
{
    public interface IGamePlayState
    {
        GameState Tag { get; }
    }
}
