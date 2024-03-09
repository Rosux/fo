public class World
{
    public List<Location> Locations; // holds the Town/Castle/Mountain/Farm
    public Location CurrentLocation;
    public SubLocation CurrentSubLocation;

    public World(List<Location> Locations)
    {
        this.Locations = Locations;
    }

    public bool Travel(Location Ldestination, SubLocation Sdestination)
    {
        if (Locations.Contains(Ldestination) && !Ldestination.Locked && Ldestination.SubLocations.Contains(Sdestination))
        {
            this.CurrentLocation = Ldestination;
            // run callback on sub-location if there is one before and after switching (to do the exit and enter calls)
            if (this.CurrentSubLocation.ExitCallback != null)
            {
                this.CurrentSubLocation.ExitCallback(this.CurrentSubLocation);
            }
            this.CurrentSubLocation = Sdestination;
            if (this.CurrentSubLocation.EnterCallback != null)
            {
                this.CurrentSubLocation.EnterCallback(this.CurrentSubLocation);
            }
            return true;
        }
        return false;
    }
    
    public bool Travel(string Sdestination)
    {
        for (int i = 0; i < this.Locations.Count; i++)
        {
            Location loco = this.Locations[i];
            for (int j = 0; j < loco.SubLocations.Count; j++)
            {
                SubLocation subway = loco.SubLocations[j];
                if (Sdestination == subway.Name)
                {
                    return Travel(loco, subway);
                }
            }
        }
        return false;
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