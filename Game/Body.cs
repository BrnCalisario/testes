using System.Drawing;

namespace MyGame;

public abstract class Body
{
    public Rectangle Rect { get; set; }
    
    public Pen Pen { get; set; }

    public virtual void Draw(Graphics g)
    {
        g.DrawRectangle(Pen, Rect);
    }
    
    public abstract void Update();
}

public class TextBox : Body
{
    public TextBox(Point? position, string text = "")
    {
        this.Text = text;

        var size = new Size(text.Length * (int)Font.Size, (int)Font.Size);
        this.Rect = new Rectangle(position ?? Point.Empty, size);

        this.Pen = Pens.Black;
    }

    public string Text { get; set; } = "";
    public Font Font { get; set; } = new Font("Arial", 16);

    public override void Draw(Graphics g)
    {
        g.DrawRectangle(Pen, Rect);
        g.DrawString(Text, Font, Pen.Brush, Rect);
    }

    public override void Update()
    {
        
    }
}