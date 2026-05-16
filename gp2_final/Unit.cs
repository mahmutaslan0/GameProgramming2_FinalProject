using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gp2_final;

public enum UnitState { Walking, Idle, Attacking, Dying }

public class Unit
{
    public Vector2 Position;
    public Texture2D WalkTexture;
    public Texture2D AttackTexture;
    public Texture2D DeathTexture;
    public float Speed;
    public float Scale;
    public float Depth;

    public float MaxHp;
    public float CurrentHp;
    public float Damage;
    public float AttackTimer = 0f;
    public float AttackSpeed = 1f; 

    public UnitState State = UnitState.Walking;
    public Unit Target; 

    public bool IsReadyToDespawn = false;

    protected int _walkFrames;
    protected int _attackFrames;
    protected int _deathFrames;
    protected int _currentFrame = 0;
    protected float _frameTimer = 0f;
    protected float _frameSpeed = 0.15f;

    public Unit(Texture2D walkTex, Texture2D attackTex, Texture2D deathTex, Vector2 startPos, int walkFrames, int attackFrames, int deathFrames, float scale, float speed, float maxHp, float damage)
    {
        WalkTexture = walkTex;
        AttackTexture = attackTex;
        DeathTexture = deathTex;
        Position = startPos;
        _walkFrames = walkFrames;
        _attackFrames = attackFrames;
        _deathFrames = deathFrames;
        Scale = scale;
        Speed = speed;
        MaxHp = maxHp;
        CurrentHp = maxHp;
        Damage = damage;
    }

    public bool IsDead => CurrentHp <= 0;

    public void TakeDamage(float amount)
    {
        CurrentHp -= amount;
    }

    public virtual void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (CurrentHp <= 0 && State != UnitState.Dying)
        {
            State = UnitState.Dying;
            _currentFrame = 0;
            _frameTimer = 0f;
        }

        int totalFrames = _walkFrames;
        if (State == UnitState.Attacking) totalFrames = _attackFrames;
        else if (State == UnitState.Dying) totalFrames = _deathFrames;

        if (State == UnitState.Walking || State == UnitState.Attacking || State == UnitState.Dying)
        {
            _frameTimer += deltaTime;
            if (_frameTimer >= _frameSpeed)
            {
                _currentFrame++;
                if (_currentFrame >= totalFrames)
                {
                    if (State == UnitState.Dying)
                    {
                        _currentFrame = totalFrames - 1;
                        IsReadyToDespawn = true; 
                    }
                    else
                    {
                        _currentFrame = 0; 
                    }
                }
                _frameTimer = 0f;
            }
        }
        else
        {
            _currentFrame = 0; 
        }

        if (State == UnitState.Attacking && Target != null && !IsDead)
        {
            AttackTimer += deltaTime;
            if (AttackTimer >= AttackSpeed)
            {
                Target.TakeDamage(Damage);
                AttackTimer = 0f;
            }
        }
        else
        {
            AttackTimer = 0f; 
        }

        Depth = 0.8f + (Position.Y / 10000f);
        Depth = MathHelper.Clamp(Depth, 0.0f, 1.0f);
    }

    public virtual void Draw(SpriteBatch spriteBatch, Texture2D blankTexture, SpriteEffects flipEffect)
    {
        Texture2D currentTexture = WalkTexture;
        int totalFrames = _walkFrames;

        if (State == UnitState.Attacking)
        {
            currentTexture = AttackTexture;
            totalFrames = _attackFrames;
        }
        else if (State == UnitState.Dying)
        {
            currentTexture = DeathTexture;
            totalFrames = _deathFrames;
        }

        if (_currentFrame >= totalFrames) _currentFrame = totalFrames - 1;

        int frameWidth = currentTexture.Width / totalFrames;
        int frameHeight = currentTexture.Height;
        Rectangle sourceRect = new Rectangle(_currentFrame * frameWidth, 0, frameWidth, frameHeight);

        spriteBatch.Draw(currentTexture, Position, sourceRect, Color.White, 0f, Vector2.Zero, Scale, flipEffect, Depth);
    }
}