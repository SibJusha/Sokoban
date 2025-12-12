using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public class MainMenuScreen : MenuScreen
{
    public MainMenuScreen(SokobanGame game) 
        : base(game)
    {
        MenuEntries.Add(new MenuEntry("Play", PlayMenuEntrySelected));
        MenuEntries.Add(new MenuEntry("Editor", EditorEntrySelected));
        MenuEntries.Add(new MenuEntry("Settings", false));
        MenuEntries.Add(new MenuEntry("Exit", ExitOptionSelected));
    }

    private void PlayMenuEntrySelected(object sender, EventArgs e)
    {
        // TODO: show levels screen
        // ScreenManager.ShowScreen(this);
        // ScreenManager.ShowScreen(new LevelsMenuScreen(Game));
        ScreenManager.ShowScreen(new LevelScreen(Game, Game.LevelManager.LevelsMap["lvl0"]));
    }

    private void EditorEntrySelected(object sender, EventArgs e)
    {
        //TODO: open editor
    }

    private void ExitOptionSelected(object sender, EventArgs e)
    {
        Game.Exit();
    }
}