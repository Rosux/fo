public class Location
{
    public string Name; // holds the location name like town/castle/mountain/farm
    public List<SubLocation> SubLocations; // holds the sub-locations like bar/fountain/dungeon/cave/river
    public int TravelPrice;
    public Location(string Name, List<SubLocation> SubLocations, int TravelPrice)
    {
        this.Name = Name;
        this.SubLocations = SubLocations;
        this.TravelPrice = TravelPrice;
    } 
    public override string ToString()
    {
        return $"Location: {Name}\nTravelPrice: {TravelPrice}\n";
    }
}