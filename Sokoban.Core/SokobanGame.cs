using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Localization;
using Sokoban.Core.Managers;
using Sokoban.Core.Screens;
using Sokoban.Core.Logic;

namespace Sokoban.Core;

public class SokobanGame : Game
{
    private readonly GraphicsDeviceManager graphicsDeviceManager;
    public ScreenManager ScreenManager { get; private init; }
    public LevelsManager LevelsManager { get; private init; }
    
    public SokobanGame()
    {
        Content.RootDirectory = "Content";

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Services.AddService(graphicsDeviceManager);
        graphicsDeviceManager.SupportedOrientations = 
            DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        graphicsDeviceManager.PreferredBackBufferWidth = 1280;
        graphicsDeviceManager.PreferredBackBufferHeight = 720;

        ScreenManager = new ScreenManager(this, graphicsDeviceManager);
        Components.Add(ScreenManager); 

        LevelsManager = new LevelsManager(this);
        Services.AddService(LevelsManager); 
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

        LevelsManager.PreloadLevels();
        TileCreator.Font = Content.Load<SpriteFont>("Fonts/Tiles");
        EntityCreator.Font = Content.Load<SpriteFont>("Fonts/Tiles");
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new(20, 20, 20));

        base.Draw(gameTime);
    }
}