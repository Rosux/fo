public class Location
{
    public string Name; // holds the location name like town/castle/mountain/farm
    public List<SubLocation> SubLocations; // holds the sub-locations like bar/fountain/dungeon/cave/river
    public int TravelPrice;
    public bool Locked;
    
    public Location(string Name, List<SubLocation> SubLocations, int TravelPrice, bool locked=false)
    {
        this.Name = Name;
        this.SubLocations = SubLocations;
        this.TravelPrice = TravelPrice;
        this.Locked = locked;
    } 
    public override string ToString()
    {
        return $"Location: {Name}\nTravelPrice: {TravelPrice}\n";
    }
}