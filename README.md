# VirtualDeck
A virtual deck of cards meant for playtesting paper prototypes without paper

The library of cards are generated by editing the EXCEL document (Cards.xlsx)

The xlsx document functions as a database for the application.   The first page defines the library of cards, the second page defines the number of copies in each deck (cards are referenced by their name). 

format to define new cards

| Card Name   | Description | AP (Action Points) | Art  |
| ----------- | ----------- | ----------- | ----------- |
| name of card      | text description       | Integer     | name of .png file in resources folder            |
