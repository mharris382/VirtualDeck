using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Security;
using UniRx;
using Debug = System.Diagnostics.Debug;

public class CellManager : MonoBehaviour
{
    public TestCellEntity[] entities;
    
    [Required, AssetsOnly]
    public Cell prefab;

    [Range(4, 15)]
    public int initialPoolSize = 10;


    public Transform cellParent;
    public Transform poolParent;


    private Queue<Cell> _readyQueue = new Queue<Cell>();

    private List<Cell> _active = new List<Cell>();
    
    private Dictionary<Cell, IDisposable> _listeners = new Dictionary<Cell, IDisposable>();
    private Dictionary<ICellEntity, Cell> _cellAssignments = new Dictionary<ICellEntity, Cell>();
    private bool _dirty = false;


    public Vector3 startPosition;
    public float xOffset;


    public void Awake()
    {
        poolParent ??= new GameObject($"{name} Inactive Cells Pool").transform;
        cellParent ??= this.transform;
        
        for (int i = 0; i < initialPoolSize; i++)
        {
            _readyQueue.Enqueue(CreateNewCell());
        }
    }


    public void Start()
    {
        IEnumerable<TestCellEntity> orderedEntities = from e in entities
                orderby e.startSpeed
                where e != null
                select e;

        foreach (var testCellEntity in orderedEntities)
        {
            AssignCellToEntity(testCellEntity);
        }
    }


    public void LateUpdate()
    {
        if (_dirty)
        {
            _active.Sort(new CellSorter());
            Vector3 start = transform.position + startPosition;
            Vector3 offset = transform.right * xOffset;
            for (int i = _active.Count-1; i >= 0; i--)
            {
                _active[i].transform.SetSiblingIndex(i);
                _active[i].index.Value = i;
                _active[i].transform.position = start + (offset * i);
            }
            _dirty = false;
        }
    }

    public void AssignCellToEntity(ICellEntity entity)
    {
        if(_readyQueue.Count == 0) _readyQueue.Enqueue(CreateNewCell());
        var cell = _readyQueue.Dequeue();
        
        cell.transform.parent = cellParent;
        entity.AssignCell(cell);
        
        var d = cell.cellScore.Subscribe(score => { _dirty = true; });
        
        _active.Add(cell);
        _listeners.Add(cell, d);
        _cellAssignments.Add(entity, cell);
        
        _dirty = true;
        
    }

    public void RemoveEntity(ICellEntity entity)
    {
        if (_cellAssignments.ContainsKey(entity) == false) return;
        
        var cell = _cellAssignments[entity];
        _cellAssignments.Remove(entity);
        DeactivateCell(cell);
        
        
        var listener = _listeners[cell];
        _listeners.Remove(cell);
        listener.Dispose();

        _active.Remove(cell);

        _dirty = true;
    }

    private void DeactivateCell(Cell cell)
    {
        _dirty = true;
        cell.transform.parent = poolParent;
        _readyQueue.Enqueue(cell);
    }
    
    Cell CreateNewCell()
    {
        var inst = Instantiate(prefab, poolParent);
        return inst;
    }
    
    
    
    private class CellSorter : IComparer<Cell>
    {
        public int Compare(Cell x, Cell y)
        {
            Debug.Assert(x != null, nameof(x) + " != null");
            Debug.Assert(y != null, nameof(y) + " != null");
            return x.CellScore - y.CellScore;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var p0 = transform.position + startPosition;
        var p1 = p0 + (transform.right * xOffset);
        Gizmos.DrawSphere(p0, 0.125f);
        Gizmos.DrawSphere(p1, 0.125f);
        Gizmos.DrawLine(p0, p1);
    }
}