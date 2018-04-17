---
layout: documentation
title:  "HttpClient"
---

# HttpClient

## Get
- Get website content
 ```javascript
  var httpClient = new HttpClient();
  var response = httpClient.get('https://www.theguardian.com');
  console.log(response);
  //{
  //"statusCode": 200,
  //"reasonPhrase": "OK",
  //"body": "\n<!DOCTYPE html>\n<html ...",
  //"headers": {
  //  "Accept-Ranges": "bytes",
  //  "Date": "Thu, 12 Apr 2018 20:27:26 GMT",
  //  "Age": "0",
  //  "Connection": "keep-alive"
  //}
}
```

- Get json
 ```javascript
var httpClient = new HttpClient();
var response = httpClient.get({
  url:'https://jsonplaceholder.typicode.com/posts',
  headers: {'Content-Type':'application/json'}
 });
response.bodyJson = JSON.parse(response.body);                        
console.log(response);
//{
//  "statusCode": 200,
//  "reasonPhrase": "OK",
//  "body": "[\n  {\n    \"userId\": 1,\n    \"id\": 1,\n    \"title\": \"sunt aut facere repellat provident occaecati ...",
//  "headers": {
//    "Connection": "keep-alive",
//    "Date": "Sun, 15 Apr 2018 12:41:51 GMT",
//    "Pragma": "no-cache"
//  },
//  "bodyJson": [
//    {
//      "userId": 1,
//      "id": 1,
//      "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
//      ...
//    },
//    ...
//  ]
//}
```

## Post
- Post json
```javascript
var httpClient = new HttpClient();
var response = httpClient.post({
  url:'https://jsonplaceholder.typicode.com/posts',
  headers: {'Content-Type':'application/json'},
  body: { userId: 1, title: 'bla', body: 'bla bla bla'}
  });
response.bodyJson = JSON.parse(response.body);                        
console.log(response);
//{
//  "statusCode": 201,
//  "reasonPhrase": "Created",
//  "body": "{\n  \"id\": 101\n}",
//  "headers": {
//    "Cache-Control": "no-cache",
//    "Connection": "keep-alive",
//    "Date": "Sun, 15 Apr 2018 12:46:32 GMT"
//  },
//  "bodyJson": {
//    "id": 101
//  }
//}
```

## Put
- Put json
```javascript
var httpClient = new HttpClient();
var response = httpClient.put({
  url:'https://jsonplaceholder.typicode.com/posts/1',
  headers: {'Content-Type':'application/json'},
  body: { userId: 1, title: 'bla', body: 'bla bla bla'}
  });
response.bodyJson = JSON.parse(response.body);                        
console.log(response);
//{
//  "statusCode": 200,
//  "reasonPhrase": "OK",
//  "body": "{\n  \"id\": 1\n}",
//  "headers": {
//    "Cache-Control": "no-cache",
//    "Connection": "keep-alive",
//    "Date": "Sun, 15 Apr 2018 12:48:14 GMT"
//  },
//  "bodyJson": {
//    "id": 1
//  }
//}
```

## Delete
- Delete json
```javascript
var httpClient = new HttpClient();
var response = httpClient.delete({
 url:'https://jsonplaceholder.typicode.com/posts/1'
 });
response.bodyJson = JSON.parse(response.body);                        
console.log(response);
//{
//  "statusCode": 200,
//  "reasonPhrase": "OK",
//  "body": "{}",
//  "headers": {
//    "Cache-Control": "no-cache",
//    "Connection": "keep-alive",
//    "Date": "Sun, 15 Apr 2018 12:49:52 GMT"
//  },
//  "bodyJson": {}
//}
```

## Head
- Head json
```javascript
var httpClient = new HttpClient();
var response = httpClient.head({
 url:'https://jsonplaceholder.typicode.com/posts/1'
 });
console.log(responseStatus);
//{
//  "statusCode": 200,
//  "reasonPhrase": "OK",
//  "body": "",
//  "headers": {
//    "Cache-Control": "public, max-age=14400",
//    "Connection": "keep-alive",
//    "Date": "Sun, 15 Apr 2018 13:06:40 GMT"
//  }
//}
```

## Send
- Send head
```javascript
var httpClient = new HttpClient();
var response = httpClient.send({
  url:'https://jsonplaceholder.typicode.com/posts/1',
  method:'head'
});
console.log(response);
//{
//  "statusCode": 200,
//  "reasonPhrase": "OK",
//  "body": "",
//  "headers": {
//    "Cache-Control": "public, max-age=14400",
//    "Connection": "keep-alive",
//    "Date": "Sun, 15 Apr 2018 13:10:00 GMT"
//  }
//}
```

