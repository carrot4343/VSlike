using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Cell
{
    //그리드 셀에 소속된 오브젝트 리스트셋
    public HashSet<GameObject> Objects { get; } = new HashSet<GameObject>();
}
public class GridController : BaseController
{
    Grid m_grid;

    Dictionary<Vector3Int, Cell> m_cells = new Dictionary<Vector3Int, Cell>();

    public override bool Init()
    {
        base.Init();

        m_grid = gameObject.GetOrAddcompnent<Grid>();
        return true;
    }

    public void Add(GameObject go)
    {
        //오브젝트의 위치를 좌표(그리드)로 변환
        Vector3Int cellPos = m_grid.WorldToCell(go.transform.position);

        //위에서 얻은 좌표에 해당하는 그리드 셀
        Cell cell = GetCell(cellPos);
        if(cell == null)
        {
            return;
        }
        //그리드 셀의 오브젝트 리스트에 go 추가
        cell.Objects.Add(go);
    }

    public void Remove(GameObject go)
    {
        Vector3Int cellPos = m_grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
        {
            return;
        }
        //add와 동일하나 셀의 오브젝트 리스트에서 add인지 remove 인지만 다름.
        cell.Objects.Remove(go);
    }

    Cell GetCell(Vector3Int cellPos)
    {
        Cell cell = null;
        if (m_cells.TryGetValue(cellPos, out cell) == false)
        {
            cell = new Cell();
            m_cells.Add(cellPos, cell);
        }

        return cell;
    }

    public List<GameObject> GatherObjects(Vector3 pos, float range)
    {
        List<GameObject> objects = new List<GameObject>();

        //pos 주변 range 만큼의 범위에 속하는 grid 계산
        Vector3Int left = m_grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = m_grid.WorldToCell(pos + new Vector3(+range, 0));
        Vector3Int bottom = m_grid.WorldToCell(pos + new Vector3(0, -range));
        Vector3Int top = m_grid.WorldToCell(pos + new Vector3(0, +range));

        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;

        //위에서 구한 범위에 포함되는 오브젝트 탐색
        for(int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                if (m_cells.ContainsKey(new Vector3Int(x, y, 0)) == false)
                    continue;

                //x,y에 해당하는 그리드 셀의 오브젝트들을 objects 리스트에 모두 추가함.
                objects.AddRange(m_cells[new Vector3Int(x, y, 0)].Objects);
            }
        }

        return objects;
    }
}
