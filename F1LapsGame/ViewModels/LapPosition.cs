namespace F1LapsGame.Models;

public class LapPosition
{
    public int CurrentPosition { get; set; }
    public int DriverId { get; set;}
    public int[] DriverLaptimesInMs { get; set; }
    public int PreviousLapPosition { get; set; }
    public bool WillImprove { get; set; } = false;

    public LapPosition(int numberOfLaps)
    {
        DriverLaptimesInMs = new int[numberOfLaps];
    }
}
