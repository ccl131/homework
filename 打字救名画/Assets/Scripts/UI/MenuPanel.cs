using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    public override void OnInit()
    {
        layer = PanelManager.Layer.bottom;
    }

    public override void OnShow(params object[] args)
    {
        MusicManager.Instance.PlayMusic("Audio/BGM");

        Find<Text>("scoreTxt").text = Player.score.ToString();

        Find<Button>("startGameBtn").onClick.AddListener(() =>
        {
            MusicManager.Instance.PlaySound("Audio/按钮点击");
            PanelManager.Open<SelectLevelPanel>(); Close();
        });

        Find<Button>("clearData").onClick.AddListener(() =>
        {
            MusicManager.Instance.PlaySound("Audio/按钮点击");
            if(DBManager.ClearData())
            {
                Find<Text>("scoreTxt").text = Player.score.ToString();
            }
        });

        Find<Button>("quitBtn").onClick.AddListener(() =>
        {
            MusicManager.Instance.PlaySound("Audio/按钮点击");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
