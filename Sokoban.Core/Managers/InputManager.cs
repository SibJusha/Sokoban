using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sokoban.Core.Logic;

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

    public Direction GetDirection()
    {
        var pressed = new[]
        {
            (Keys.Up,    Direction.Up),    (Keys.W, Direction.Up),
            (Keys.Down,  Direction.Down),  (Keys.S, Direction.Down),
            (Keys.Left,  Direction.Left),  (Keys.A, Direction.Left),
            (Keys.Right, Direction.Right), (Keys.D, Direction.Right)
        };

        var active = pressed.Where(k => IsKeyPressed(k.Item1)).ToList();

        return active.Count >= 2 ? Direction.None : active.FirstOrDefault().Item2;
    }

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