using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace gp2_final;

public enum GameState { MainMenu, Settings, Level1 }

public class Game1 : Game
{
    private List<Soldier> _soldiers;
    private Texture2D _soldierTex;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _blankTexture;
    private SpriteFont _menuFont;
    
    
    private MouseState _prevMouseState;
    private KeyboardState _prevKeyboardState; 

    private GameState _currentState;

    private MainMenu _mainMenu;
    private SettingsMenu _settingsMenu;

    
    private Texture2D _texBackground;

    
    private Texture2D _playerCastleTex, _enemyCastleTex;
    private Castle _playerCastle, _enemyCastle;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _soldiers = new List<Soldier>();
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        _currentState = GameState.MainMenu;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _soldierTex = Content.Load<Texture2D>("soldier");
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _blankTexture = new Texture2D(GraphicsDevice, 1, 1);
        _blankTexture.SetData(new[] { Color.White });
        _menuFont = Content.Load<SpriteFont>("MenuFont");

        
        _texBackground = Content.Load<Texture2D>("background"); 
        
        _playerCastleTex = Content.Load<Texture2D>("dusman_kalesi"); 
        _enemyCastleTex = Content.Load<Texture2D>("cave");

        
        _mainMenu = new MainMenu(_menuFont, _blankTexture, 
            onStart: () => _currentState = GameState.Level1, 
            onSettings: () => _currentState = GameState.Settings, 
            onExit: Exit);

        _settingsMenu = new SettingsMenu(_menuFont, _blankTexture, _graphics, 
            onBack: () => _currentState = GameState.MainMenu);

        
        _playerCastle = new Castle(new Rectangle(-220, 120, 460, 420), 1000f, _playerCastleTex);
        _enemyCastle = new Castle(new Rectangle(580, 200, 320, 300), 1000f, _enemyCastleTex);
    }

    protected override void Update(GameTime gameTime)
    {
        MouseState rawMouse = Mouse.GetState();
        KeyboardState currentKeyboard = Keyboard.GetState(); 

        float scaleX = (float)_graphics.PreferredBackBufferWidth / 800f;
        float scaleY = (float)_graphics.PreferredBackBufferHeight / 600f;

        MouseState currentMouseState = new MouseState(
            (int)(rawMouse.X / scaleX), (int)(rawMouse.Y / scaleY),
            rawMouse.ScrollWheelValue, rawMouse.LeftButton, rawMouse.MiddleButton, 
            rawMouse.RightButton, rawMouse.XButton1, rawMouse.XButton2);

        switch (_currentState)
        {
            case GameState.MainMenu:
                _mainMenu.Update(currentMouseState, _prevMouseState);
                break;
            case GameState.Settings:
                _settingsMenu.Update(currentMouseState, _prevMouseState);
                break;
            case GameState.Level1:
                if (currentKeyboard.IsKeyDown(Keys.Escape))
                    _currentState = GameState.MainMenu;

                
                if (currentKeyboard.IsKeyDown(Keys.D2) && _prevKeyboardState.IsKeyUp(Keys.D2))
                {
                    System.Random rnd = new System.Random();
                    
                    float randomY = rnd.Next(250, 450); 
                    _soldiers.Add(new Soldier(_soldierTex, new Vector2(150, randomY)));
                }

                
                foreach (var soldier in _soldiers)
                {
                    soldier.Update(gameTime);
                }
                break;
        }

        _prevMouseState = currentMouseState;
        _prevKeyboardState = currentKeyboard; 
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float scaleX = (float)_graphics.PreferredBackBufferWidth / 800f;
        float scaleY = (float)_graphics.PreferredBackBufferHeight / 600f;
        Matrix scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1f);

        if (_currentState == GameState.Level1)
        {
            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, transformMatrix: scaleMatrix);
            
            GraphicsDevice.Clear(Color.Black); 

            
            _spriteBatch.Draw(_texBackground, new Rectangle(0, 0, 800, 600), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.0f);

            
            _playerCastle.Draw(_spriteBatch, _menuFont);
            _enemyCastle.Draw(_spriteBatch, _menuFont);
            
            
            foreach (var soldier in _soldiers)
            {
                soldier.Draw(_spriteBatch);
            }
            
            _spriteBatch.End();
        }
        else
        {
            _spriteBatch.Begin(transformMatrix: scaleMatrix);
            if (_currentState == GameState.MainMenu) _mainMenu.Draw(_spriteBatch, GraphicsDevice);
            else if (_currentState == GameState.Settings) _settingsMenu.Draw(_spriteBatch, GraphicsDevice);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }
}