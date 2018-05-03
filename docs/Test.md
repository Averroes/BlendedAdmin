
---
layout: documentation
title:  "TableView"
---

Elastic
 ```javascript
function main(arg)
{
   var httpClient = new HttpClient();
   var matchAllItems = httpClient.post({
     url:'http://user:pass@elastic.url/posts/_search',
     headers: {'Content-Type':'application/json'},
     body: {'query': {'match_all': { }}}
   });
   var matchAllItemsBody = JSON.parse(matchAllItems.body);
   var matchAllItemsHits = matchAllItemsBody.hits.hits.map(function(a) {return a['_source'];});
   
   var tableView = new TableView(matchAllItemsHits); 
   return [tableView];
}
main(arg);
```

Mongo
 ```javascript
function main(arg)
{
   var mongoClient = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
   var items = mongoClient
     .bios
     .find({'name.first': 'John'}, { _id:0, name: 1, birth: 1})
     .toArray();

   var tableView = new TableView(items);
   return tableView;
}
main(arg);
```

