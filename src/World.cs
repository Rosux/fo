public class World
{
    public List<Location> Locations; // holds the Town/Castle/Mountain/Farm
    public Location CurrentLocation;
    public SubLocation? CurrentSubLocation;

    public World(List<Location> Locations)
    {
        this.Locations = Locations;
    }

    public bool TravelToLocation(Location destination)
    {
        if (Locations.Contains(destination) && !destination.Locked)
        {
            this.CurrentLocation = destination;
            this.CurrentSubLocation = null;
            return true;
        }
        return false;
    }

    public void TravelToSubLocation(SubLocation destination)
    {
        this.CurrentSubLocation = destination;
    }
    
    public void PrintAllLocations()
    {
        foreach (Location location in Locations)
        {
            Console.WriteLine(location);
        }
    }

    public int GetLongestLocation()
    {
        int longest = -1;
        for (int i = 0; i < this.Locations.Count; i++)
        {
            if (this.Locations[i].Name.Length > longest)
            {
                longest = this.Locations[i].Name.Length;
            }
        }
        return longest;
    }

    public int GetLongestSubLocation(Location location)
    {
        int longest = -1;
        for (int i = 0; i < location.SubLocations.Count; i++)
        {
            if (location.SubLocations[i].Name.Length > longest)
            {
                longest = location.SubLocations[i].Name.Length;
            }
        }
        return longest;
    }
}