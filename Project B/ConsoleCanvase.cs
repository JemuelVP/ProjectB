using Spectre.Console;


public class ConsoleCanvas
{
    private struct CanvasPixel
    {
        public char Character;
        public ConsoleColor Color;
    }

    private int width;
    private int height;
    private CanvasPixel[,] canvas;
    public int cursorX { get; private set; }
    public int cursorY { get; private set; }

    public ConsoleCanvas(int width, int height)
    {
        this.width = width;
        this.height = height;
        canvas = new CanvasPixel[width, height];
        ClearCanvas();
        cursorX = 0;
        cursorY = 0;
    }

    public void ClearCanvas()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                canvas[x, y] = new CanvasPixel { Character = ' ', Color = ConsoleColor.White };
            }
        }
    }

    public void DrawSmallCanvas(Canvas canvas, int schedule_id)
    {
        
        for (var row = 0; row < canvas.Height; row++)
        {
            for (var col = 0; col < canvas.Width; col++)
            {
                using (DataBaseConnection db = new DataBaseConnection())
                {
                    var chair = db.Chair.FirstOrDefault(c => c.Row == row && c.Column == col && c.CinemaHallID == 1);
                    if (chair != null)
                    {
                        var ticket = db.Ticket.FirstOrDefault(t => t.Schedule_ID == schedule_id && t.Chair_ID == chair.ID);
                        if (ticket != null)
                        {
                            canvas.SetPixel(col, row, Color.Grey); // Chair is not available, color it gray
                            continue; // Skip to the next iteration
                        }
                    }
                }

                // Set colors for other areas
                if (row >= 5 && row <= 8 && col >= 5 && col <= 6)
                {
                    canvas.SetPixel(col, row, Color.Red3);
                }
                else if 
                (
                    (row >= 3 && row <= 4 && col >= 5 && col <= 6) ||
                    (row >= 9 && row <= 10 && col >= 5 && col <= 6 ) ||
                    (row >= 4 && row <= 9 && col >= 4 && col <= 7 ) ||
                    (row >= 5 && row <= 8 && col >= 3 && col <= 8 )
                )
                {
                    canvas.SetPixel(col, row, Color.DarkOrange3_1);
                }
                else if 
                (
                    (row >= 0 && row <= 2 && col == 0) ||
                    (row >= 0 && row <= 2 && col == 11) ||
                    (row == 0 && col >= 0 && col <= 1) ||
                    (row == 0 && col >= 10 && col <= 11) ||
                    (row >= 11 && row <= 13 && col == 0) ||
                    (row >= 12 && row <= 13 && col == 1) ||
                    (row >= 11 && row <= 13 && col == 11)||
                    (row >= 12 && row <= 13 && col == 10)
                )
                {
                    canvas.SetPixel(col, row, Color.Black);
                }
                else
                {
                    canvas.SetPixel(col, row, Color.DodgerBlue3);
                }
            }
        }
    }

    public void DrawMediumCanvas(Canvas canvas, int schedule_id)
    {
        for (var row = 0; row < canvas.Height; row++)
        {
            for (var col = 0; col < canvas.Width; col++)
            {
                using (DataBaseConnection db = new DataBaseConnection())
                {
                    var chair = db.Chair.FirstOrDefault(c => c.Row == row && c.Column == col && c.CinemaHallID == 2);
                    if (chair != null)
                    {
                        var ticket = db.Ticket.FirstOrDefault(t => t.Schedule_ID == schedule_id && t.Chair_ID == chair.ID);
                        if (ticket != null)
                        {
                            canvas.SetPixel(col, row, Color.Grey); // Chair is not available, color it gray
                            continue; // Skip to the next iteration
                        }
                    }
                }
                if (
                    (row >= 5 && row <= 12 && col >= 8 && col <= 9) ||
                    (row >= 6 && row <= 11 && col >= 7 && col <= 10) ||
                    (row >= 7 && row <= 10 && col >= 6 && col <= 11))
                {
                    canvas.SetPixel(col, row, Color.Red3);
                }
                else if
                (
                    (row >= 1 && row <= 15 && col >= 6 && col <= 11) ||
                    (row >= 2 && row <= 13 && col >= 5 && col <= 12) ||
                    (row >= 4 && row <= 12 && col >= 4 && col <= 13) ||
                    (row >= 6 && row <= 11 && col >= 3 && col <= 14) ||
                    (row >= 8 && row <= 10 && col >= 2 && col <= 15)
                )
                {
                    canvas.SetPixel(col, row, Color.DarkOrange3_1);
                }
                else if
                (
                    (row >= 0 && row <= 5 && col == 0) ||
                    (row >= 0 && row <= 5 && col == 17) ||
                    (row >= 11 && row <= 18 && col == 0) ||
                    (row >= 11 && row <= 18 && col == 17) ||
                    (row >= 14 && row <= 18 && col == 1) ||
                    (row >= 14 && row <= 18 && col == 16) ||
                    (row >= 17 && row <= 18 && col == 2) ||
                    (row >= 17 && row <= 18 && col == 15)
                )
                {
                    canvas.SetPixel(col, row, Color.Black);
                }
                else
                {
                    canvas.SetPixel(col, row, Color.DodgerBlue3);
                }
            }
        }
    }

    public void DrawLargeCanvas(Canvas canvas, int schedule_id)
    {
        for (var row = 0; row < canvas.Height; row++)
        {
            for (var col = 0; col < canvas.Width; col++)
            {
                using (DataBaseConnection db = new DataBaseConnection())
                {
                    var chair = db.Chair.FirstOrDefault(c => c.Row == row && c.Column == col && c.CinemaHallID == 3);
                    if (chair != null)
                    {
                        var ticket = db.Ticket.FirstOrDefault(t => t.Schedule_ID == schedule_id && t.Chair_ID == chair.ID);
                        if (ticket != null)
                        {
                            canvas.SetPixel(col, row, Color.Grey); // Chair is not available, color it gray
                            continue; // Skip to the next iteration
                        }
                    }
                }
                if 
                (
                    (row >= 4 && row <= 12 && col >= 13 && col <= 16) ||
                    (row >= 5 && row <= 11 && col >= 12 && col <= 17) ||
                    (row >= 6 && row <= 11 && col >= 11 && col <= 18)
                )
                {
                    canvas.SetPixel(col, row, Color.Red3);
                }
                else if
                (
                    (row >= 1 && row <= 16 && col >= 12 && col <= 17) ||
                    (row >= 1 && row <= 15 && col >= 9 && col <= 20) ||
                    (row >= 2 && row <= 13 && col >= 8 && col <= 21) ||
                    (row >= 4 && row <= 11 && col >= 7 && col <= 22) ||
                    (row >= 6 && row <= 10 && col >= 6 && col <= 23) ||
                    (row >= 8 && row <= 9 && col >= 5 && col <= 24)
                )
                {
                    canvas.SetPixel(col, row, Color.DarkOrange3_1);
                }
                else if
                (
                    (row >= 0 && row <= 6 && col == 0) ||
                    (row >= 0 && row <= 5 && col == 1) ||
                    (row >= 0 && row <= 4 && col == 2) ||
                    (row == 0 && col == 3) ||
                    (row >= 0 && row <= 6 && col == 29) ||
                    (row >= 0 && row <= 5 && col == 28) ||
                    (row >= 0 && row <= 4 && col == 27) ||
                    (row == 0 && col == 26) ||
                    (row >= 12 && row <= 19 && col == 0) ||
                    (row >= 13 && row <= 19 && col == 1) ||
                    (row >= 15 && row <= 19 && col == 2) ||
                    (row >= 17 && row <= 19 && col >= 3 && col <= 4) ||
                    (row >= 18 && row <= 19 && col >= 5 && col <= 6) ||
                    (row == 19 && col == 7) ||
                    (row >= 12 && row <= 19 && col == 29) ||
                    (row >= 13 && row <= 19 && col == 28) ||
                    (row >= 15 && row <= 19 && col == 27) ||
                    (row >= 17 && row <= 19 && col >= 25 && col <= 26) ||
                    (row >= 18 && row <= 19 && col >= 23 && col <= 24) ||
                    (row == 19 && col == 22)
                )
                {
                    canvas.SetPixel(col, row, Color.Black);
                }
                else
                {
                    canvas.SetPixel(col, row, Color.DodgerBlue3);
                }
            }
        }
    }

    public void Draw(int schedule_id, string size, int width, int height, List<Tuple<int, int>> selectedChairs)
    {
        // Clear the console
        // Console.Clear();
        // Set the cursor position to the top left
        Console.SetCursorPosition(0, 0);
        // Create a new canvas
        var canvas = new Canvas(width, height);

        // Draw the cinema hall based on its size
        switch (size)
        {
            case "Small":
                DrawSmallCanvas(canvas, schedule_id);
                break;
            case "Medium":
                DrawMediumCanvas(canvas, schedule_id);
                break;
            case "Large":
                DrawLargeCanvas(canvas, schedule_id);
                break;
            default:
                throw new ArgumentOutOfRangeException("size");
        }
        // Render the cursor position
        canvas.SetPixel(cursorX, cursorY, 'X'); // For example, 'X' represents the cursor position
        // Render the selected chairs
        foreach (var chair in selectedChairs)
        {
            canvas.SetPixel(chair.Item2, chair.Item1, Color.Green); // Note: chair.Item2 is column (x), chair.Item1 is row (y)
        }
        // Render the canvas
        AnsiConsole.Write(canvas);
        // Update the console cursor position to below the canvas
        Console.SetCursorPosition(0, height + 1);
        // Explain what each color means
        AnsiConsole.Markup("[Grey]Grijs: stoel is verkocht[/]\n");
        AnsiConsole.Markup("[Red3]Rood: Love seats[/]\n");
        AnsiConsole.Markup("[DarkOrange3_1]Oranje: Extra Beenruimte[/]\n");
        AnsiConsole.Markup("[DodgerBlue3]Blauw: Standaard[/]\n");
        AnsiConsole.Markup("[Green]Groen: Selected chairs[/]\n");
    }

    public void SetPixel(int x, int y, ConsoleColor color)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            // Set the character and color for the specified pixel
            canvas[x, y] = new CanvasPixel { Character = '■', Color = color };
        }
    }

    public void MoveCursor(int dx, int dy)
    {
        int newX = cursorX + dx;
        int newY = cursorY + dy;

        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
        {
            cursorX = newX;
            cursorY = newY;
        }
    }
}