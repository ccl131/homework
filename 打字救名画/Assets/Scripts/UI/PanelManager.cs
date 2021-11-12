using System.Collections.Generic;
using UnityEngine;

public class PanelManager
{
    //层级列表
    private static Dictionary<Layer, Transform> layers = new Dictionary<Layer, Transform>();
    //打开的面板列表
    public static Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();
    //结构
    public static Transform root;
    public static Transform canvas;

    //Layer
    public enum Layer
    {
        bottom,
        middle,
        top,
    }

    //初始化
    public static void Init()
    {
        layers.Clear();
        root = GameObject.Find("root").transform;
        canvas = root.Find("Canvas");
        Transform bootom = canvas.Find("bottom");
        Transform middle = canvas.Find("middle");
        Transform top = canvas.Find("top");
        Transform tip = canvas.Find("tip");
        layers.Add(Layer.bottom, bootom);
        layers.Add(Layer.middle, middle);
        layers.Add(Layer.top, top);
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    public static void Open<T>(params object[] para) where T : BasePanel
    {
        //已经打开
        string name = typeof(T).ToString();
        if (panels.ContainsKey(name)) return;
        //组件
        BasePanel panel = root.gameObject.AddComponent<T>();
        panel.OnInit();
        panel.Init();
        //父容器  
        Transform layer = layers[panel.layer];
        panel.skin.transform.SetParent(layer, false);
        //列表
        panels.Add(name, panel);
        //OnShow
        panel.OnShow(para);
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    public static void Close(string name)
    {
        //没有打开
        if (!panels.ContainsKey(name)) return;
        BasePanel panel = panels[name];
        //OnClose
        panel.OnClose();
        //列表
        panels.Remove(name);
        //销毁
        GameObject.Destroy(panel.skin);
        Component.Destroy(panel);
    }
}
