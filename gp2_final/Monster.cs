using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gp2_final;

public class Monster
{
    public Vector2 Position;
    public Texture2D Texture;
    
    public float Speed = 50f; 
    public float Scale = 1.5f; 
    public float Depth;

    private int _totalFrames = 8;
    private int _currentFrame = 0;
    private float _frameTimer = 0f;
    private float _frameSpeed = 0.12f;

    public Monster(Texture2D texture, Vector2 startPos)
    {
        Texture = texture;
        Position = startPos;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _frameTimer += deltaTime;
        if (_frameTimer >= _frameSpeed)
        {
            _currentFrame++;
            if (_currentFrame >= _totalFrames)
                _currentFrame = 0; 
            
            _frameTimer = 0f;
        }

        Position.X -= Speed * deltaTime;

        Depth = 0.8f + (Position.Y / 10000f);
        Depth = MathHelper.Clamp(Depth, 0.0f, 1.0f);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int frameWidth = Texture.Width / _totalFrames;
        int frameHeight = Texture.Height;

        Rectangle sourceRect = new Rectangle(_currentFrame * frameWidth, 0, frameWidth, frameHeight);

        spriteBatch.Draw(Texture, Position, sourceRect, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, Depth);
    }
}