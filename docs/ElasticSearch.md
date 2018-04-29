---
layout: documentation
title:  "ElasticSearch"
---

# ElasticSearch

## Create Index
```javascript
 var httpClient = new HttpClient();
 var response = httpClient.put({
   url:'http://paas:4763eca5c8f733b03adbc66c3871ef33@gloin-eu-west-1.searchly.com/posts',
   headers: {'Content-Type':'application/json'},
   body: {'settings' : {'index' : { 'number_of_shards' : 3, 'number_of_replicas' : 2 }}}
 });
 console.log(response);
 //{
 //  "statusCode": 200,
 //  "reasonPhrase": "OK",
 //  "body": "{\"acknowledged\":true,\"shards_acknowledged\":true,\"index\":\"posts\"}",
 //  "headers": {
 //    "Date": "Thu, 26 Apr 2018 21:01:35 GMT",
 //    "Connection": "keep-alive",
 //    "Access-Control-Allow-Origin": "*",
 //    "Access-Control-Allow-Credentials": "true",
 //    "Access-Control-Allow-Headers": "authorization,content-type",
 //    "X-B3-TraceId": "34b538e72e1cad19"
 //  }
 //}
```

## Index item
```javascript
 var httpClient = new HttpClient();
 var response = httpClient.post({
    url:'http://paas:4763eca5c8f733b03adbc66c3871ef33@gloin-eu-west-1.searchly.com/posts/_doc/1',
    headers: {'Content-Type':'application/json'},
    body: {'user':'daniel', 'post_date':'2009-11-15T14:12:12', 'message':'trying out Elasticsearch'}
  });
 console.log(response);
 //{
 //  "statusCode": 201,
 //  "reasonPhrase": "Created",
 //  "body": "{\"_index\":\"posts\",\"_type\":\"_doc\",\"_id\":\"1\",\"_version\":1,\"result\":\"created\",\"_shards\":{\"total\":2,\"successful\":2,\"failed\":0},\"_seq_no\":0,\"_primary_term\":1}",
 //  "headers": {
 //    "Date": "Thu, 26 Apr 2018 21:13:06 GMT",
 //    "Connection": "keep-alive",
 //    "Location": "\/4566cf42-1bbe-44a1-9e8c-43d7bfbf42a1_posts\/_doc\/1",
 //    "Access-Control-Allow-Origin": "*",
 //    "Access-Control-Allow-Credentials": "true",
 //    "Access-Control-Allow-Headers": "authorization,content-type",
 //    "X-B3-TraceId": "367b98954ceef95c"
 //  }
 //}
```

## Get all items
```javascript
 var httpClient = new HttpClient();
 var matchAllItems = httpClient.post({
    url:'http://paas:4763eca5c8f733b03adbc66c3871ef33@gloin-eu-west-1.searchly.com/posts/_search',
    headers: {'Content-Type':'application/json'},
    body: {'query': {'match_all': { }}}
 });
 var matchAllItemsBody = JSON.parse(matchAllItems.body);
 var matchAllItemsHits = matchAllItemsBody.hits.hits.map(function(a) {return a['_source'];});;
 console.log(matchAllItemsHits);
 //[
 //  {
 //    "user": "daniel",
 //    "post_date": "2009-11-15T14:12:12",
 //    "message": "trying out Elasticsearch"
 //  }
 //]
```

## Delete Index
```javascript
 var httpClient = new HttpClient();
 var response = httpClient.delete({
    url:'http://paas:4763eca5c8f733b03adbc66c3871ef33@gloin-eu-west-1.searchly.com/posts',
 });
 console.log(response);
 //{
 //  "statusCode": 200,
 //  "reasonPhrase": "OK",
 //  "body": "{\"acknowledged\":true}",
 //  "headers": {
 //    "Date": "Thu, 26 Apr 2018 21:10:49 GMT",
 //    "Connection": "keep-alive",
 //    "Access-Control-Allow-Origin": "*",
 //    "Access-Control-Allow-Credentials": "true",
 //    "Access-Control-Allow-Headers": "authorization,content-type",
 //    "X-B3-TraceId": "ba0185832d758881"
 //  }
 //}
```
