---
layout: documentation
title:  "Documentation"
---

# Sql - Show list of items

The following code will display the first 200 albums.

```javascript
function main(arg)
{
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source=chinook.db;'});
  var albums = sqlClient.query('select * from albums');
  var tableView = new TableView(employees);
  return [tableView];
}
main(arg);
```
![Sql - Show List Of Items](images/Sql_ShowListOfItems.PNG)


Pagging can be implemented in the following way. 

```javascript
function main(arg)
{
  var page = arg.queryString.p ? parseInt(arg.queryString.p) : 1;
  var pageSize = 20;
  var begin = (page - 1) * pageSize;
  var end = begin + pageSize;
  
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source=chinook.db;'});
  var albums = sqlClient.query('select * from albums').slice(begin, end);
  
  var tableView = new TableView(albums);
  return [tableView];
}
main(arg);
```

The code above is is not very effective as the entire list is be retrived from the server just to display first page. Depending on database, the server side rendering can be implemented in different way. In Sqlite, we can use *limit* keyword to limit number of records returned by query.

```javascript
function main(arg)
{
  var page = arg.queryString.p ? parseInt(arg.queryString.p) : 1;
  var pageSize = 20;
  var begin = (page - 1) * pageSize;
  var end = begin + pageSize;
  
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source=chinook.db;'});
  var albums = sqlClient.query('select * from albums limit' + end).slice(begin, end);
  
  var tableView = new TableView(albums);
  return [tableView];
}
main(arg);
```
