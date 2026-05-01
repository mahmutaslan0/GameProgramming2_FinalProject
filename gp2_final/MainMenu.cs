using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace gp2_final;

public class MainMenu
{
    private Button _btnStart;
    private Button _btnSettings;
    private Button _btnExit;
    
    private SpriteFont _font;
    private Texture2D _blankTexture;

    
    public MainMenu(SpriteFont font, Texture2D blankTexture, Action onStart, Action onSettings, Action onExit)
    {
        _font = font;
        _blankTexture = blankTexture;

        _btnStart = new Button(new Rectangle(300, 200, 200, 50), "START GAME", _font, Color.LightGreen);
        _btnStart.OnClick += onStart;

        _btnSettings = new Button(new Rectangle(300, 270, 200, 50), "SETTINGS", _font, Color.LightGray);
        _btnSettings.OnClick += onSettings;

        _btnExit = new Button(new Rectangle(300, 340, 200, 50), "EXIT", _font, Color.IndianRed);
        _btnExit.OnClick += onExit;
    }

    public void Update(MouseState currentMouseState, MouseState prevMouseState)
    {
        _btnStart.Update(currentMouseState, prevMouseState);
        _btnSettings.Update(currentMouseState, prevMouseState);
        _btnExit.Update(currentMouseState, prevMouseState);
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        graphicsDevice.Clear(Color.DarkSlateBlue);
        
        spriteBatch.DrawString(_font, "TUG OF WAR", new Vector2(320, 100), Color.White);

        _btnStart.Draw(spriteBatch, _blankTexture);
        _btnSettings.Draw(spriteBatch, _blankTexture);
        _btnExit.Draw(spriteBatch, _blankTexture);
    }
}