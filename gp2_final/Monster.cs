using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gp2_final;

public class Monster : Unit
{
    public Monster(Texture2D walkTex, Texture2D attackTex, Texture2D deathTex, Vector2 startPos) 
        : base(walkTex, attackTex, deathTex, startPos, 8, 6, 4, 3f, 40f, 120f, 25f) 
    { 
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        if (State == UnitState.Walking && !IsDead)
        {
            Position.X -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}