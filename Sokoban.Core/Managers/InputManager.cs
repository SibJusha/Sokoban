using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sokoban.Core.Managers;

public enum MouseButton
{
    Left, Right, Middle, XButton1, XButton2
}

public class InputManager
{
    // public MouseButton CurrentMouseButton { get; private set; }

    private KeyboardState currentKeyboardState = new(); 
    private KeyboardState prevKeyboardState = new();

    private MouseState currentMouseState = new();
    private MouseState prevMouseState = new();

    public void Update(GameTime gameTime)
    {
        prevKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();

        prevMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();
    }

    public bool IsUp()
    {
        return IsKeyPressed(Keys.Up) || IsKeyPressed(Keys.W); 
    }

    public bool IsDown()
    {
        return IsKeyPressed(Keys.Down) || IsKeyPressed(Keys.S); 
    }

    public bool IsCanceled()
    {
        return IsKeyPressed(Keys.Escape); 
    }

    public bool IsSelected()
    {
        return IsKeyPressed(Keys.Enter); 
    }

    private bool IsKeyPressed(Keys key)
    {
        return currentKeyboardState.IsKeyDown(key) 
                && prevKeyboardState.IsKeyUp(key);
    }

    // public bool IsLeftMouseButtonClicked()
    // {
    //     return CurrentMouseButton.Left 
    // }
}