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
function main(arg)
{
  var formView = new FormView({
    method:'post',
    controls: [[{name:'firstName', value:arg.form.firstName},{name:'lastName', value:arg.form.firstName}]]
  });
  
  if (arg.method == 'post')
  {
    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source=chinook.db;'});
    var employees = sqlClient.query(
      'insert into employees(firstName, lastName) values(@firstName,@lastName)',
      {firstName: arg.form.firstName, lastName: arg.form.lastName}
    );
  }

  return [formView];
}
main(arg);
```

