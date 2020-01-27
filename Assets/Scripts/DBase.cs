using UnityEngine;
using Mono.Data.Sqlite;
using System;
using UnityEngine.UI;

public class DBase : MonoBehaviour
{
    SqliteConnection sqlite_conn;
    public Text dbname;
    string table;
    void Start()
    {
        /*try
        {
            OpenDB();
        }
        catch (Exception e1)
        {
            Debug.LogWarning("open error!!!"+e1.Message);
        }*/
        
    }
    
   
    void OpenDB()
    {
        sqlite_conn = new SqliteConnection("URI=file:" + Application.dataPath + "/test.s3db.db");
        sqlite_conn.Open();

        SqliteCommand sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.ExecuteNonQuery();
    }

    void WriteDB(byte i, byte snake,byte ladder )
    {
        byte s, l;
        SqliteCommand sqlite_cmd = sqlite_conn.CreateCommand();

        if(ReadDB(i, out s, out l))
        {
            sqlite_cmd.CommandText = "DELETE FROM ";
        }

        sqlite_cmd.CommandText = "INSERT INTO b1 (id, snake, ladder) " +
            "VALUES (" + i.ToString() + "," + snake.ToString() + "," + ladder.ToString() + ");";
        sqlite_cmd.ExecuteNonQuery();
    }

    bool ReadDB(byte i,out byte snake,out byte ladder )
    {
        
        SqliteCommand sqlite_cmd;             
        SqliteDataReader sqlite_datareader; 

        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM b1 WHERE id="+i+";";
        sqlite_datareader = sqlite_cmd.ExecuteReader();
        if (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
        {           
            byte snakeReader = sqlite_datareader.GetByte(1);
            byte ladderReader = sqlite_datareader.GetByte(2);            
            snake = snakeReader;
            ladder = ladderReader;
            return true;
        }
        
        snake = 101;
        ladder = 101;
        return false;
    }
    
    public void LoadDB()
    {
        if(dbname.text=="")
        {
            Debug.Log("empty dbname");
            return;
        }

        OpenDB();
        try
        {
            //check if db is available
        }
    }

    public void SaveDB()
    {
        if (dbname.text == "")
        {
            Debug.Log("empty dbname");
            return;
        }

    }
}
