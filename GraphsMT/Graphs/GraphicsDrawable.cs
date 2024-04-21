namespace Graphs;

public class GraphicsDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        dirtyRect.Width = 1920;
        dirtyRect.Height = 1080;
        canvas.StrokeColor = Colors.Red;
        
        var strct = dirtyRect;
        var strctCenter = strct.Center;
        strctCenter.X = 1000;
        strctCenter.Y = 500;
        
        canvas.StrokeSize = 6;
        canvas.DrawLine(10, 10, 90, 100);
    }      
}