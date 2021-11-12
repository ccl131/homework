using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public interface IEventInfo
{
    //这是一个空接口
}

public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class Eventcenter
{
    private static Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听（无参）
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">回调方法</param>
    public static void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }

    /// <summary>
    /// 移除对应的事件监听(无参)
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">回调方法</param>
    public static void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }

    /// <summary>
    /// 通过事件名字进行事件触发(无参)
    /// </summary>
    /// <param name="name">事件名字</param>
    public static void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            Debug.Log("Fire:<color=red>" + name+"</color>");
            (eventDic[name] as EventInfo).actions?.Invoke();
        }
    }

    /// <summary>
    /// 添加事件监听<T>
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    /// <param name="name">事件名字</param>
    /// <param name="action">回调方法</param>
    public static void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //没有的情况
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 移除对应的事件监听
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    /// <param name="name">事件名字</param>
    /// <param name="action">回调方法</param>
    public static void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }

    /// <summary>
    /// 通过事件名字进行事件触发
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    /// <param name="name">事件名字</param>
    /// <param name="info">参数</param>
    public static void EventTrigger<T>(string name, T info)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions?.Invoke(info);
            Debug.Log("Fire:<color=red>" + name+"</color><color=blue>/</color><color=red>"+info+"</color>");
        }
    }

    /// <summary>
    /// 清空所有事件监听(主要用在切换场景时)
    /// </summary>
    public static void Clear()
    {
        eventDic.Clear();
    }
}


