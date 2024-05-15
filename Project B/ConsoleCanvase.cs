
using Spectre.Console;

public class ConsoleCanvas
{
    private struct CanvasPixel
    {
        public ConsoleColor Color;
        public char Character;
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
                canvas[x, y] = new CanvasPixel { Color = ConsoleColor.White, Character = ' ' };
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
                    canvas.SetPixel(col, row, Color.Red);
                }
                else if ((row >= 3 && row <= 10 && col <= 4 && col >= 3) ||
                        (row >= 3 && row <= 10 && col <= 8 && col >= 7) ||
                        (row >= 9 && row <= 10 && col >= 5 && col <= 6) ||
                        (row >= 3 && row <= 4 && col >= 5 && col <= 6))
                {
                    canvas.SetPixel(col, row, Color.Orange1);
                }
                else if ((row == 0 && col >= 0 && col <= 1) || (row >= 0 && row <= 2 && col == 0) ||
                        (row == 13 && col >= 0 && col <= 1) || (row >= 11 && row <= 13 && col == 0) ||
                        (row == 0 && col >= 10 && col <= 11) || (row >= 0 && row <= 2 && col == 11) ||
                        (row == 13 && col >= 10 && col <= 11) || (row >= 11 && row <= 13 && col >= 11))
                {
                    canvas.SetPixel(col, row, Color.Black);
                }
                else
                {
                    canvas.SetPixel(col, row, Color.Blue);
                }
            }
        }
    }
    public void DrawMidiumCanvas(Canvas canvas, int schedule_id)
    {
        for (var row = 0; row < canvas.Height; row++)
        {
            for (var col = 0; col < canvas.Width; col++)
            {
                // get db
                // get chair with hall id = 1 and column = col and row = row
                // query tickets table for ticket with schedule id x and chair id from before
                // if any results => unavailable
                // else go through
                if (
                    (row >= 5 && row <= 12  && col >= 8 && col <= 9) || 
                    (row >= 6 && row <= 11 && col >= 7 && col <= 10) || 
                    (row >= 7 && row <= 10 && col >= 6 && col <= 11) )
                {
                    canvas.SetPixel(col, row, Color.Red);
                }
                else if
                (
                    (row >= 1 && row <= 15 && col >= 6 && col <= 11)||
                    (row >= 2 && row <= 13 && col >= 5 && col <= 12)||
                    (row >= 4 && row <= 12 && col >= 4 && col <= 13)||
                    (row >= 6 && row <= 11 && col >= 3 && col <= 14)||
                    (row >= 8 && row <= 10 && col >= 2 && col <= 15)
                )
                {
                    canvas.SetPixel(col, row, Color.Orange3);
                }
                else if 
                (
                    (row >= 0 && row <= 5 && col == 0 )||
                    (row >= 0 && row <= 5 && col == 17)||
                    (row >= 11 && row <= 18 && col == 0)||
                    (row >= 11 && row <= 18 && col == 17)||
                    (row >= 14 && row <= 18 && col == 1)||
                    (row >= 14 && row <= 18 && col == 16)||
                    (row >= 17 && row <= 18 && col == 2)||
                    (row >= 17 && row <= 18 && col == 15)
                )
                {
                    canvas.SetPixel(col, row, Color.Black);
                }
                else
                {
                    canvas.SetPixel(col, row, Color.Blue);
                }
            }
        }
    }

    public void Draw(int schedule_id, string size, int width, int height)
    {
        Console.Clear();
        var canvas = new Canvas(width, height);
        // Draw some shapes
        switch (size)
        {
            case "Small":
                DrawSmallCanvas(canvas, schedule_id);
                break;
            case "Medium":
                DrawMidiumCanvas(canvas, schedule_id);
                break;
            case "Large":
                DrawSmallCanvas(canvas, schedule_id);
                break;
            default:
                throw new ArgumentOutOfRangeException("size");
        }
        canvas.SetPixel(cursorX, cursorY, 'X'); // For example, 'X' represents the cursor position

        // Render the canvas
        AnsiConsole.Write(canvas);
        Console.SetCursorPosition(0, height+1);
    }
    public void SetPixel(int x, int y, char c)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            // Set the character and color for the specified pixel
            canvas[x, y] = new CanvasPixel { Character = c, Color = Color.Black };
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

