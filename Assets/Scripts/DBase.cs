//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

//using System.Data;
using Mono.Data.Sqlite;
using System;

public class DBase : MonoBehaviour
{
    static uint n=0;
    // Start is called before the first frame update
    void Start()
    {
        if (n > 0)
            return;
        n++;

        try
        {
            OpenDB();
        }
        catch (Exception e1)
        {
            Debug.LogWarning("open error!!!"+e1.Message);
        }
        try
        {
            //WriteDB();
        }
        catch(Exception e2)
        {
            Debug.LogWarning("write error!!"+e2.ToString());
        }
        try
        {
            ReadDB();
        }
        catch(Exception e3)
        {
            Debug.LogWarning("read error!!"+e3.ToString());
        }
    }
    SqliteConnection sqlite_conn;
   
    void OpenDB()
    {
        if (n > 1)
            return;
        //string conn = "URI=file:" + Application.dataPath + "/PickAndPlaceDatabase.s3db"; //Path to database.
        sqlite_conn = new SqliteConnection("URI=file:" + Application.dataPath + "/test.s3db.db");

        // open the connection:
        sqlite_conn.Open();

        SqliteCommand sqlite_cmd = sqlite_conn.CreateCommand();

        // Let the SQLiteCommand object know our SQL-Query:
        //sqlite_cmd.CommandText = "CREATE TABLE test (id integer primary key, text varchar(100));";

        // Now lets execute the SQL ;-)
        sqlite_cmd.ExecuteNonQuery();
    }

    void WriteDB()
    {
        if (n > 1)
            return;

        SqliteCommand sqlite_cmd = sqlite_conn.CreateCommand();

        sqlite_cmd.CommandText = "INSERT INTO b1 (id, name) VALUES (1, 'hello hello');";

        sqlite_cmd.ExecuteNonQuery();
    }

    void ReadDB()
    {
        if (n > 1)
            return; // Database Connection Object
        SqliteCommand sqlite_cmd;             // Database Command Object
        SqliteDataReader sqlite_datareader;  // Data Reader Object

        string output="";

        

        sqlite_cmd = sqlite_conn.CreateCommand();

        sqlite_cmd.CommandText = "SELECT * FROM b1";

        sqlite_datareader = sqlite_cmd.ExecuteReader();

        // The SQLiteDataReader allows us to run through each row per loop
        while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
        {
            // Print out the content of the text field:
            // System.Console.WriteLine("DEBUG Output: '" + sqlite_datareader["text"] + "'");

            object idReader = sqlite_datareader.GetValue(0);
            string textReader = sqlite_datareader.GetString(1);

            output += idReader + " '" + textReader + "' " + "\n";
            Debug.Log(output);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
