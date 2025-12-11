using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public class MenuScreen : Screen 
{
    private int selectedEntry = 0; 

    protected List<MenuEntry> menuEntries;
    public IList<MenuEntry> MenuEntries => menuEntries;

    public SpriteFont Font { get; set; }

    public MenuScreen(SokobanGame game)
        : this(game, [])
    {
    }

    public MenuScreen(SokobanGame game, IEnumerable<MenuEntry> entries) 
        : base(game)
    {
        menuEntries = new List<MenuEntry>(entries); 
        Font = game.ScreenManager.Font;
    }

    public override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime)
    {
        UpdateEntriesPositions();

        var spriteBatch = ScreenManager.SpriteBatch;

        spriteBatch.Begin();

        for (var i = 0; i < menuEntries.Count; ++i)
        {
            var entry = menuEntries[i];
            entry.Draw(this, i == selectedEntry, gameTime);
        }

        spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        selectedEntry = SetNextEnabledEntry(selectedEntry);
        
        foreach (var entry in menuEntries)
        {
            entry.Update(this, false, gameTime);
        }
    }

    private void UpdateEntriesPositions()
    {
        var position = new Vector2(0, 200);

        foreach (var entry in menuEntries)
        {
            position.X = ScreenManager.ScreenSize.X / 2 - entry.GetWidth(this) / 2;
            entry.Position = position;
            position.Y += entry.GetHeight(this); 
        }
    }

    public override void HandleInput(GameTime gameTime, InputManager inputManager)
    {
        // TODO: add mouse input
        // if (inputManager.IsLeftMouseButtonClicked())
        //     TextSelectedCheck(inputManager.CurrentCursorLocation);
        if (inputManager.IsUp())
        {
            var nextSelectedEntry = selectedEntry;

            if (nextSelectedEntry > 0)
                nextSelectedEntry--;

            while (!menuEntries[nextSelectedEntry].Enabled && nextSelectedEntry > 0)
                nextSelectedEntry--;

            selectedEntry = nextSelectedEntry;
        }

        if (inputManager.IsDown())
        {
            var nextSelectedEntry = selectedEntry;

            if (nextSelectedEntry < menuEntries.Count - 1)
                nextSelectedEntry++;

            selectedEntry = SetNextEnabledEntry(nextSelectedEntry);
        }

        if (inputManager.IsSelected())
            OnSelectEntry(selectedEntry);
        if (inputManager.IsCanceled())
            Exit();
    }

    protected virtual void OnSelectEntry(int selectedEntry)
    {
        menuEntries[selectedEntry].OnSelection();
    }

    private int SetNextEnabledEntry(int nextSelectedEntry)
    {
        while (!menuEntries[nextSelectedEntry].Enabled 
            && nextSelectedEntry < menuEntries.Count - 1)
            nextSelectedEntry++;

        return nextSelectedEntry;
    }
}