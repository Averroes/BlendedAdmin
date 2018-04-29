---
layout: documentation
title:  "MongoClient"
---

# MongoClient

The client implements part of collecton (https://docs.mongodb.com/manual/reference/method/js-collection) api.

## Initialization

MongoClient expects conection url as a first argument. 
The client will connnect to the database and expose collections as properties.

 ```javascript
  var mongoClient = new  MongoClient('mongodb://UserId:Password@ServerUrl:ServerPort/DatabaseName');
  
  mongoClient.SomeCollection.find();
```

## Find

 ```javascript
 // to find all items
  mongoClient.bios.find();
  
  // to find using filter
  db.bios.find({ _id: 5 });
  
  // to find with projection
   db.bios.find({ }, { name: 1, contribs: 1, _id: 0 });
```
