using System;

[Serializable]
public class SungShin_Choice //대화에서 선택지를 고르기 위함
{
    public string text; //선택지 대사 (ex: 예 / 아니요)
    public int nextID; //선택지를 골랐을 때 어떤 대화번호로 넘어갈지
}