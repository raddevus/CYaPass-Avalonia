using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CYaPass_Avalonia.Models;

public class PwdGrid : Control
{
    private const int NumCells = 6;
    private const int CellSize = 50;
    private const int PostRadius = 6;
    private int pointTrack = 0;
    private HashSet<int> postIndexes = new();
    public String SiteKey{get;set;}
    private Point? firstPoint = null;
    private UserPath up = new();
   public bool IsPatternHidden{get;set;} = false;
   private int loopCount = 0;
    private readonly List<Point> _userPoints = new();
    private readonly HashSet<string> _usedSegments = new();   // prevent duplicates
    private readonly List<(Point A, Point B)> _segments = new();

    public static readonly DirectProperty<PwdGrid, string?> GeneratedPasswordProperty =
        AvaloniaProperty.RegisterDirect<PwdGrid, string?>(
            nameof(GeneratedPassword),
            o => o.GeneratedPassword);

    private string? _generatedPassword;
    public string? GeneratedPassword
    {
        get => _generatedPassword;
        private set => SetAndRaise(GeneratedPasswordProperty, ref _generatedPassword, value);
    }

    public PwdGrid()
    {
        PointerPressed += OnPointerPressed;
    }
   
    public void ForceRender(){
       InvalidateVisual();
    }

    public override void Render(DrawingContext ctx)
    {
        base.Render(ctx);

        DrawBackground(ctx);
        DrawGridLines(ctx);
        DrawPosts(ctx);
        if (!IsPatternHidden){
            DrawUserShape(ctx);

           if (firstPoint != null){
               DrawHighlightCircle(ctx, firstPoint?.X ?? 0D, firstPoint?.Y ?? 0D);
           }
        }
    }

    // -----------------------------
    // Drawing
    // -----------------------------

    private void DrawBackground(DrawingContext ctx)
    {
        ctx.FillRectangle(Brushes.WhiteSmoke, new Rect(0, 0, Bounds.Width, Bounds.Height));
    }

    private void DrawGridLines(DrawingContext ctx)
    {
        var pen = new Pen(Brushes.DarkGray, 2);

        for (int i = 0; i < NumCells; i++)
        {
            ctx.DrawLine(pen, new Point(0, i * CellSize), new Point((NumCells-1) * CellSize, i * CellSize));
            ctx.DrawLine(pen, new Point(i * CellSize, 0), new Point(i * CellSize, (NumCells-1) * CellSize));
        }
    }

    private void DrawPosts(DrawingContext ctx)
    {
        var brush = Brushes.OrangeRed;

        for (int x = 0; x < NumCells; x++)
        {
            for (int y = 0; y < NumCells; y++)
            {
                var px = x * CellSize;
                var py = y * CellSize;
                ctx.DrawEllipse(
                    brush,
                    new Pen(brush, 1),
                    new Point(px, py),
                    PostRadius,
                    PostRadius);
            }
        }
    }
   private void DrawHighlightCircle(DrawingContext ctx, double px, double py){
      var brush = Brushes.Orange;
      System.Console.WriteLine($"px: {px} py: {py}");
      ctx.DrawEllipse(
            null,
            new Pen(brush, 3),
            new Point(px, py),
            12,
            12);

   }
    private void DrawUserShape(DrawingContext ctx)
    {
        var pen = new Pen(Brushes.Green, 4);

        foreach (var seg in _segments)
        {
            ctx.DrawLine(pen, seg.A, seg.B);
        }
    }

    // -----------------------------
    // Pointer Input
    // -----------------------------

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
       // If the pattern is hidden then the user cannot
       // draw on the control - just return
       if (IsPatternHidden){return;}

        var pos = e.GetPosition(this);
        if (TryHitPost(pos, out var snapped))
        {
            if (firstPoint == null) { firstPoint = snapped;}
            AddUserPoint(snapped);
            InvalidateVisual();
        }
    }

    // -----------------------------
    // Hit Testing
    // -----------------------------

    private bool TryHitPost(Point p, out Point snapped)
    {
       loopCount = 0;
        for (int x = 0; x < NumCells; x++)
        {
           
            for (int y = 0; y < NumCells; y++)
            {
               //loopCount++;
                var px = x * CellSize;
                var py = y * CellSize;

                var dx = p.X - px;
                var dy = p.Y - py;

                if (Math.Sqrt(dx * dx + dy * dy) <= PostRadius * 2)
                {
                    snapped = new Point(px, py);
                    postIndexes.Add(loopCount);
                    return true;
                }
                loopCount++;
            }
        }

        snapped = default;
        return false;
    }

    // -----------------------------
    // User Path Logic
    // -----------------------------

    private void AddUserPoint(Point p)
    {
        if (_userPoints.Count > 0)
        {
            var last = _userPoints[^1];

            // Prevent duplicate segments (in either direction)
            string key1 = $"{last.X},{last.Y}-{p.X},{p.Y}";
            string key2 = $"{p.X},{p.Y}-{last.X},{last.Y}";

            if (!_usedSegments.Contains(key1) && !_usedSegments.Contains(key2))
            {
                _segments.Add((last, p));
                _usedSegments.Add(key1);
                _usedSegments.Add(key2);
            }
        }
         up.append(new System.Drawing.Point((int)p.X, (int)p.Y), calculatePoints());
         up.CalculateGeometricValue();
         Console.WriteLine($"Userpath points: {up.PointValue}");
        _userPoints.Add(p);


        // Optional: auto-generate password when path is long enough
        if (_segments.Count >= 1)
            GeneratedPassword = GeneratePassword();
    }


    public void UpdatePassword(int multiHash){
       GeneratedPassword = GeneratePassword(multiHash);
    }

    // -----------------------------
    // Password Generation
    // -----------------------------

    private string GeneratePassword(int multiHash = 0)
    {
        // Convert each segment into a direction code
        var codes = _segments.Select(seg => EncodeDirection(seg.A, seg.B));

        // Combine into a single string
        string combined = $"{up.PointValue}{SiteKey}"; //string.Join("-", codes);

        // Hash it for security
        var hashResult = Sha256(combined).ToLower();
         for (int counter = 1; counter <= multiHash; counter++){
            hashResult = Sha256($"{up.PointValue}{hashResult}").ToLower();
            Console.WriteLine($"hashResult: {hashResult}");
         }
        return hashResult;
    }

    private int calculatePoints(){
      return (int) (loopCount + (loopCount * Math.Truncate((decimal)(loopCount / 6) * 10)));
    }

    private string EncodeDirection(Point a, Point b)
    {
        int dx = Math.Sign(b.X - a.X);
        int dy = Math.Sign(b.Y - a.Y);

        return (dx, dy) switch
        {
            (1, 0) => "R",
            (-1, 0) => "L",
            (0, 1) => "D",
            (0, -1) => "U",
            (1, 1) => "DR",
            (1, -1) => "UR",
            (-1, 1) => "DL",
            (-1, -1) => "UL",
            _ => "?"
        };
    }

    private string Sha256(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    // -----------------------------
    // Reset
    // -----------------------------

    public void Reset()
    {
       IsPatternHidden = false;
       up = new();
       pointTrack = 0;
       postIndexes = new();
        firstPoint = null; 
        _userPoints.Clear();
        _segments.Clear();
        _usedSegments.Clear();
        GeneratedPassword = String.Empty;
        InvalidateVisual();
    }
}

