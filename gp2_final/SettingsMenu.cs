using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace gp2_final;

public class SettingsMenu
{
    private Button _btnFullscreen;
    private Button _btnMusicUp;
    private Button _btnMusicDown;
    private Button _btnSfxUp;
    private Button _btnSfxDown;
    private Button _btnBack;

    private SpriteFont _font;
    private Texture2D _blankTexture;
    private GraphicsDeviceManager _graphics;

    public float MusicVolume { get; private set; } = 0.5f;
    public float SfxVolume { get; private set; } = 0.5f;

    public SettingsMenu(SpriteFont font, Texture2D blankTexture, GraphicsDeviceManager graphics, Action onBack)
    {
        _font = font;
        _blankTexture = blankTexture;
        _graphics = graphics;

        _btnFullscreen = new Button(new Rectangle(300, 150, 200, 50), "TOGGLE FULLSCREEN", _font, Color.LightSkyBlue);
        _btnFullscreen.OnClick += () => 
        {
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        };

        
        _btnMusicDown = new Button(new Rectangle(430, 220, 40, 40), "-", _font, Color.Gray);
        _btnMusicDown.OnClick += () => 
        {
            MusicVolume = MathHelper.Clamp(MusicVolume - 0.1f, 0f, 1f);
            MediaPlayer.Volume = MusicVolume;
        };

        _btnMusicUp = new Button(new Rectangle(480, 220, 40, 40), "+", _font, Color.Gray);
        _btnMusicUp.OnClick += () => 
        {
            MusicVolume = MathHelper.Clamp(MusicVolume + 0.1f, 0f, 1f);
            MediaPlayer.Volume = MusicVolume;
        };

        
        _btnSfxDown = new Button(new Rectangle(430, 280, 40, 40), "-", _font, Color.Gray);
        _btnSfxDown.OnClick += () => 
        {
            SfxVolume = MathHelper.Clamp(SfxVolume - 0.1f, 0f, 1f);
            SoundEffect.MasterVolume = SfxVolume;
        };

        _btnSfxUp = new Button(new Rectangle(480, 280, 40, 40), "+", _font, Color.Gray);
        _btnSfxUp.OnClick += () => 
        {
            SfxVolume = MathHelper.Clamp(SfxVolume + 0.1f, 0f, 1f);
            SoundEffect.MasterVolume = SfxVolume;
        };

        _btnBack = new Button(new Rectangle(300, 380, 200, 50), "BACK", _font, Color.LightGray);
        _btnBack.OnClick += onBack;
    }

    public void Update(MouseState currentMouseState, MouseState prevMouseState)
    {
        _btnFullscreen.Update(currentMouseState, prevMouseState);
        _btnMusicDown.Update(currentMouseState, prevMouseState);
        _btnMusicUp.Update(currentMouseState, prevMouseState);
        _btnSfxDown.Update(currentMouseState, prevMouseState);
        _btnSfxUp.Update(currentMouseState, prevMouseState);
        _btnBack.Update(currentMouseState, prevMouseState);
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        graphicsDevice.Clear(Color.DarkSlateGray);
        spriteBatch.DrawString(_font, "SETTINGS", new Vector2(350, 80), Color.White);
        
        spriteBatch.DrawString(_font, $"MUSIC: {MusicVolume:P0}", new Vector2(280, 230), Color.White);
        spriteBatch.DrawString(_font, $"SFX: {SfxVolume:P0}", new Vector2(280, 290), Color.White);

        _btnFullscreen.Draw(spriteBatch, _blankTexture);
        _btnMusicDown.Draw(spriteBatch, _blankTexture);
        _btnMusicUp.Draw(spriteBatch, _blankTexture);
        _btnSfxDown.Draw(spriteBatch, _blankTexture);
        _btnSfxUp.Draw(spriteBatch, _blankTexture);
        _btnBack.Draw(spriteBatch, _blankTexture);
    }
}