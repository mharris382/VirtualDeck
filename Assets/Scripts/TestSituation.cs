using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Excel;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;

public class TestSituation : MonoBehaviour
{
    [Sirenix.OdinInspector.FilePath(AbsolutePath = true, Extensions = "xlsx")]
    public string path;


    public string value = "A0";


    [Button]
    void Test2()
    {
        var allWs = Workbook.Worksheets(path);
        var ws = allWs.First();

        var r = value[0];
        var c = value[1];
        int row = ((int) r) - 65;
        int col = Int32.Parse(c.ToString());
        string s = $"Cell{value} ({row},{col}) = {ws.Rows[col].Cells[row].Text}";
        Debug.Log(s);
    }


    void Save()
    {
    }

    [Button]
    void Run()
    {
        foreach (var ws in Workbook.Worksheets(path))
        {
            int rowNum = 0;
            foreach (var row in ws.Rows)
            {
                int colNum = 0;

                foreach (var cell in row.Cells)
                {
                    char colC = (char) (colNum + 65);
                    Debug.Log($"Cell({colC},{rowNum + 1})" + cell.Text.ToString());
                    colNum++;
                }

                rowNum++;
            }
        }

        // var csvReader = new CsvReader(reader);
        // var records = csvReader.GetRecords<Automobile>();
    }
}


class Automobile
{
    public string Make { get; set; }
    public string Model { get; set; }
    public AutomobileType Type { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public AutomobileComment Comment { get; set; }
}

class AutomobileComment
{
    public string Comment { get; set; }
}

enum AutomobileType
{
    None,
    Car,
    Truck,
    Motorbike
}

