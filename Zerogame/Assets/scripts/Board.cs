﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Board
{
    public int rows, columns;

    public Square[] squares;

    public List<Text> texts = new List<Text>();

    public GameObject[,] boardElements;

    public Line[,] lines;
    
    public string[] players = { "Player", "Ai"};

    public int activePlayer = 0;

    public Text activePlayerText;

    GameObject turnTexts;
    GameObject endTexts;

    public Board(int _rows, int _columns)
    {
        rows = _rows;
        columns = _columns;

        turnTexts = GameObject.Find("Turn Texts");
        endTexts = GameObject.Find("End Texts");

        endTexts.SetActive(false);
    }

    public void InitializePlayerText(Text _text)
    {
        activePlayerText = _text;
        activePlayerText.text = players[0];
    }
    
    public void UpdateColours(Line move)
    {
        bool anyClosedSquare = false;
        List<Square> squaresClosed = new List<Square>();

        foreach (Square square in move.ParentSquares)
        {
            if (square.IsClosedSquare())
            {
                anyClosedSquare = true;
                squaresClosed.Add(square);
                texts[square.Index].text = players[activePlayer];
            }
        }

        if (anyClosedSquare)
            foreach (Square square in squaresClosed)
            {
                square.SetClosedColor(Color.gray);
            }
        else
            move.SetColor(Color.red);

    }

    public void FindSquares()
    {
        int squareIndex = 0;

        for (int row = 1; row < lines.GetLength(0); row += 2)
        {
            for (int column = 0; column < lines.GetLength(1) - 1; column++)
            {
                if (lines[row, column] != null)
                {
                    squares[squareIndex].SetLine("W", lines[row, column], squares[squareIndex]);
                    squares[squareIndex].SetLine("N", lines[row - 1, column], squares[squareIndex]);
                    squares[squareIndex].SetLine("S", lines[row + 1, column], squares[squareIndex]);
                    squares[squareIndex].SetLine("E", lines[row, column + 1], squares[squareIndex]);

                    ++squareIndex;
                }
            }

        }
    }

    /// <summary>
    /// Devuelve el siguiente jugador al que tocará jugar
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    public int NextPlayer()
    {
        if (activePlayer >= players.Length - 1)
            activePlayer = 0;
        else
            ++activePlayer;

        activePlayerText.text = players[activePlayer];

        return activePlayer;        
    }

    /// <summary>
    /// Comprueba si están todas las casillas marcadas
    /// </summary>
    /// <returns></returns>
    public bool IsEndOfGame()
    {
        for (int i = 0; i < squares.Length; i++)
        {
            if (!squares[i].IsClosedSquare())
                return false;
        }

        return true;
    }

    /// <summary>
    /// Devuelve un valor del tablero
    /// </summary>
    /// <returns></returns>
    public Line Evaluate(int activePlayer)
    {
        /*
         * O O O
         * O O O
         * O O O
         * */
         
        // Todas las líneas empiezan con un valor y se va cambiando el valor de las
        // casillas contiguas a la línea pulsada

        /*if (lines[row, column] != null)
        {
            if (row == 0)
            {

            }
            else if (row == lines.GetLength(0) - 1)
            {

            }
            if (column == 0)
            {

            }
            else if (column == lines.GetLength(1) - 1)
            {

            }
        }*/



        throw new NotImplementedException();
    }

    /// <summary>
    /// Todas los posibles movimientos de un estado del tablero
    /// </summary>
    /// <returns></returns>
    public List<Line> PossibleMoves()
    {
        List<Line> availableLines = new List<Line>();

        for (int i = 0; i < squares.Length; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (!squares[i].GetLine(j).IsPressed)
                    availableLines.Add(squares[i].GetLine(j));
            }
        }

        return availableLines;
    }

    /// <summary>
    /// Crea un nuevo tablero a partir de un movimiento
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public Board GenerateNewBoardFromMove(Line move)
    {
        Board newBoard = DuplicateBoard();
       // newBoard.lines[move.row, move.column].state = Line.State.pressed;

        if(!GameController.Instance.IsSquare(move))
            newBoard.activePlayer = NextPlayer();

        return newBoard;
    }

    private Board DuplicateBoard()
    {
        Board newBoard = new Board(rows, columns);
        for (int i = 0; i < squares.Length; i++)
        {
            newBoard.squares[i] = this.squares[i];
        }
        newBoard.activePlayer = this.activePlayer;

        return newBoard;
    }

    public void FinishGame()
    {
        turnTexts.SetActive(false);
        
        RectTransform endTextRectTr = endTexts.GetComponent<RectTransform>();

        Text scoreText = endTextRectTr.GetChild(0).GetComponent<Text>();
        Text winnerText = endTextRectTr.GetChild(1).GetComponent<Text>();
        FinalScore(scoreText, winnerText);

        endTexts.SetActive(true);
    }

    void FinalScore(Text score, Text winner)
    {
        string separator = "   -   ";
        int[] count = new int[players.Length]; // Número de jugadores
        int bestScore = -1;
        int bestIndex = -1;

        // Conteo
        for (int i = 0; i < texts.Count; i++)
        {
            for (int j = 0; j < players.Length; j++)
            {
                if (texts[i].text.Equals(players[j]))
                    ++count[j];

                if (count[j] > bestScore)
                {
                    bestScore = count[j];
                    bestIndex = j;
                }
            }
        }

        // Actualizo textos de final de juego

        winner.text = players[bestIndex] + " wins!";

        score.text = "";
        for (int i = 0; i < count.Length; i++)
        {
            score.text += players[i] + ": " + count[i];
            if (i != count.Length - 1)
                score.text += separator;


        }
    }
}
