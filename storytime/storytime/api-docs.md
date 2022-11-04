# API docs

{% swagger method="post" path="authenticate" baseUrl="https://storytime.nl/core/api/v1/" summary="Authenticate" %}
{% swagger-description %}
Authenticate to receive an access token.
{% endswagger-description %}

{% swagger-parameter in="body" name="email" required="true" type="String" %}
Email to login
{% endswagger-parameter %}

{% swagger-parameter in="body" name="password" type="String" required="true" %}
Password to login
{% endswagger-parameter %}

{% swagger-response status="200: OK" description="" %}
```javascript
{
    // Response
    'token_type': 'Bearer',
    'access_token': '...',
    'id_token': '...',
    'refresh_token': '...',
    'expires_in': 3600
}
```
{% endswagger-response %}

{% swagger-response status="401: Unauthorized" description="" %}
```javascript
{
    // Response
    'error': 'You are not authorized!'
}
```
{% endswagger-response %}
{% endswagger %}

{% swagger method="get" path="me" baseUrl="https://storytime.nl/core/api/v1/" summary="Receive whether the token is valid" %}
{% swagger-description %}

{% endswagger-description %}

{% swagger-parameter in="header" required="true" name="Authorization" type="String" %}
Bearer <token>
{% endswagger-parameter %}

{% swagger-response status="200: OK" description="" %}
```javascript
{
    // Response
    'msg': 'Token is valid'
}
```
{% endswagger-response %}

{% swagger-response status="401: Unauthorized" description="" %}
```javascript
{
    // Response
    'error': 'error message'
}
```
{% endswagger-response %}
{% endswagger %}

{% swagger method="get" path="firebase/projects" baseUrl="https://storytime.nl/core/api/v1/" summary="Get all projects" %}
{% swagger-description %}
Get all projects from a specific user.
{% endswagger-description %}

{% swagger-parameter in="header" name="Authorization" type="String" required="true" %}
Bearer <token>
{% endswagger-parameter %}

{% swagger-response status="200: OK" description="" %}
```javascript
{
    // Response
    'projects': [
      '$project': {
        'gameStats': {
          'formulaEnemies': '...',
          'formulaPlayers': '...',
          'maxLevel': 99
        },
        'members': {
          '$member': true | false
        },
        'metadata': {
          'alias': '...'
          'created_at': 0,
          'deleted': true | false,
          'description': '...'
          'languages': {
            '$language': true | false,
          },
          'owner': '...' // Owner id
          'private': true | false,
          'relatedTables': {
            'characters': "..." // table id
            // ... etc
          },
          'title': '...',
          'updated_at': 0,
          'version': {
            'major': 2020,
            'minor': 1,
            'patch': 3
          }
        }
        'referenceTables'
        'tables': {
          '$table': {
            'enabled: true | false,
            // 2020.1.5 and under...
            'description': '...',
            'name': '...',
            // 2020.1.6 and up...
            'metadata': {
              'description': '...',
              'name': '...'
            }
          }
        }
      } 
    ]
}
```
{% endswagger-response %}

{% swagger-response status="401: Unauthorized" description="" %}
```javascript
{
    // Response
    'error_msg': 'error message or No project found with the name <project> found'
}
```
{% endswagger-response %}
{% endswagger %}

{% swagger method="get" path="firebase/projects/{project}" baseUrl="https://storytime.nl/core/api/v1/" summary="Get a specific project" %}
{% swagger-description %}
Get a specific project.
{% endswagger-description %}

{% swagger-parameter in="header" type="String" name="Authorization" required="true" %}
Bearer <token>
{% endswagger-parameter %}

{% swagger-response status="200: OK" description="" %}
```javascript
{
  // Response
  'gameStats': {
    'formulaEnemies': '...',
    'formulaPlayers': '...',
    'maxLevel': 99
  },
  'members': {
    '$member': true | false
  },
  'metadata': {
    'alias': '...'
    'created_at': 0,
    'deleted': true | false,
    'description': '...'
    'languages': {
    '$language': true | false,
  },
  'owner': '...' // Owner id
  'private': true | false,
  'relatedTables': {
    'characters': "..." // table id
    // ... etc
  },
  'title': '...',
  'updated_at': 0,
  'version': {
    'major': 2020,
    'minor': 1,
    'patch': 3
  }
  'referenceTables'
  'tables': {
    '$table': {
      'enabled: true | false,
      // 2020.1.5 and under...
      'description': '...',
      'name': '...',
      // 2020.1.6 and up...
      'metadata': {
        'description': '...',
        'name': '...'
      }
    }
  }
}
```
{% endswagger-response %}

{% swagger-response status="401: Unauthorized" description="" %}
```javascript
{
    // Response
   'error_msg': 'error message or No project found with the name $project found'
}
```
{% endswagger-response %}
{% endswagger %}

{% swagger method="get" path="firebase/projects/{project}/tables" baseUrl="https://storytime.nl/core/api/v1/" summary="Get all tables of a project" %}
{% swagger-description %}

{% endswagger-description %}

{% swagger-parameter in="header" type="String" name="Authorization" required="true" %}
Bearer <token>
{% endswagger-parameter %}

{% swagger-response status="200: OK" description="" %}
```javascript
[
  // Response
  '$table': {
    'data': {
      '$increment': {
        '$key': '$value'
      }
    },
    'metadata': {
      'created_at': 0,
      'deleted': true | false,
      'description': '...',
      'lastUID': '$increment',
      'owner': '...', // owner id
      'private': true | false,
      'title': '...',
      'updated_at': 0,
      'version': {
        'major': 2020,
        'minor': 1,
        'patch': 3
      }
    },
    'projectID': '$project', // project id
    'revisions': [ 
      {
        'created_at': 0,
        'currentRevID': 0,
        'deleted': true | false,
        'newValue': {
          '$key': '$value',
        },
        'oldValue': {
          '$key': '$value',
        },
        'revision': 0, 
        'rowID': '$increment' 
        'uid': '...' // member id
        'updated_at': 0
      }
    ]
  }
]
```
{% endswagger-response %}

{% swagger-response status="401: Unauthorized" description="" %}
```javascript
{
    // Response
}
```
{% endswagger-response %}
{% endswagger %}

{% swagger method="get" path="firebase/projects/{project}/tables/{table}" baseUrl="https://storytime.nl/core/api/v1/" summary="Get specific table from a project." %}
{% swagger-description %}

{% endswagger-description %}

{% swagger-parameter in="header" name="Authorization" type="String" required="true" %}
Bearer <token>
{% endswagger-parameter %}

{% swagger-response status="200: OK" description="" %}
```javascript
{
  // Response
  'data': {
    '$increment': {
      '$key': '$value'
    }
  },
  'metadata': {
    'created_at': 0,
    'deleted': true | false,
    'description': '...',
    'lastUID': '$increment',
    'owner': '...', // owner id
    'private': true | false,
    'title': '...',
    'updated_at': 0,
    'version': {
      'major': 2020,
      'minor': 1,
      'patch': 3
    }
  },
  'projectID': '$project', // project id
  'revisions': [ 
    {
      'created_at': 0,
      'currentRevID': 0,
      'deleted': true | false,
      'newValue': {
        '$key': '$value',
      },
      'oldValue': {
        '$key': '$value',
      },
      'revision': 0, 
      'rowID': '$increment' 
      'uid': '...' // member id
      'updated_at': 0
    }
  ]
}
```
{% endswagger-response %}

{% swagger-response status="401: Unauthorized" description="" %}
```javascript
{
    // Response
}
```
{% endswagger-response %}
{% endswagger %}
