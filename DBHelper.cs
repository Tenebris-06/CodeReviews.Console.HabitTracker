using System.Data;
using Microsoft.Data.Sqlite;

public static class DBHelper {

    public static SqliteConnection InitializeConnection(){
        var connection = new SqliteConnection("Data Source=data.db");
        connection.Open();
        return connection;
    }

    public static void InitializeDB(){
        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
        CREATE TABLE IF NOT EXISTS habits (
        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        name TEXT UNIQUE NOT NULL
        );

        CREATE TABLE IF NOT EXISTS occurrences (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        habit_id INTEGER NOT NULL,
        occurrence_date TEXT,
        FOREIGN KEY (habit_id) REFERENCES habits(id)
        );

        INSERT OR IGNORE INTO habits (name)
        VALUES  ("Study"),
                ("Doomscroll");
        """;
        command.ExecuteNonQuery();
    }

    // >>> HABITS
    public static void InsertHabit(string name){
        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """ 
        INSERT INTO habits (name)
        VALUES  (@name);
        """;
        command.Parameters.AddWithValue("@name", name);
        command.ExecuteNonQuery();
    }

    public static void DeleteHabit(string name){
        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
        DELETE FROM habits WHERE name = @name;
        """;
        command.Parameters.AddWithValue("@name", name);
        command.ExecuteNonQuery();
    }

    public static void UpdateHabit(int id, string name)
    {
        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
        UPDATE habits
        SET name = @name
        WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", name);
        command.ExecuteNonQuery();
    }

    public static List<Habit> GetHabits(){
        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM habits;";

        using var reader = command.ExecuteReader();
        var habits = new List<Habit>();

        while (reader.Read())
        {
            habits.Add(new Habit
            {
                id = reader.GetInt32(0),
                name = reader.GetString(1)
            });
        }
        return habits;
    }

    // >>> OCCURRENCES

    public static void AddOccurrence(int id, DateTime date)
    {
        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
        INSERT INTO occurrences (habit_id, occurrence_date)
        VALUES (@id, @date);
        """;
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@date", date);
        command.ExecuteNonQuery();
    }

    public static List<Occurrence> GetOccurrences()
    {
        List<Occurrence> occurrences = new();

        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
        SELECT 
            occurrences.id,
            habits.name,
            occurrences.occurrence_date
        FROM habits
        JOIN occurrences
        ON habits.id = occurrences.habit_id; 
        """;

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            occurrences.Add(new Occurrence{
                id = reader.GetInt32(0),
                HabitName = reader.GetString(1),
                Date = reader.GetDateTime(2)
            });
        }
        return occurrences;
    }

    public static void DeleteOccurrence(int id)
    {
        using var connection = InitializeConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
        DELETE FROM occurrences WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }
}