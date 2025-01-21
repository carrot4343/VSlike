using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class UI_MonsterInfoItem : UI_Base
{
    #region UI 기능 리스트
    // 정보 갱신
    // MonsterImage : 몬스터 이미지
    // MonsterLevelValueText : 몬스터 레벨 ( "Lv." + 레벨)

    // 추후 후광 효과같은 그래픽 강조 포인트가 추가될 수 있음

    #endregion

    #region Enum
     enum Buttons
    {
        MonsterInfoButton
    }
    enum Texts
    {
        MonsterLevelValueText,

    }

    enum Images
    {
        MonsterImage,
    }
    #endregion

    MonsterData m_monster;
    Transform _makeSubItemParents;
    int m_level;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        //GetButton((int)Buttons.MonsterInfoButton).gameObject.BindEvent(OnClickMonsterInfoButton);

        #endregion

        return true;
    }
    public void SetInfo(int monsterId, Transform makeSubItemParents, int level = 1)
    {
        _makeSubItemParents = makeSubItemParents;
        transform.localScale = Vector3.one;

        if (Managers._Data.MonsterDic.TryGetValue(monsterId, out m_monster))
        {
            m_monster = Managers._Data.MonsterDic[monsterId];
            m_level = level;
        }
        

        Refresh();
    }

    void Refresh()
    {
        if (m_init == false)
            return;
        if (m_monster == null)
        {
            gameObject.SetActive(false);
            return;
        }

        GetText((int)Texts.MonsterLevelValueText).text = $"Lv. {m_level}";
        GetImage((int)Images.MonsterImage).sprite = Managers._Resource.Load<Sprite>(m_monster.sprite);

    }

    // 툴팁 호출
    /*void OnClickMonsterInfoButton()
    {
        //Managers.Sound.PlayButtonClick();
        // UI_ToolTipItem 프리팹 생성
        UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
        item.transform.localScale = Vector3.one;
        RectTransform targetPos = this.gameObject.GetComponent<RectTransform>();
        RectTransform parentsCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
        item.SetInfo(_creature, targetPos, parentsCanvas);
        item.transform.SetAsLastSibling();
    }*/

}
