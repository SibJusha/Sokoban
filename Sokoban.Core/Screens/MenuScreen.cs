using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public class MenuScreen : Screen 
{
    protected List<MenuEntry> menuEntries;
    public IList<MenuEntry> MenuEntries => menuEntries;
    protected int selectedEntry = 0; 
    private int topVisibleIndex = 0;
    private int maxVisibleEntries;
    private int entryHeight;

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

    public override void Initialize()
    {
        base.Initialize();
        entryHeight = Font.LineSpacing;
        RecalculateVisibleCount();
    }

    public override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime)
    {
        UpdateEntriesPositions();

        int start = topVisibleIndex;
        int end = Math.Min(topVisibleIndex + maxVisibleEntries, menuEntries.Count);

        for (var i = start; i < end; ++i)
        {
            menuEntries[i].Draw(this, i == selectedEntry, gameTime);
        }

        DrawScrollbar();
    }

    public override void Update(GameTime gameTime)
    {
        selectedEntry = SetNextEnabledEntry(selectedEntry);
        
        foreach (var entry in menuEntries)
        {
            entry.Update(this, false, gameTime);
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

            while (nextSelectedEntry > 0 && !menuEntries[nextSelectedEntry].Enabled)
                nextSelectedEntry--;

            selectedEntry = nextSelectedEntry;
            EnsureSelectedIsVisible();
        }

        if (inputManager.IsDown())
        {
            var nextSelectedEntry = selectedEntry;

            if (nextSelectedEntry < menuEntries.Count - 1)
                nextSelectedEntry++;

            selectedEntry = SetNextEnabledEntry(nextSelectedEntry);
            EnsureSelectedIsVisible();
        }

        if (inputManager.IsSelected())
            OnSelectEntry();
        if (inputManager.IsCanceled())
            Exit();
    }

    protected virtual void OnSelectEntry()
    {
        if (selectedEntry >= 0 && selectedEntry < menuEntries.Count)
            menuEntries[selectedEntry].OnSelection();
    }

    private void EnsureSelectedIsVisible()
    {
        if (selectedEntry < topVisibleIndex)
            topVisibleIndex = selectedEntry;
        else if (selectedEntry >= topVisibleIndex + maxVisibleEntries)
            topVisibleIndex = Math.Max(0, selectedEntry - maxVisibleEntries + 1);
    }

    private void RecalculateVisibleCount()
    {
        var availableHeight = ScreenManager.ScreenSize.Y * 0.75f;
        maxVisibleEntries = Math.Max(1, (int)(availableHeight / entryHeight));
    }

    private void UpdateEntriesPositions()
    {
        var visibleCount = Math.Min(maxVisibleEntries, 
            menuEntries.Count - topVisibleIndex);
        var totalHeight = visibleCount * entryHeight;
        var curY = (ScreenManager.ScreenSize.Y - totalHeight) / 2;

        for (var i = 0; i < visibleCount; ++i)
        {
            var globalIdx = topVisibleIndex + i;
            var entry = menuEntries[globalIdx];
            var width = entry.GetWidth(this);
            var x = ScreenManager.ScreenSize.X / 2f - width / 2f;
            entry.Position = new Vector2(x, curY);
            curY += entry.GetHeight(this);
        } 
    }

    private void DrawScrollbar()
    {
        if (menuEntries.Count <= maxVisibleEntries) 
            return;

        var spriteBatch = ScreenManager.SpriteBatch;
        var trackX = (int)ScreenManager.ScreenSize.X - 50;
        var trackTop = ScreenManager.ScreenSize.Y * 0.15f;
        var trackHeight = ScreenManager.ScreenSize.Y * 0.7f;

        spriteBatch.Draw(ScreenManager.WhitePixel,
            new Rectangle(trackX, (int)trackTop, 12, (int)trackHeight),
            Color.Black);

        var ratio = (float)topVisibleIndex / (menuEntries.Count - maxVisibleEntries);
        var thumbHeight = maxVisibleEntries / (float)menuEntries.Count * trackHeight;
        thumbHeight = Math.Max(30, thumbHeight);
        var thumbY = trackTop + ratio * (trackHeight - thumbHeight);

        spriteBatch.Draw(ScreenManager.WhitePixel,
            new Rectangle(trackX + 2, (int)thumbY + 2, 8, (int)thumbHeight),
            Color.White);
    }

    private int SetNextEnabledEntry(int nextSelectedEntry)
    {
        while (!menuEntries[nextSelectedEntry].Enabled 
            && nextSelectedEntry < menuEntries.Count - 1)
            nextSelectedEntry++;

        return nextSelectedEntry;
    }
}