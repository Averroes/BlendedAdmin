---
layout: documentation
title:  "SqlClient"
---

# SqlClient

## Initialization

- Sqlite
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = database.db;'});
  
  //using properties
  var sqlClient = new  SqlClient({provider:'Sqlite', dataSource:'database.db'});
```

- MySql
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'MySql',connectionString:'SERVER=ServerUrl;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'MySql',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'mysql://UserId:Password@ServerUrl/DatabaseName'});
```

- MariaDb
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'MariaDb',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'MariaDb',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'mariadb://UserId:Password@ServerUrl/DatabaseName'});
```

- Postgres
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Postgres',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'Postgres',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'postgres://UserId:Password@ServerUrl/DatabaseName'});
```

- Oracle (not tested)
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Oracle',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'Oracle',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'oracle://UserId:Password@ServerUrl/DatabaseName'});
```

- SqlServer (not tested)
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'SqlServer',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'SqlServer',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'sqlserver://UserId:Password@ServerUrl/DatabaseName'});
```

- DB2 (not tested)
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'DB2',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'DB2',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'db2://UserId:Password@ServerUrl/DatabaseName'});
```

- Odbc
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Odbc',connectionString:'Driver={MySQL ODBC 5.1 Driver};SERVER=ServerUrl;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'Odbc',
                        driver:'{MySQL ODBC 5.1 Driver}',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Password'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'mysql://UserId:Password@ServerUrl/DatabaseName'});
```

## Run Select Query
 
- Simple select query
```javascript
  var rows = sqlClient.query('select * from employees');
  console.log(rows);   // [{Id:1, Name:'John'},{Id:2, Name:'Mike'},...]
```

- Select with parameters
```javascript
  // first version
  var rows = sqlClient.query('select * from employees where Id=@Id', {Id:1});
  console.log(rows);   // [{Id:1, Name:'John'}]
 
  // second version
  var rows = sqlClient.query({
    sql:'select * from employees where Id=@Id', 
    parameters:{Id:1}
  });
  console.log(rows);   // [{Id:1, Name:'John'}]
```

## Run Cursor

- Read records using hasNext() and next() methods.
```javascript
 var cursor = sqlClient.cursor('select * from employees');
 while(cursor.hasNext())
 {
   var item = cursor.next();
   console.log(item);
 }
 cursor.close();
 // {Id:1, Name:'John'}
 // {Id:2, Name:'Mike'}
```

- Read records using each(function(){}) method
```javascript
var cursor = sqlClient.cursor('select * from employees');
cursor.each(function(item) {
 console.log(item);
});
cursor.close();
// {Id:1, Name:'John'}
// {Id:2, Name:'Mike'}
```

- Read first item
```javascript
var cursor = sqlClient.cursor('select * from employees where Id=@Id', {Id:1});
var item = cursor.first();
cursor.close();
console.log(item); // [{Id:1, Name:'John'}]
```

## Multiline quries

Use grave accent to write multilines queries
```javascript
 var rows = sqlClient.query(`select * 
 from employees`);
 console.log(rows);   // [{Id:1, Name:'John'},{Id:2, Name:'Mike'},...]
```
 
## Connect and End

When the query or cursor is run, the client connects to the database automaticaly, and the connection is close when the script is finished. Behaviour can be controlled with connect() and end() methods.
```javascript
  sqlClient.connect();
  
  sqlClient.query(...);
  
  sqlClient.end();
```

## Transactions

To run several queries in transaction, first you need to connect and then start transaction. After running the queries, commit it.
```javascript
  sqlClient.connect();
  sqlClient.transaction();
  
  sqlClient.query('INSERT ...');
  sqlClient.query('UPDATE ...');
  
  sqlClient.commit();
```

Transaction can also be rollback at any point.
```javascript
  sqlClient.connect();
  sqlClient.transaction();
  
  sqlClient.query('INSERT ...');
  sqlClient.query('UPDATE ...');
  
  sqlClient.rollback();
```

## Error handling

In case of any error, the client throws error. You have to use try-catch statement to handle them.
```javascript
  try {
    var sqlClient = new  SqlClient({provider:'MySql',connectionString:'Server=blabla;Database=blabla;UID=blabla;PASSWORD=blabla;'});
    sqlClient.connect();
  } catch(err) {
    console.log(err); // Cannot connect to the database......
  }
```
