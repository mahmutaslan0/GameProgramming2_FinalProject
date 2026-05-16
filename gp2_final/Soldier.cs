using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gp2_final;

public class Soldier : Unit
{
    public Soldier(Texture2D walkTex, Texture2D attackTex, Texture2D deathTex, Vector2 startPos) 
        : base(walkTex, attackTex, deathTex, startPos, 6, 4, 10, 1f, 60f, 100f, 20f) 
    { 
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        if (State == UnitState.Walking && !IsDead)
        {
            Position.X += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}