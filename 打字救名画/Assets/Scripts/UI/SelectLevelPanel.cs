using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelPanel : BasePanel
{
    private Transform levelItems;

    public override void OnInit()
    {
        layer = PanelManager.Layer.bottom;
    }

    public override void OnShow(params object[] args)
    {
        MusicManager.Instance.PlayMusic("Audio/选择界面");
        levelItems = Find<Transform>("levelItems");
        Find<Button>("returnBtn").onClick.AddListener(() => 
        { 
            MusicManager.Instance.PlaySound("Audio/按钮点击"); 
            PanelManager.Open<MenuPanel>(); 
            Close(); 
        });

        //关卡数量
        int levelCount = SceneManager.sceneCountInBuildSettings - 2;

        for (int i = 1; i <= levelCount; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("UI/levelItem"));
            obj.name = i.ToString();
            obj.transform.Find("indexText").GetComponent<Text>().text = i.ToString();
            obj.transform.GetComponent<Button>().onClick.AddListener(delegate () { OnLevelBtnClick(obj.name); });
            obj.transform.SetParent(levelItems);
        }
    }
    private void OnLevelBtnClick(string index)
    {
        MusicManager.Instance.PlaySound("Audio/按钮点击");
        PanelManager.Open<GamePanel>(index);
        Close();

    }
}
