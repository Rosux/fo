public class World
{
    public List<Location> Locations; // holds the Town/Castle/Mountain/Farm
    public Location CurrentLocation;
    public World(List<Location> Locations)
    {
        this.Locations = Locations;
    }
    public void Travel(Location destination, Stats playerStats)
    {
        if (Locations.Contains(destination))
        {
            int travelCost = destination.TravelPrice;

            if (playerStats.Gold >= travelCost)
            {
                playerStats.Gold -= travelCost;
                CurrentLocation = destination;
                Console.WriteLine($"Traveled to {destination.Name} for {travelCost} gold.");
            }
            else
            {
                Console.WriteLine("Not enough gold to travel.");
            }
        }
        else
        {
            Console.WriteLine("Invalid destination.");
        }
    }
    
    public void PrintAllLocations()
    {
        foreach (Location location in Locations)
        {
            Console.WriteLine(location);
        }
    }
}