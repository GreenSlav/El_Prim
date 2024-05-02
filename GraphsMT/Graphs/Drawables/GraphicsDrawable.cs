using Graphs.Models;
using Graphs.ViewModels;
using Font = Microsoft.Maui.Graphics.Font;

namespace Graphs.Drawables;

public class GraphicsDrawable : IDrawable
{
    public static Dictionary<int, ValueTuple<int, int>> CoordsToDraw = new Dictionary<int, ValueTuple<int, int>>();
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        
        if (MainPageViewModel.SolvedMatrix != null && CoordsToDraw.Count != 0)
        {
            for (int i = 0; i < MainPageViewModel.SolvedMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < MainPageViewModel.SolvedMatrix.GetLength(1); j++)
                {
                    if (MainPageViewModel.SolvedMatrix[i, j] != 0)
                    {
                        Vertex vertex1 = new Vertex(i, CoordsToDraw[i].Item1, CoordsToDraw[i].Item2);
                        Vertex vertex2 = new Vertex(j, CoordsToDraw[j].Item1, CoordsToDraw[j].Item2);
        
                        canvas.FillColor = Colors.Black;
                        canvas.StrokeSize = 0; 
                        canvas.FillEllipse(vertex1.X, vertex1.Y, 75, 75);
                        canvas.FillEllipse(vertex2.X, vertex2.Y, 75, 75);
        
                        canvas.FontColor = Colors.White;
                        canvas.FontSize = 25;
        
                        canvas.Font = Font.DefaultBold;
                        canvas.DrawString(vertex1.Number.ToString(), vertex1.X, vertex1.Y, 75, 75, HorizontalAlignment.Center, VerticalAlignment.Center);
                        canvas.DrawString(vertex2.Number.ToString(), vertex2.X, vertex2.Y, 75, 75, HorizontalAlignment.Center, VerticalAlignment.Center);
        
        
        
                        DrawLineBetweenVertexes(canvas, dirtyRect, vertex1, vertex2, 75);
                    }
                }
            }
        }
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
        
        
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 3;
        canvas.DrawLine(source, destination);
    }
}