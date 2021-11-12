using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 皮肤路径
    /// </summary>
    protected string skinPath;

    /// <summary>
    /// 皮肤
    /// </summary>
    public GameObject skin;

    /// <summary>
    /// 显示层级
    /// </summary>
    public PanelManager.Layer layer = PanelManager.Layer.bottom;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        //皮肤
        skinPath = "UI/" + this.GetType().ToString();
        GameObject skinPrefab = Resources.Load<GameObject>(skinPath);
        skin = GameObject.Instantiate(skinPrefab);
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    public void Close()
    {
        string name = this.GetType().ToString();
        PanelManager.Close(name);
    }

    //查找组件
    public T Find<T>(string path)
    {
        return skin.transform.Find(path).GetComponent<T>();
    }

    //初始化时
    public virtual void OnInit() { }

    //显示时
    public virtual void OnShow(params object[] args) { }

    //关闭时
    public virtual void OnClose() { }
}
