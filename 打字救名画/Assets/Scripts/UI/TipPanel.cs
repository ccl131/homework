using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public override void OnInit()
    {
        layer = PanelManager.Layer.top;
    }

    public override void OnShow(params object[] args)
    {
        Find<Text>("tip/tipText").text = (string)args[0];
        Find<Button>("tip/returnBtn").onClick.AddListener(() => { MusicManager.Instance.PlaySound("Audio/按钮点击"); Close(); });
    }
}
