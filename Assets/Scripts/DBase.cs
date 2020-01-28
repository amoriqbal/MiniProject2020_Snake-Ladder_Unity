using UnityEngine;
using Mono.Data.Sqlite;
using System;
using UnityEngine.UI;

public class DBase : MonoBehaviour
{
    SqliteConnection sqlite_conn;
    public Text dbname;
    void Start()
    {
        try
        {
            OpenDB();
        }
        catch(Exception e)
        {
            Debug.Log("open err!! " + e.Message);
        }
    }
    
   
    void OpenDB()
    {
        sqlite_conn = new SqliteConnection("Data Source=" + Application.dataPath + "/test.s3db.db;");
        sqlite_conn.Open();
    }

    bool isTablePresent(string table)
    {
        SqliteCommand sqlite_cmd = sqlite_conn.CreateCommand();
        SqliteDataReader sdr;
        sqlite_cmd.CommandText = "select name from sqlite_master where type='table' and name='" + table + "';";
        try
        {
            sdr = sqlite_cmd.ExecuteReader();
            
        }
        catch(Exception e)
        {
            Debug.Log("is table present exception!!!!"+e.Message);
            sdr = null;
        }
        if (sdr.HasRows)
        {
            return true;
        }

        return false;
    }
    void WriteDB(string table,int i, int snl )
    {
        int s;
        SqliteCommand sqlite_cmd = sqlite_conn.CreateCommand();
        //SqliteDataReader sdr;
        if(!isTablePresent(table))
        {
            sqlite_cmd.CommandText = "create table " + table + " (id int primary key, snl int);";
            try
            {
                sqlite_cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Debug.Log("create err!!! " + e.Message);
            }
        }
        if(ReadDB(table,i, out s))
        {
            sqlite_cmd.CommandText = "delete * from "+table +" where id="+i+";";

            sqlite_cmd.ExecuteNonQuery();
        }

        sqlite_cmd.CommandText = "INSERT INTO " + table + " (id, snl) " +
            "VALUES (" + i.ToString() + "," + snl.ToString() + ");";
        sqlite_cmd.ExecuteNonQuery();
    }

    bool ReadDB(string table, int i,out int snl )
    {
        
        SqliteCommand sqlite_cmd=sqlite_conn.CreateCommand();             
        SqliteDataReader sqlite_datareader;
        if (!isTablePresent(table))
        {
            snl = BoardWaypoints._NUM_BOXES + 1;
            return false;
        }
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM "+table+" WHERE id="+i+";";
        sqlite_datareader = sqlite_cmd.ExecuteReader();
        if (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
        {           
            int snlReader = sqlite_datareader.GetInt32(1);
            //int ladderReader = sqlite_datareader.GetInt32(2);            
            snl = snlReader;
            //ladder = ladderReader;
            return true;
        }
        
        snl = BoardWaypoints._NUM_BOXES+1;
        //ladder = BoardWaypoints._NUM_BOXES+1;
        return false;
    }
    
    public void LoadDB(string table)
    {
        if(dbname.text=="")
        {
            Debug.Log("empty dbname");
            return;
        }
        if(!isTablePresent(table))
        {
            //error pop up
            Debug.Log("table not found");
            return;
        }

        SqliteCommand cmd = sqlite_conn.CreateCommand();
        SqliteDataReader sdr;

        cmd.CommandText = "select id,snl from '" + table + "';";
        sdr = cmd.ExecuteReader();
        while (sdr.Read())
        {
            int index, snl_data;
            index = sdr.GetInt32(0);
            snl_data = sdr.GetInt32(1);
            if (index > BoardWaypoints._NUM_BOXES - 1)
            {
                Debug.Log("databse has more rows than boardwaypoints");
                break;
            }
            if (snl_data > BoardWaypoints._NUM_BOXES - 1)
            {
                Debug.Log("snl data=" + snl_data.ToString());
                break;
            }
            BoardWaypoints.Instance.snl[index] = snl_data;
            if (snl_data != -1)
            {
                if (index > snl_data)
                {

                    BoardWaypoints.Instance.sprites[index] = TouchBoxController.Instance.DrawSnake(BoardWaypoints.Instance.waypoints[index],
                        BoardWaypoints.Instance.waypoints[snl_data]);
                }
                else
                {
                    BoardWaypoints.Instance.sprites[index] = TouchBoxController.Instance.DrawLadder(BoardWaypoints.Instance.waypoints[index],
                        BoardWaypoints.Instance.waypoints[snl_data]);
                }
            }
        }
    }

    public void LoadDB(Text table)
    {
        LoadDB(table.text);
    }

    public void SaveDB(string table)
    {
        if (dbname.text == "")
        {
            Debug.Log("empty dbname");
            return;
        }
        SqliteCommand cmd = sqlite_conn.CreateCommand();
        CreateEmptyTable(table);
        cmd.CommandText = "insert into " + table + "(id,snl) values ";
        for(int i=0;i<BoardWaypoints._NUM_BOXES;i++)
        {
            cmd.CommandText += "(" + i.ToString() + "," 
                + BoardWaypoints.Instance.snl[i].ToString() + ")" 
                + (i == BoardWaypoints._NUM_BOXES - 1 ? ";" : ",");
        }
        cmd.ExecuteNonQuery();
    }

    public void SaveDB(Text table)
    {
        SaveDB(table.text);
    }

    public void CreateEmptyTable(string name)
    {
        if(name=="")
        {
            Debug.Log("name of table is empty");
            return;
        }

        SqliteCommand cmd = sqlite_conn.CreateCommand();
        if(isTablePresent(name))
        {
            cmd.CommandText = "drop table " + name +";";
            cmd.ExecuteNonQuery();
        }
        
        
        cmd.CommandText = "create table " + name + " (id int primary key,snl int)";
        
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            Debug.Log("trunc/create prob!!!" + e.Message);
        }
    }

    ~DBase()
    {
        sqlite_conn.Close();
    }
}
