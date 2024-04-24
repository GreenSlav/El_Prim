using Graphs.Models;
using Font = Microsoft.Maui.Graphics.Font;

namespace Graphs;

public class GraphicsDrawable : IDrawable
{
    private List<Vertex> VertexesToDraw = new();
    
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // Vertex vertex = new Vertex(3, 960-40, 540-40);
        // canvas.FillColor = ColorGenerator();
        // canvas.StrokeSize = 0;
        // canvas.FillEllipse(vertex.X, vertex.Y, 80, 80);
        
        
        
        // Vertex vertex1 = new Vertex(1, 1800, 800);
        // Vertex vertex2 = new Vertex(2, 200, 500);
        //
        // canvas.FillColor = Colors.Red;
        // canvas.StrokeSize = 0;
        // canvas.FillEllipse(vertex1.X, vertex1.Y, 75, 75);
        // canvas.FillEllipse(vertex2.X, vertex2.Y, 75, 75);
        //
        // canvas.FontColor = Colors.GreenYellow;
        // canvas.FontSize = 25;
        //
        // canvas.Font = Font.DefaultBold;
        // canvas.DrawString(vertex1.Number.ToString(), vertex1.X, vertex1.Y, 75, 75, HorizontalAlignment.Center, VerticalAlignment.Center);
        // canvas.DrawString(vertex2.Number.ToString(), vertex2.X, vertex2.Y, 75, 75, HorizontalAlignment.Center, VerticalAlignment.Center);
        //
        //
        //
        // DrawLineBetweenVertexes(canvas, dirtyRect, vertex1, vertex2, 75);
    }


    public void DrawLineBetweenVertexes(ICanvas canvas, RectF dirtyRect, Vertex vertexFirst, Vertex vertexSecond, int diameter)
    {
        double radius = diameter / 2.0;
        
        // AD
        double hypotenuse = Math.Pow(Math.Pow(vertexFirst.X - vertexSecond.X, 2)
                                     + Math.Pow(vertexFirst.Y - vertexSecond.Y, 2), 0.5);
        
        // ODA
        double firstAngle = Math.Abs(vertexFirst.Y - vertexSecond.Y) / hypotenuse;
        
        // MBD = ODA
        // MB
        double firstSmallerX = Math.Abs(vertexFirst.X - vertexSecond.X) * diameter / 2.0 / hypotenuse;
        // AM
        double firstSmallerY = Math.Pow(Math.Pow(diameter / 2.0, 2) - Math.Pow(firstSmallerX, 2), 0.5);
        
        
        
        // UD
        double secondSmallerX = diameter / 2.0 * Math.Abs(vertexFirst.X - vertexSecond.X) / hypotenuse;
        //CU
        double secondSmallerY = Math.Pow(Math.Pow(diameter / 2.0, 2) - Math.Pow(secondSmallerX, 2), 0.5);

        double x;
        double y;

        x = (vertexFirst.X < vertexSecond.X) ? vertexFirst.X + firstSmallerX : vertexFirst.X - firstSmallerX;
        y = (vertexFirst.Y < vertexSecond.Y) ? vertexFirst.Y + firstSmallerY : vertexFirst.Y - firstSmallerY;
        PointF source = new PointF((int)(x+radius), (int)(y+radius));

        x = (vertexFirst.X < vertexSecond.X) ? vertexSecond.X - secondSmallerX : vertexSecond.X + secondSmallerX;
        y = (vertexFirst.Y < vertexSecond.Y) ? vertexSecond.Y - secondSmallerY : vertexSecond.Y + secondSmallerY;
        PointF destination = new PointF((int)(x+radius), (int)(y+radius));
        
        LinearGradientPaint linearGradientPaint = new LinearGradientPaint
        {
            StartColor = Colors.Yellow,
            EndColor = Colors.Green,
            // StartPoint is already (0,0)
            // EndPoint is already (1,1)
        };
        canvas.StrokeColor = Colors.Red;
        canvas.StrokeSize = 3;
        canvas.DrawLine(source, destination);
        
    }
    
    
    public void DrawVertex(ICanvas canvas, RectF dirtyRect, int number)
    {
        
    }
    
    public void DrawLineToVertex(ICanvas canvas, RectF dirtyRect, PointF sourceVertex, PointF destinationVertex, Color color)
    {
        
    }

    public Color ColorGenerator()
    {
        return new Color(new Random().Next(0, 255), new Random().Next(0, 255), new Random().Next(0, 255));
    }
}