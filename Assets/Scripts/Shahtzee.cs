using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


//
public enum ScoreTypes
{
    Aces,
    Twos,
    Threes,
    Fours,
    Fives,
    Sixes,
    ThreeOfAKind,
    FourOfAKind,
    FullHouse,
    SmallStraight,
    LargeStraight,
    Chance,
    Shahtzee
}


//
public class Shahtzee
{
    public int completedCards;

    // TODO/improve: maybe we should store each score in the category instead of a bool?
    //               this probably would be too much for a jam game, but maybe in a full version
    //               we could store each completed card and show the player after the game or something
    public bool[] completedCategories;

    public int cardBonus = 50;


    public Shahtzee()
    {
        completedCards = 0;
        completedCategories = new bool[Enum.GetNames(typeof(ScoreTypes)).Length];
    }

    public void Reset()
    {
        completedCards = 0;
        completedCategories = new bool[Enum.GetNames(typeof(ScoreTypes)).Length];
    }

    public void NewCard()
    {
        completedCards++;
        completedCategories = new bool[Enum.GetNames(typeof(ScoreTypes)).Length];
    }

    public bool IsCurrentCardComplete()
    {
        foreach(bool b in completedCategories)
        {
            if(!b) return false;
        }
        return true;
    }

    public int Bonus()
    {
        return completedCards * cardBonus;
    }

    private int AddAll(List<int> dice)
    {
        int score = 0;

        foreach(int d in dice)
            score += d;

        return score;
    }

    private int AddAllThatMatchNum(List<int> dice, int num)
    {
        int score = 0;

        foreach(int d in dice)
            score += (d == num) ? 1 : 0;

        return score;
    }

    private bool CheckNumOfAKind(List<int> dice, int num)
    {
        foreach(int d in dice)
        {
            var list = dice.FindAll(x => x == d);
            if(list.Count == num)
                return true;
        }

        return false;
    }

    // add up all ones
    public int Aces(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Aces] = true;
        return AddAllThatMatchNum(dice, 1) * 1;
    }

    // add up all twos
    public int Twos(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Twos] = true;
        return AddAllThatMatchNum(dice, 2) * 2;
    }

    // add up all threes
    public int Threes(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Threes] = true;
        return AddAllThatMatchNum(dice, 3) * 3;
    }

    // add up all fours
    public int Fours(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Fours] = true;
        return AddAllThatMatchNum(dice, 4) * 4;
    }

    // add up all fives
    public int Fives(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Fives] = true;
        return AddAllThatMatchNum(dice, 5) * 5;
    }

    // add up all sixes
    public int Sixes(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Sixes] = true;
        return AddAllThatMatchNum(dice, 6) * 6;
    }

    // three matching dice
    // add total of all dice
    public int ThreeOfAKind(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.ThreeOfAKind] = true;

        if(CheckNumOfAKind(dice, 3))
                return AddAll(dice);

        return 0;
    }

    // four matching dice
    // add total of all dice
    public int FourOfAKind(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.FourOfAKind] = true;

        if(CheckNumOfAKind(dice, 4))
                return AddAll(dice);

        return 0;
    }

    // 3 of a kind and 2 of kind
    // score 25
    public int FullHouse(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.FullHouse] = true;

        dice.Sort();

        if(dice[0] == dice[1] && dice[2] == dice[0] && dice[3] != dice[0] && dice[3] == dice[4])
            return 25;

        if(dice[0] == dice[1] && dice[2] != dice[0] && dice[2] == dice[3] && dice[2] == dice[4])
            return 25;

        return 0;
    }

    // sequence of 4
    // score 30
    public int SmallStraight(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.SmallStraight] = true;

        List<int> list = new HashSet<int>(dice).ToList();

        string s = "";

        foreach(int d in list)
            s += d.ToString();

        if(s.Contains("1234") || s.Contains("2345") || s.Contains("3456"))
            return 30;

        return 0;
    }

    // sequence of 5
    // score 40
    public int LargeStraight(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.LargeStraight] = true;

        List<int> list = new HashSet<int>(dice).ToList();

        string s = "";

        foreach(int d in list)
            s += d.ToString();

        if(s.Contains("12345") || s.Contains("23456"))
            return 40;

        return 0;
    }

    // add total of all dice
    public int Chance(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Chance] = true;
        return AddAll(dice);
    }

    // YAHTZEE!!! 5 of a kind
    // score 50
    public int FiveOfAKind(List<int> dice)
    {
        completedCategories[(int)ScoreTypes.Shahtzee] = true;

        if(CheckNumOfAKind(dice, 5))
                return AddAll(dice);

        return 0;
    }

    public bool IsComplete(ScoreTypes s)
    {
        return (completedCategories[(int)s]);
    }
}
