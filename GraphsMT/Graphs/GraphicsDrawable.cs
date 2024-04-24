namespace Graphs;

public class GraphicsDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // dirtyRect.Width = 1920;
        // dirtyRect.Height = 1080;
        canvas.StrokeColor = Colors.Red;
        
        
        canvas.StrokeSize = 1;
        canvas.DrawLine(10, 10, 90, 100);
    }      
}