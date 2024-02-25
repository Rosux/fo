public class Stats
{
    public int CurrentHealth; // characters current health
    public int MaxHealth; // characters current health
    public int Attack; // your skill in attacking
    public int Defence; // your skill in defending
    public int Gold; // characters current gold

    public Stats(int MaxHealth = 0, int Attack = 0, int Defence = 0, int Gold = 0)
    {
        this.MaxHealth = MaxHealth;
        this.CurrentHealth = MaxHealth;
        this.Attack = Attack;
        this.Defence = Defence;
        this.Gold = Gold;
    }

    public bool Pay(int Amount)
    {
        if (Gold - Amount < 0)
        {
            return false;
        }
        else
        {
            Gold = Gold - Amount;
            return true;
        }
    }
    public void AddGold(int Amount)
    {
        Gold = Gold + Amount;
    }

    public bool Damage(int DMG)
    {
        CurrentHealth = CurrentHealth - DMG;
        if (CurrentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void Heal(int Amount)
    {
        CurrentHealth = CurrentHealth + Amount;
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}