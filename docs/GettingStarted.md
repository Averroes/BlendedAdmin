---
layout: documentation
title:  "Getting started"
---

# Getting started

How it works? <br/>
Write JavaScript code, <br/>
that<br/>

* connects to applications or databases (eg. MySql, RestApi)
* do actions (eg. get list of products)
* returns view (eg. table with products)

## main function

The following snipped is the hello word in the BlendedAdmin. Main function returns HtmlView that is rendered on the page.

```javascript
function main(arg)
{
   var htmlView = new HtmlView('<div>hello word!</div>'); 
   return [htmlView];
}
main(arg);
```
