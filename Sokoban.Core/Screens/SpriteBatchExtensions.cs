namespace Microsoft.Xna.Framework.Graphics;

public static class SpriteBatchExtensions
{
    public static void DrawStringWithShadow(this SpriteBatch spriteBatch,
        SpriteFont font,
        string text,
        Vector2 position,
        Color color)
    {
        var shadowPos = position + new Vector2(1f, 1f);
        spriteBatch.DrawString(font, text, shadowPos, Color.Black);
        spriteBatch.DrawString(font, text, position, color);
    }
}