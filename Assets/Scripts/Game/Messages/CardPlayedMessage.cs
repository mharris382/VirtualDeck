using UniRx;

public class CardPlayedMessage
{
    public CardInstance CardInstance;
    public Subject<Unit> OnFinished {get;private set;} = new Subject<Unit>();
    public CardPlayedMessage(CardInstance cardInstance)
    {
        CardInstance = cardInstance;
        
    }
    
    
    public void CardAnimationFinished(){
        OnFinished.OnNext(Unit.Default);
        OnFinished.OnCompleted();
    }
}



/*

Hey is this a bad way to use messages (using unirx MessageBroker)?
```
public class CardPlayedMessage
{
    public CardInstance CardInstance;
    public Subject<Unit> OnCardPlayingFinished {get;private set;} = new Subject<Unit>();
    public CardPlayedMessage(CardInstance cardInstance){CardInstance = cardInstance;}
    public void CardAnimationFinished(){
       OnCardPlayingFinished.OnNext(Unit.Default);
       OnCardPlaying.OnCompleted();
    }
}
```
Somewhere card is being played
```
 MessageBroker.Default.Publish(new CardPlayedMessage(this.Card));
```

The system responsible for animating stuff
```
MessageBroker.Default.Recieve<CardPlayedMessage>().Subscribe(msg => AddToCardToAnimQueue(msg.Card))
```


The system responsible for cleaning up after cards have been played
```
MessageBroker.Default.Receive<CardPlayedMessage>().Subscribe(msg => msg.OnFinished.Take(1).Subscribe(_ => CleanupCard(msg.Card))
```

*/