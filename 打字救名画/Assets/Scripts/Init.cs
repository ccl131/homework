using UnityEngine;

public class Init : MonoBehaviour
{
    private void Start()
    {
        PanelManager.Init();
        if (!DBManager.GetPlayerData())
        {
            Debug.LogError("获取玩家数据失败");
        }
        PanelManager.Open<MenuPanel>();
        DontDestroyOnLoad(this);
    }
}
