using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelectPopup : UI_Base
{
    [SerializeField] Transform m_grid;

    List<UI_SkillCardItem> m_items = new List<UI_SkillCardItem>();

    private void Start()
    {
        PopulateGrid();
    }
    void PopulateGrid()
    {
        foreach (Transform t in m_grid.transform)
            Managers._Resource.Destroy(t.gameObject);

        for(int i = 0; i < 3; i++)
        {
            var go = Managers._Resource.Instantiate("UI_SkillCardItem.prefab");
            UI_SkillCardItem item = go.GetOrAddcompnent<UI_SkillCardItem>();

            item.transform.SetParent(m_grid.transform);

            m_items.Add(item);
        }
    }
}
