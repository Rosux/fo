public class Quest
{
    public string QuestName { get; set; }
    public string QuestDescription { get; set; }
    public string QuestTask { get; set; }
    public bool IsCompleted { get; private set; }

    public Quest(string name, string description, string task)
    {
        QuestName = name;
        QuestDescription = description;
        QuestTask = task;
        IsCompleted = false;
    }
    public void Start()
    {
        Console.WriteLine($"Quest started: {QuestName}\n{QuestDescription}");
        PrintTask();
    }

    public void Complete()
    {
        IsCompleted = true;
        Console.WriteLine($"Quest \"{QuestName}\" completed!");
    }

    public void PrintTask()
    {
        Console.WriteLine($"Quest: {QuestTask}");
    }

}
    // holds stuff like questName, questDescription, questTask, completed?
    // has some method to start/complete/fail the quest
    // maybe has some reward thing linked to it
