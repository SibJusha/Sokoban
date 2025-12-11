using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Runtime.Serialization;
using Sokoban.Core.Localization;
using Sokoban.Core.Managers;
using Sokoban.Core.Screens;

namespace Sokoban.Core;

public class SokobanGame : Game
{
    private readonly GraphicsDeviceManager graphicsDeviceManager;
    public ScreenManager ScreenManager { get; private init; }
    public LevelManager LevelManager { get; private init; }

    // public SpriteBatch SpriteBatch { get; private init; }
    
    public SokobanGame()
    {
        Content.RootDirectory = "Content";

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Services.AddService(graphicsDeviceManager);
        graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

        // SpriteBatch = new SpriteBatch(graphicsDeviceManager.GraphicsDevice);

        ScreenManager = new ScreenManager(this);
        Components.Add(ScreenManager); 

        LevelManager = new LevelManager(this);
        Services.AddService(LevelManager); 
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Load supported languages and set the default language.
        List<CultureInfo> cultures = LocalizationManager.GetSupportedCultures();
        var languages = new List<CultureInfo>();
        for (int i = 0; i < cultures.Count; i++)
        {
            languages.Add(cultures[i]);
        }

        // TODO You should load this from a settings file or similar,
        // based on what the user or operating system selected.
        var selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
        LocalizationManager.SetCulture(selectedLanguage);

        ScreenManager.ShowScreen(new MainMenuScreen(this));
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        LevelManager.PreloadLevels();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MonoGameOrange);

        base.Draw(gameTime);
    }
}