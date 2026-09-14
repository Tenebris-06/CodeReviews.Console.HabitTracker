using System.Net;
using Microsoft.Data.Sqlite;

public class Program
{
    public static void Main(string[] args)
    {

        DBHelper.InitializeDB();
        Program p = new Program();
        p.MainMenu();

    }

    public void MainMenu()
    {
        int choice;



        // int.TryParse(Console.ReadLine(), out choice);

        {

            while (true)
            {

                Console.WriteLine("Select an Option:\n");
                Console.WriteLine("""
        [1] Habits
        [2] Occurrences
        """);

                choice = Utilities.ReadInt(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        HabitsMenu();
                        break;

                    case 2:
                        OccurrencesMenu();
                        break;

                    default:
                        Console.WriteLine("Please enter a valid option\n");
                        break;
                }
            }
        }
    }
    public void HabitsMenu()
    {
        int choice;
        bool running = true;
        while (running)
        {
            Console.WriteLine("""
        [1] View Habits
        [2] Add Habits
        [3] Delete Habits
        [0] Return To Main Menu
        """);

            choice = Utilities.ReadInt(Console.ReadLine());


            switch (choice)
            {
                case 1:
                    var habitlist = DBHelper.GetHabits();
                    foreach (Habit habit in habitlist)
                    {
                        Console.WriteLine(habit.id + " --- " + habit.name);
                    }
                    break;

                case 2:
                    var HabitToAdd = Utilities.ReadString("Enter habit name:");
                    DBHelper.InsertHabit(HabitToAdd);
                    break;

                case 3:
                    var HabitToDelete = Utilities.ReadString("Enter habit name:");
                    DBHelper.DeleteHabit(HabitToDelete);
                    break;

                case 0:
                    running = false;
                    break;
            }
            MainMenu();
        }
    }

    public void OccurrencesMenu()
    {
        int choice;


        bool running = true;
        while (running)
        {
            Console.WriteLine("""
        [1] View Today's Occurrences
        [2] Add Occurrence
        [3] Delete Occurrence
        [4] View ALL Occurrences
        [0] Return To Main Menu
        """);


            choice = Utilities.ReadInt(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    List<Occurrence> occurrences1 = DBHelper.GetOccurrences();
                    occurrences1 = occurrences1.Where(o => o.Date.Date == DateTime.Today).ToList();
                    foreach (Occurrence o in occurrences1)
                    {
                        Console.WriteLine(o.HabitName + "---" + o.Date);
                    }
                    break;

                case 2:
                    List<Habit> habits = DBHelper.GetHabits();
                    foreach (Habit h in habits)
                    {
                        Console.WriteLine(h.id + "---" + h.name);
                    }

                    int HabitChoice;
                    while (true)
                    {
                        HabitChoice = Utilities.ReadInt("Enter the ID of the habit that occurred:");
                        if (!(habits.Any(h => h.id == HabitChoice)))
                        {
                            Console.WriteLine("Invalid Option. Please try again:");
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    Console.WriteLine("Enter the date/time this habit occurred: (leave empty for right now)");
                    var OccurrenceDateTime = Utilities.ReadDate(Console.ReadLine());
                    DBHelper.AddOccurrence(HabitChoice, OccurrenceDateTime);
                    break;

                case 3:
                    List<Occurrence> occurrences3 = DBHelper.GetOccurrences();
                    foreach (Occurrence o in occurrences3)
                    {
                        Console.WriteLine(o.id + "---" + o.HabitName + "---" + o.Date);
                    }


                    while (true)
                    {
                        Console.WriteLine("Enter the ID of the occurrence you would like to delete: ");
                        int OccurrenceToDelete = Utilities.ReadInt(Console.ReadLine());

                        if ((occurrences3.Any(o => o.id == OccurrenceToDelete)))
                        {
                            DBHelper.DeleteOccurrence(OccurrenceToDelete);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Option. Please Try Again");
                        }
                    }
                    break;

                case 4:
                    List<Occurrence> occurrences2 = DBHelper.GetOccurrences();
                    foreach (Occurrence o in occurrences2)
                    {
                        Console.WriteLine(o.HabitName + "---" + o.Date);
                    }
                    break;

                case 0:
                    running = false;
                    break;


                default:
                    Console.WriteLine("Invalid Input. Please Try Again:");
                    break;
            }
        }
    }
}