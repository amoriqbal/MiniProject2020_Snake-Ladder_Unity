using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MatchDriver : MonoBehaviour
{
    public PieceController[] players;//references to the player pieces Controller components
    private string[] pnames = { "RED", "BLUE", "YELLOW", "GREEN" };
    public int turn;//Which Player's turn is it now
    public int dieVal;//value of dice throw. always bw 1 and 6
    public Text DieValText;
    public Text DieThrowTurn;
    public Text WinText;
    public GameObject winCanvas;
    public GameObject matchCanvas;
    System.Random random;
    
    
    void Start()
    {
        turn = 0;
        dieVal = 1;
        DieValText.text = dieVal.ToString();
        DieThrowTurn.text = "THROW DIE PLAYER " + pnames[turn];
        random = new System.Random();
    }

    public void TakeTurn()
    {
        ThrowDie();
        players[turn].Transit(dieVal);
        if (players[turn].pos == 99)
            WinGame();
        else
        {
            turn = (turn + 1) % players.Length;
            DieThrowTurn.text = "THROW DIE PLAYER " + pnames[turn];
        }
    }
    
    public void ThrowDie()
    {
        dieVal = random.Next(1, 6);
        DieValText.text = dieVal.ToString();
        
        //return dieVal;
    }

    public void WinGame()
    {
        winCanvas.SetActive(true);
        matchCanvas.SetActive(false);
        WinText.text = "PLAYER " + pnames[turn] + " HAS WON THIS MATCH!";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
