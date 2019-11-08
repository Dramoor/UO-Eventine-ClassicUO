﻿#region license

//  Copyright (C) 2019 ClassicUO Development Community on Github
//
//	This project is an alternative client for the game Ultima Online.
//	The goal of this is to develop a lightweight client considering 
//	new technologies.  
//      
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using SDL2;
using System.Runtime.CompilerServices;

using ClassicUO.Configuration;

namespace ClassicUO.Game.Scenes
{
    public enum ScenesType
    {
        Login,
        Game
    }

    internal sealed class SceneManager
    {
        public Scene CurrentScene { get; private set; }

        public void ChangeScene(ScenesType type)
        {
            CurrentScene?.Destroy();
            CurrentScene = null;

            switch (type)
            {
                case ScenesType.Login:
                    Engine.IsMaximized = false;
                    Engine.WindowWidth = 640;
                    Engine.WindowHeight = 480;
                    Engine.AllowWindowResizing = false;
                    CurrentScene = new LoginScene();

                    break;

                case ScenesType.Game:

                    Engine.AllowWindowResizing = true;

                    if (!Bootstrap.StartInLittleWindow)
                    {
                        if (ProfileManager.Current != null)
                        {
                            Engine.SetPreferredBackBufferSize(ProfileManager.Current.WindowClientBounds.X, ProfileManager.Current.WindowClientBounds.Y);

                            if (!ProfileManager.Current.RestoreLastGameSize)
                            {
                                Engine.IsMaximized = true;
                            }
                            else
                            {
                                Engine.WindowWidth = ProfileManager.Current.WindowClientBounds.X;
                                Engine.WindowHeight = ProfileManager.Current.WindowClientBounds.Y;
                                if (ProfileManager.Current.WindowClientPosition.HasValue)
                                {
                                    Engine.WindowPosition = ProfileManager.Current.WindowClientPosition.Value;
                                }
                            }
                        }
                    }

                    CurrentScene = new GameScene();

                    break;
            }

            CurrentScene?.Load();
        }

        [MethodImpl(256)]
        public T GetScene<T>() where T : Scene
        {
            if (CurrentScene is T s)
                return s;

            return null;
        }
    }
}