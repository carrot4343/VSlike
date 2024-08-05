using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Cell
{
    //�׸��� ���� �Ҽӵ� ������Ʈ ����Ʈ��
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
        //������Ʈ�� ��ġ�� ��ǥ(�׸���)�� ��ȯ
        Vector3Int cellPos = m_grid.WorldToCell(go.transform.position);

        //������ ���� ��ǥ�� �ش��ϴ� �׸��� ��
        Cell cell = GetCell(cellPos);
        if(cell == null)
        {
            return;
        }
        //�׸��� ���� ������Ʈ ����Ʈ�� go �߰�
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
        //add�� �����ϳ� ���� ������Ʈ ����Ʈ���� add���� remove ������ �ٸ�.
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

        //pos �ֺ� range ��ŭ�� ������ ���ϴ� grid ���
        Vector3Int left = m_grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = m_grid.WorldToCell(pos + new Vector3(+range, 0));
        Vector3Int bottom = m_grid.WorldToCell(pos + new Vector3(0, -range));
        Vector3Int top = m_grid.WorldToCell(pos + new Vector3(0, +range));

        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;

        //������ ���� ������ ���ԵǴ� ������Ʈ Ž��
        for(int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                if (m_cells.ContainsKey(new Vector3Int(x, y, 0)) == false)
                    continue;

                //x,y�� �ش��ϴ� �׸��� ���� ������Ʈ���� objects ����Ʈ�� ��� �߰���.
                objects.AddRange(m_cells[new Vector3Int(x, y, 0)].Objects);
            }
        }

        return objects;
    }
}
