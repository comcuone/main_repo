public enum Speaker //어떤 대사창을 사용할지 결정하기 위함
{
    NPC,
    Player
}

[System.Serializable]
public class Sungshin_Dialogue
{
    public Speaker speaker; //대사를 말하는 대상(NPC 또는 Player)
    public string text; //대사

    public SungShin_Choice[] choices; //이 대사에서 표시할 선택지 목록(선택지가 없는 경우 비워둠)
}