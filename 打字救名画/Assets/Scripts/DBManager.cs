using System;
using UnityEngine;
using System.Collections.Generic;

public class DBManager
{
    /// <summary>
    /// 获取玩家数据
    /// </summary>
    public static bool GetPlayerData()
    {
        try
        {
            if (!PlayerPrefs.HasKey("score"))
            {
                PlayerPrefs.SetInt("score", 0);
            }
            Player.score = PlayerPrefs.GetInt("score");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("[数据]获取玩家信息失败:" + e.ToString());
            return false;
        }
    }

    /// <summary>
    /// 保存角色信息
    /// </summary>
    public static bool UpdatePlayerData()
    {
        try
        {
            PlayerPrefs.SetInt("score", Player.score);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("[数据]更新玩家信息失败:" + e.ToString());
            return false;
        }
    }

    /// <summary>
    /// 清空玩家数据
    /// </summary>
    /// <returns></returns>
    public static bool ClearData()
    {
        try
        {
            PlayerPrefs.SetInt("score", 0);
            Player.score = 0;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
    }
}