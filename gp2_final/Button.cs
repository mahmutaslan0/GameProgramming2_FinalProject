using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace gp2_final;

public class Button
{
    private Rectangle _rectangle;
    private Color _color;
    private SpriteFont _font;
    private bool _isHovered;

    public string Text { get; set; }

    public event Action OnClick;

    public Button(Rectangle rectangle, string text, SpriteFont font, Color color)
    {
        _rectangle = rectangle;
        Text = text;
        _font = font;
        _color = color;
    }

    public void Update(MouseState mouseState, MouseState prevMouseState)
    {
        _isHovered = _rectangle.Contains(mouseState.Position);

        if (_isHovered && mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
        {
            OnClick?.Invoke();
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D blankTexture)
    {
        Color drawColor = _isHovered ? Color.Gray : _color;

        spriteBatch.Draw(blankTexture, _rectangle, drawColor);

        if (!string.IsNullOrEmpty(Text) && _font != null)
        {
            Vector2 textSize = _font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                _rectangle.X + (_rectangle.Width / 2) - (textSize.X / 2),
                _rectangle.Y + (_rectangle.Height / 2) - (textSize.Y / 2)
            );

            spriteBatch.DrawString(_font, Text, textPosition, Color.Black);
        }
    }
}