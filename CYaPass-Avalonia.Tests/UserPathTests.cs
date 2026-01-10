using CYaPass_Avalonia.Models;
using System.Drawing;

namespace CYaPass_Avalonia.Tests;

public class UserPathTests 
{
    [Fact]
    public void GeneratePointTest()
    {
         var up = new UserPath();
         up.append(new Point(32,64), 0);
         up.append(new Point(100,100), 1);
         up.CalculateGeometricValue();
         Console.WriteLine($"point value: {up.PointValue}");
    }
}
