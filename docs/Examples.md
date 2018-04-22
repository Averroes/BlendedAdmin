---
layout: documentation
title:  "Examples"
---

```javascript
function main(arg)
{
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source=chinook.db;'});
  var employees = sqlClient.query('select * from employees');
  var tableView = new TableView(employees);
  return [tableView];
}
main(arg);
```

```javascript
function main(request)
{
  if (request.method == 'get')
  {
    var formView = new FormView({
      method:'post',
      controls: [[{name:'firstName'},{name:'lastName'}]]
    });
    return [formView];
  }
  
  if (arg.method == 'post')
  {
    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source=chinook.db;'});
    var employees = sqlClient.query(
      'insert into employees(firstName, lastName) values(@firstName,@lastName)',
      {firstName: request.form.firstName, lastName: request.form.lastName}
    );
    var htmlView = new HtmlView('<div>Thank you for adding new employee.</div>');
    return [htmlView];
  }
}
main(arg);
```

