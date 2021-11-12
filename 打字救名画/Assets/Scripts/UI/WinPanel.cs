using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanel : BasePanel
{
    private int levelIndex;
    public override void OnInit()
    {
        layer = PanelManager.Layer.middle;
    }

    public override void OnShow(params object[] args)
    {
        MusicManager.Instance.PlayMusic("Audio/胜利");
        levelIndex = (int)args[0];
        Find<Text>("levelText").text = levelIndex.ToString();
        Find<Text>("scoreText").text = ((int)args[1]).ToString();
        Find<Button>("nextBtn").onClick.AddListener(()=>
        {
            MusicManager.Instance.PlaySound("Audio/按钮点击");
            if (levelIndex<SceneManager.sceneCountInBuildSettings-2)
            {
                PanelManager.Open<GamePanel>((levelIndex+1).ToString());
            }
            else
            {
                PanelManager.Open<SelectLevelPanel>();
            }
            Close();
        });
    }
}
