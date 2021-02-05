using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class UIPileCount : MonoBehaviour
{
  [Required] [ChildGameObjectsOnly] public TMPro.TextMeshProUGUI drawPile;
  [Required] [ChildGameObjectsOnly] public TMPro.TextMeshProUGUI discardPile;

 [Required]  public Transform drawPileParent;
 [Required]  public Transform discardPileParent;

   private void Awake()
   {
       var pubsub = MessageBroker.Default;

       Observable.Merge(
       pubsub.Receive<CardAddedToHandMessage>().AsUnitObservable(),
       pubsub.Receive<DiscardMessage>().AsUnitObservable(),
       pubsub.Receive<NewTurnMessage>().AsUnitObservable())
               .DelayFrame(1).Subscribe(_ => ReadPileCounts());
        
   }

   private void Start()
   {
       Invoke("ReadPileCounts", .5f);
   }

   private void ReadPileCounts()
   {
       drawPile.text = drawPileParent.childCount.ToString();
       discardPile.text = discardPileParent.childCount.ToString();
   }
}
