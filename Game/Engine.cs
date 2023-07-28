using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MyGame;

public class Canvas : Form
{
    public Canvas()
    {
        WindowState = FormWindowState.Maximized;
        FormBorderStyle = FormBorderStyle.None;

        this.DoubleBuffered = true;

        this.KeyDown += (o, e) =>
        {
            if(e.KeyCode == Keys.Escape)
                Application.Exit();
        };
    }
}

public abstract class GameEngine
{
    public GameEngine()
    {
        Window = new Canvas();       
        RenderStack = new List<Body>(); 

        Window.Paint += OnRender;
        Window.KeyDown += OnKeyDown;
        Window.KeyUp += OnKeyUp;

        GameLoopThread = new Thread(GameLoop);

        GameLoopThread.Start();
        Application.Run(Window);
    }

    public Canvas Window = null;
    public Thread GameLoopThread = null;
    List<Body> RenderStack { get; set; }

    public static Dictionary<Keys, bool> KeyMap { get; set; }
        = new Dictionary<Keys, bool>()
        {
            {Keys.W, false},
            {Keys.S, false},
            {Keys.A, false},
            {Keys.D, false},
        };

    bool Running = false;

    public void AddBody(Body b)
    {
        if (b is null) return;

        this.RenderStack.Add(b);
    }   

    protected void OnRender(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        g.Clear(Color.White);

        List<Body> render = new List<Body>(RenderStack);

        foreach(var b in render)        
            b.Draw(g);        
    }    

    private void GameLoop(object obj)
    {
        OnLoad();
        while(this.Running)
        {
            Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
            OnUpdate();
            Thread.Sleep(1);
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if(!KeyMap.ContainsKey(e.KeyCode)) return;

        KeyMap[e.KeyCode] = true;
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if(!KeyMap.ContainsKey(e.KeyCode)) return;

        KeyMap[e.KeyCode] = false;
    }

    protected abstract void OnUpdate();
    protected abstract void OnLoad();
}

public sealed class Game : GameEngine
{
    private static Game current = null;
    public static Game Current 
    {
        get
        {
            current ??= new Game();
            return current;
        }
    }

    protected override void OnLoad()
    {
        TextBox tb = new TextBox(new(0, 0), "Teste");
        this.AddBody(tb);
    }

    protected override void OnUpdate()
    {
        
    }
}