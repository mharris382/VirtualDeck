using System;
using Entity;
using NUnit.Framework;
using UnityEngine;

public class TestEntities
{
    private GameEntities gameEntities;
    
    [SetUp]
    public void Setup()
    {
        var go = new GameObject("GameEntities", typeof(GameEntities));
        this.gameEntities = go.GetComponent<GameEntities>();
    }


    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(this.gameEntities.gameObject);
    }


    public void TestHealthExists()
    {
        
    }


    [Test]
    public void TestParse()
    {
        var entity = ScriptableObject.CreateInstance<Entity.Entity>();
        entity.name = "Player1";
        entity.SetAbbreviations(new []{"P1", "p1"});

        string statTarget = "hp";
        
        
        
        InitTestEntities(entity);
        //NOTE: Default stat value is 10
        string phrase1 = $"p1.hp-20";
        int expect1 = -10;
        Assert.IsTrue(gameEntities.TryParseOperation(phrase1));
        Assert.AreEqual(expect1, entity.GetStatValue("hp"));
        string phrase2 = $"P1.hp+15";
        int expect2 = 5;
        Assert.IsTrue(gameEntities.TryParseOperation(phrase2));
        Assert.AreEqual(expect2, entity.GetStatValue("hp"));
        string phrase3 = $"p1.hp=10";
        int expect3 = 10;
        Assert.IsTrue(gameEntities.TryParseOperation(phrase3));
        Assert.AreEqual(expect3, entity.GetStatValue("hp"));
        string phrase4 = $"P1.hp/10";
        int expect4 = 1;
        Assert.IsTrue(gameEntities.TryParseOperation(phrase4));
        Assert.AreEqual(expect4, entity.GetStatValue("hp"));
        
    }

    private void InitTestEntities(Entity.Entity entity)
    {
        this.gameEntities.entities = new[] {entity};
        gameEntities.Initialize();
    }


    [Test]
    public void TestAddHealth()
    {
        var entity = ScriptableObject.CreateInstance<Entity.Entity>();
        entity.name = "Player1";
        entity.SetAbbreviations(new []{"P1", "p1"});
        string statTarget = "hp";
        
        InitTestEntities(entity);

        int expect = 10;
        Assert.AreEqual(expect, entity.GetStatValue(statTarget));
        string phrase = "p1.hp+5";
        for (int i = 0; i < 4; i++)
        {
            Assert.IsTrue(gameEntities.TryParseOperation(phrase));
            expect += 5;
            Assert.AreEqual(expect, entity[statTarget]);
        }
    }
    
    [Test]
    public void TestSubtractHealth()
    {
        var entity = ScriptableObject.CreateInstance<Entity.Entity>();
        entity.name = "Player1";
        entity.SetAbbreviations(new []{"P1", "p1"});
        string statTarget = "hp";
        InitTestEntities(entity);
        TestMathOperationOnStat(entity, statTarget, '-', (a, b) => a - b);
    }

    [Test]
    public void TestMultiplyHealth()
    {
        var entity = ScriptableObject.CreateInstance<Entity.Entity>();
        entity.name = "Player1";
        entity.SetAbbreviations(new []{"P1", "p1"});
        string statTarget = "hp";
        InitTestEntities(entity);
        TestMathOperationOnStat(entity, statTarget, '*', (a, b) => a * b);
    }

    private void TestMathOperationOnStat(Entity.Entity entity, string statTarget, char operationLexeme, Func<int, int, int> comparisonOperation )
    {
        int expect = 10;
        Assert.AreEqual(expect, entity.GetStatValue(statTarget));
        string phrase = $"p1.{statTarget}{operationLexeme}5";
        for (int i = 0; i < 3; i++)
        {
            Assert.IsTrue(gameEntities.TryParseOperation(phrase));
            expect = comparisonOperation(expect, 5);
            Assert.AreEqual(expect, entity[statTarget]);
        }
    }
    
}