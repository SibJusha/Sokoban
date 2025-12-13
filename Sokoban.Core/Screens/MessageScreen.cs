using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public class MessageScreen : Screen
{
    private string message;

    public MessageScreen(SokobanGame game, string messageText) : base(game)
    {
        message = messageText;
    }

    public override void Draw(GameTime gameTime)
    {
        var spriteBatch = ScreenManager.SpriteBatch;
        var font = ScreenManager.Font;
        var position = new Vector2(
            (ScreenManager.ScreenSize.X - font.MeasureString(message).X) / 2, 
            (ScreenManager.ScreenSize.Y - font.MeasureString(message).Y) / 2);

        spriteBatch.DrawString(font, message, position, Color.White);
    }

    public override void HandleInput(GameTime gameTime, InputManager inputManager)
    {
        if (inputManager.IsCanceled() || inputManager.IsSelected())
            Exit();
    }

    public override void Update(GameTime gameTime)
    {
    }
}