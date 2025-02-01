namespace GameBrain;

public record struct GameGrid
{
    public int GridLength { get; init; }
    public int GridWidth { get; init; }
    public int GridStartX { get; set; }
    public int GridStartY { get; set; }
}