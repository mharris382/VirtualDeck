using System;
using Entity;
using NUnit.Framework;
using UnityEngine;
//
// public class TestMathOperationsOnEntityStats
// {
//    // private GameEntities gameEntities;
//     private const string HP_STAT = "hp";
//     private const int INITIAL_HP_VALUE = 10;
//     
//     [SetUp]
//     public void Setup()
//     {
//         //var go = new GameObject("GameEntities", typeof(GameEntities));
//         //this.gameEntities = go.GetComponent<GameEntities>();
//     }
//
//
//     [TearDown]
//     public void TearDown()
//     {
//        
//     }
//     
//
//     private void InitEntity(Entity.Entity entity)
//     {
//        // this.gameEntities.entities = new[] {entity};
//        // gameEntities.Initialize();
//     }
//
//     private Entity.IEntity CreateAndInitPlayerEntity()
//     {
//         var entity = ScriptableObject.CreateInstance<Entity.Entity>();
//         entity.name = "Player1";
//         entity.SetAbbreviations(new[] {"P1", "p1"});
//
//         //this.gameEntities.entities = new[] {entity};
//         //gameEntities.Initialize();
//         return entity;
//     }
//
//     [Test]
//     public void TestAddHealth()
//     {
//         var entity = CreateAndInitPlayerEntity();
//         string phrase = "p1.hp+5";
//        // gameEntities.TryParseOperation(phrase);
//         Assert.AreEqual(INITIAL_HP_VALUE+5, entity.GetStatValue(HP_STAT));
//     }
//
//     [Test]
//     public void TestSubtractHealth()
//     {
//         var entity = CreateAndInitPlayerEntity();
//         TestMathOperationOnStat(entity, HP_STAT, '-', (a, b) => a - b);
//     }
//
//     [Test]
//     public void TestMultiplyHealth()
//     {
//         var entity = CreateAndInitPlayerEntity();
//         TestMathOperationOnStat(entity, HP_STAT, '*', (a, b) => a * b);
//     }
//
//     [Test]
//     public void TestSetHealth()
//     {
//         var entity = CreateAndInitPlayerEntity();
//         TestMathOperationOnStat(entity, HP_STAT, '=', (_, b) => b);
//     }
//     
//     
//     
//     private void TestMathOperationOnStat(Entity.IEntity entity, string statTarget, char operationLexeme, Func<int, int, int> comparisonOperation)
//     {
//         int expect = 10;
//         Assert.AreEqual(expect, entity.GetStatValue(statTarget));
//         string phrase = $"p1.{statTarget}{operationLexeme}5";
//         for (int i = 0; i < 3; i++)
//         {
//             //Assert.IsTrue(gameEntities.TryParseOperation(phrase));
//             expect = comparisonOperation(expect, 5);
//             Assert.AreEqual(expect, entity.GetStatValue(HP_STAT));
//         }
//     }
// }
//
