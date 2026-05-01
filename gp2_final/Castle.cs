using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gp2_final; 

public class Castle
{
    public Rectangle Hitbox { get; set; }
    public float MaxHp { get; set; }
    public float CurrentHp { get; set; }
    
    private Texture2D _texture; 

    public Castle(Rectangle hitbox, float maxHp, Texture2D texture)
    {
        Hitbox = hitbox;
        MaxHp = maxHp;
        CurrentHp = maxHp;
        _texture = texture;
    }

    public void TakeDamage(float amount)
    {
        CurrentHp -= amount;
        if (CurrentHp < 0) CurrentHp = 0;
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font)
    {
        
        float layerDepth = 0.5f;

        
        spriteBatch.Draw(
            _texture, 
            Hitbox, 
            null, 
            Color.White, 
            0f, 
            Vector2.Zero, 
            SpriteEffects.None, 
            layerDepth 
        );

        string hpText = $"HP: {CurrentHp} / {MaxHp}";
        Vector2 textSize = font.MeasureString(hpText);
        Vector2 textPos = new Vector2(
            Hitbox.X + (Hitbox.Width / 2) - (textSize.X / 2), 
            Hitbox.Y - 25
        );

        
        spriteBatch.DrawString(
            font, 
            hpText, 
            textPos, 
            Color.White, 
            0f, 
            Vector2.Zero, 
            1f, 
            SpriteEffects.None, 
            layerDepth + 0.01f 
        );
    }
}