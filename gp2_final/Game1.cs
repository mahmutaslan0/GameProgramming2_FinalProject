using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace gp2_final;

public enum GameState { MainMenu, Settings, Level1 }
public enum ArmyCommand { Attack, Defend }

public class Game1 : Game
{
    private List<Soldier> _soldiers;
    private Texture2D _soldierWalkTex;
    private Texture2D _soldierAttackTex;
    private Texture2D _soldierDeathTex;
    
    private List<Monster> _monsters;
    private Texture2D _monsterWalkTex;
    private Texture2D _monsterAttackTex;
    private Texture2D _monsterDeathTex;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _blankTexture;
    private SpriteFont _menuFont;
    
    private MouseState _prevMouseState;
    private KeyboardState _prevKeyboardState; 

    private GameState _currentState;
    private ArmyCommand _armyCommand = ArmyCommand.Attack;

    private MainMenu _mainMenu;
    private SettingsMenu _settingsMenu;

    private Texture2D _texBackground;

    private Texture2D _playerCastleTex, _enemyCastleTex;
    private Castle _playerCastle, _enemyCastle;

    private Button _btnSpawnSoldier;
    private Button _btnDefend;
    private Button _btnAttack;

    private float _monsterSpawnTimer = 0f;
    private float _monsterSpawnInterval = 4f; 

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _soldiers = new List<Soldier>();
        _monsters = new List<Monster>();
        
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        _currentState = GameState.MainMenu;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _soldierWalkTex = Content.Load<Texture2D>("soldier");
        _soldierAttackTex = Content.Load<Texture2D>("soldier_attack");
        _soldierDeathTex = Content.Load<Texture2D>("soldier_death");
        
        _monsterWalkTex = Content.Load<Texture2D>("canavar");
        _monsterAttackTex = Content.Load<Texture2D>("monster_attack");
        _monsterDeathTex = Content.Load<Texture2D>("monster_death");
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _blankTexture = new Texture2D(GraphicsDevice, 1, 1);
        _blankTexture.SetData(new[] { Color.White });
        _menuFont = Content.Load<SpriteFont>("MenuFont");

        _texBackground = Content.Load<Texture2D>("background"); 
        
        _playerCastleTex = Content.Load<Texture2D>("dusman_kalesi"); 
        _enemyCastleTex = Content.Load<Texture2D>("cave");

        _mainMenu = new MainMenu(_menuFont, _blankTexture, 
            onStart: () => 
            {
                _soldiers.Clear();
                _monsters.Clear();
                if (_playerCastle != null) _playerCastle.CurrentHp = _playerCastle.MaxHp;
                if (_enemyCastle != null) _enemyCastle.CurrentHp = _enemyCastle.MaxHp;
                _monsterSpawnTimer = 0f;
                _armyCommand = ArmyCommand.Attack;
                _currentState = GameState.Level1;
            }, 
            onSettings: () => _currentState = GameState.Settings, 
            onExit: Exit);

        _settingsMenu = new SettingsMenu(_menuFont, _blankTexture, _graphics, 
            onBack: () => _currentState = GameState.MainMenu);

        _playerCastle = new Castle(new Rectangle(-220, 120, 460, 420), 1000f, _playerCastleTex);
        _enemyCastle = new Castle(new Rectangle(580, 200, 320, 300), 1000f, _enemyCastleTex);

        _btnSpawnSoldier = new Button(new Rectangle(20, 540, 120, 40), "SOLDIER", _menuFont, Color.White);
        _btnSpawnSoldier.OnClick += () => 
        {
            System.Random rnd = new System.Random();
            _soldiers.Add(new Soldier(_soldierWalkTex, _soldierAttackTex, _soldierDeathTex, new Vector2(200f, rnd.Next(350, 430))));
        };

        _btnDefend = new Button(new Rectangle(150, 540, 120, 40), "DEFENCE", _menuFont, Color.LightSkyBlue);
        _btnDefend.OnClick += () => { _armyCommand = ArmyCommand.Defend; };

