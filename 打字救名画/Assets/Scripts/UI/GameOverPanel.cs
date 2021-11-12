using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    private int levelIndex;
    public override void OnInit()
    {
        layer = PanelManager.Layer.middle;
    }

    public override void OnShow(params object[] args)
    {
        MusicManager.Instance.PlayMusic("Audio/失败");
        levelIndex = (int)args[0];
        Find<Text>("levelText").text = levelIndex.ToString();
        Find<Text>("scoreText").text = ((int)args[1]).ToString();
        Find<Button>("resBtn").onClick.AddListener(() =>
        {
            MusicManager.Instance.PlaySound("Audio/按钮点击");
            MusicManager.Instance.PlaySound("Audio/重新开始");
            PanelManager.Open<GamePanel>(levelIndex.ToString());
            Close();
        });
        Find<Button>("returnBtn").onClick.AddListener(() => 
        {
            MusicManager.Instance.PlaySound("Audio/按钮点击");
            PanelManager.Open<SelectLevelPanel>();
            Close();
        });
    }



}
