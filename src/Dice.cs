public class Dice
{
    private static Random random = new Random();
    
    public int Roll()
    {
        return random.Next(1, 21); 
    }
}