        _btnAttack = new Button(new Rectangle(280, 540, 120, 40), "ATTACK", _menuFont, Color.IndianRed);
        _btnAttack.OnClick += () => { _armyCommand = ArmyCommand.Attack; };
    }

    protected override void Update(GameTime gameTime)
    {
        MouseState rawMouse = Mouse.GetState();
        KeyboardState currentKeyboard = Keyboard.GetState(); 
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                if (currentKeyboard.IsKeyDown(Keys.Escape)) _currentState = GameState.MainMenu;

                _btnSpawnSoldier.Update(currentMouseState, _prevMouseState);
                _btnDefend.Update(currentMouseState, _prevMouseState);
                _btnAttack.Update(currentMouseState, _prevMouseState);

                _monsterSpawnTimer += dt;
                if (_monsterSpawnTimer >= _monsterSpawnInterval)
                {
                    _monsterSpawnTimer = 0f;
                    System.Random rnd = new System.Random();
                    _monsters.Add(new Monster(_monsterWalkTex, _monsterAttackTex, _monsterDeathTex, new Vector2(600f, rnd.Next(300, 350))));
                }

                foreach (var s in _soldiers)
                {
                    if (s.IsDead) continue;
                    
                    if (_armyCommand == ArmyCommand.Defend && s.Position.X >= 380f)
                        s.State = UnitState.Idle;
                    else
                        s.State = UnitState.Walking;
                    s.Target = null;
                }

                foreach (var m in _monsters)
                {
                    if (m.IsDead) continue;
                    
                    m.State = UnitState.Walking;
                    m.Target = null;
                }

                foreach (var s in _soldiers)
                {
                    foreach (var m in _monsters)
                    {
                        if (s.IsDead || m.IsDead) continue;

                        if (System.Math.Abs(s.Position.X - m.Position.X) < 70f)
                        {
                            s.State = UnitState.Attacking;
                            m.State = UnitState.Attacking;
                            s.Target = m;
                            m.Target = s;
                            break; 
                        }
                    }
                }

                foreach (var s in _soldiers)
                {
                    if (s.IsDead) continue;
                    if (s.Position.X > 580f && s.State != UnitState.Attacking)
                    {
                        s.State = UnitState.Attacking;
                        _enemyCastle.TakeDamage(20f * dt);
                    }
                }

                foreach (var m in _monsters)
                {
                    if (m.IsDead) continue;
                    if (m.Position.X < 220f && m.State != UnitState.Attacking)
                    {
                        m.State = UnitState.Attacking;
                        _playerCastle.TakeDamage(20f * dt);
                    }
                }

                foreach (var soldier in _soldiers) soldier.Update(gameTime);
                foreach (var monster in _monsters) monster.Update(gameTime);

                _soldiers.RemoveAll(s => s.IsReadyToDespawn);
                _monsters.RemoveAll(m => m.IsReadyToDespawn);

                if (_playerCastle.CurrentHp <= 0 || _enemyCastle.CurrentHp <= 0)
                {
                    _currentState = GameState.MainMenu; 
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
            
            foreach (var soldier in _soldiers) soldier.Draw(_spriteBatch, _blankTexture, SpriteEffects.None);
            foreach (var monster in _monsters) monster.Draw(_spriteBatch, _blankTexture, SpriteEffects.None);
            
            _spriteBatch.End();

            _spriteBatch.Begin(transformMatrix: scaleMatrix);
            
            _btnSpawnSoldier.Draw(_spriteBatch, _blankTexture);
            _btnDefend.Draw(_spriteBatch, _blankTexture);
            _btnAttack.Draw(_spriteBatch, _blankTexture);

            float playerCastlePercent = _playerCastle.CurrentHp / _playerCastle.MaxHp;
            _spriteBatch.Draw(_blankTexture, new Rectangle(40, 20, 260, 20), Color.DarkRed);
            _spriteBatch.Draw(_blankTexture, new Rectangle(40, 20, (int)(260 * playerCastlePercent), 20), Color.Green);
            _spriteBatch.DrawString(_menuFont, $"Castle HP: {(int)_playerCastle.CurrentHp}", new Vector2(50, 22), Color.White);

            float totalSoldierHp = 0;
            float maxSoldierHp = 0;
            foreach (var s in _soldiers)
            {
                if (!s.IsDead)
                {
                    totalSoldierHp += s.CurrentHp;
                    maxSoldierHp += s.MaxHp;
                }
            }
            float soldierPercent = maxSoldierHp > 0 ? totalSoldierHp / maxSoldierHp : 0f;
            _spriteBatch.Draw(_blankTexture, new Rectangle(40, 45, 260, 20), Color.DarkRed);
            _spriteBatch.Draw(_blankTexture, new Rectangle(40, 45, (int)(260 * soldierPercent), 20), Color.Blue);
            _spriteBatch.DrawString(_menuFont, $"SOLDIER HP: {(int)totalSoldierHp}", new Vector2(50, 47), Color.White);

            float enemyCastlePercent = _enemyCastle.CurrentHp / _enemyCastle.MaxHp;
            _spriteBatch.Draw(_blankTexture, new Rectangle(500, 20, 260, 20), Color.DarkRed);
            _spriteBatch.Draw(_blankTexture, new Rectangle(500, 20, (int)(260 * enemyCastlePercent), 20), Color.Green);
            _spriteBatch.DrawString(_menuFont, $"CAVE HP: {(int)_enemyCastle.CurrentHp}", new Vector2(510, 22), Color.White);

            float totalMonsterHp = 0;
            float maxMonsterHp = 0;
            foreach (var m in _monsters)
            {
                if (!m.IsDead)
                {
                    totalMonsterHp += m.CurrentHp;
                    maxMonsterHp += m.MaxHp;
                }
            }
            float monsterPercent = maxMonsterHp > 0 ? totalMonsterHp / maxMonsterHp : 0f;
            _spriteBatch.Draw(_blankTexture, new Rectangle(500, 45, 260, 20), Color.DarkRed);
            _spriteBatch.Draw(_blankTexture, new Rectangle(500, 45, (int)(260 * monsterPercent), 20), Color.Purple);
            _spriteBatch.DrawString(_menuFont, $"MONSTER HP: {(int)totalMonsterHp}", new Vector2(510, 47), Color.White);

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