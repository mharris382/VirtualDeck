using UnityEngine;

public class TestCellEntity : MonoBehaviour, ICellEntity
{
    public Cell cell;
    public int startSpeed = 5;
    public void AssignCell(Cell cell)
    {
        this.cell = cell;
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
    }
}