# STPP_Projektas
 Saityno taikomųjų programų projektavimo kurso projektas
<hr/>
<h3>Pasirinktų technologijų aprašymas</h3>
<p>⚝ DB sprendimas - PostgreSQL</p>
<p>⚝ Backend - .NET 8 su Entity Framework DB interakcijai</p>
<p>⚝ Frontend - React su Node.js</p>


<hr/>
<h3>Sprendžiamo uždavinio aprašymas</h3>
<hr/>
<h4>Sistemos paskirtis</h4>

Sistema leidžia naudotojams bendrauti administratorių moderuojamose forumuose įrašų ir komentarų forma
<hr/>
<h4>Funkciniai reikalavimai</h4>

<p>⚝ Naudotojas gali sukurti forumus, forume gali skelbti įrašus ar ištrinti savus, skaityti ar komentuoti ant kitų įrašųS</p>
<p>⚝ Administratorius gali sukurti forumus, trinti įrašus ir komentarus</p>
<p>⚝ Svečias gali prisijungti</p>
<hr/>

<h4>Sistemos architektūra (deployment diagrama UML)</h4>

![Deployment diagram](https://raw.githubusercontent.com/DovKat/STPP_Projektas/refs/heads/main/Exports/DeployUML.png)

</h4>Naudotojo sąsajos projektas </h4>

</h5>Forumų langas (eskizas)</h5>

![Deployment diagram](https://raw.githubusercontent.com/DovKat/STPP_Projektas/refs/heads/main/Exports/ForumWindow.png)

</h5>Forumų langas (screenshot)</h5>

![Deployment diagram](https://raw.githubusercontent.com/DovKat/STPP_Projektas/refs/heads/main/Exports/ForumsReal.PNG)

<hr/>
</h5>Įrašų langas (eskizas)</h5>

![Deployment diagram](https://raw.githubusercontent.com/DovKat/STPP_Projektas/refs/heads/main/Exports/PostWindow.png)

</h5>Įrašų langas (screenshot)</h5>

![Deployment diagram](https://raw.githubusercontent.com/DovKat/STPP_Projektas/refs/heads/main/Exports/PostReal.PNG)

<hr/>
</h5>Įrašų detalių langas (eskizas)</h5>

![Deployment diagram](https://raw.githubusercontent.com/DovKat/STPP_Projektas/refs/heads/main/Exports/SpecificPostWindow.png)

</h5>Įrašų detalių langas (screenshot)</h5>

![Deployment diagram](https://raw.githubusercontent.com/DovKat/STPP_Projektas/refs/heads/main/Exports/PostDetailsReal.PNG)

<hr/>
<h4>Išvados</h4>

Naudojant minimal API ir EF greitai ir paprastai sukurtas API projektas, naudojant Swagger sukurta dokumentacija. API patalpintas kaip Azure web app.

PostgreSQL duombazė sukurta iš kodo modelių su EF pagalba. Ji patalpina AWS RDS.

Frontend projektas sukurtas React frameworku. Patalpintas kaip Azure static web app

<h4>API specifikacija</h4>

API užklausų ir atsakų pavyzdžiai, metodų aprašymai:
```Swagger.json failas:
{
  "openapi": "3.0.1",
  "info": {
    "title": "Forums API",
    "version": "v1"
  },
  "paths": {
    "/api/forums/{forumId}/posts/{postId}/comments": {
      "get": {
        "tags": [
          "Comments"
        ],
        "summary": "Get All comment",
        "description": "Returns a list of all comments.",
        "operationId": "GetAllcomments",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/CommentDto"
                  }
                },
                "example": [
                  {
                    "id": 1,
                    "postId": 5,
                    "description": "This is the content of the first example Comment.",
                    "createdOn": "2024-12-05T10:08:34.4348845+00:00"
                  },
                  {
                    "id": 2,
                    "postId": 6,
                    "description": "This is the content of the Second example Comment.",
                    "createdOn": "2024-12-05T10:08:34.4348865+00:00"
                  }
                ]
              }
            }
          },
          "404": {
            "description": "Not Found"
          }
        }
      },
      "post": {
        "tags": [
          "Comments"
        ],
        "summary": "Create a new comment",
        "description": "Creates a new comment with the given data and returns the created comment.",
        "operationId": "CreateComment",
        "parameters": [
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateCommentDto"
              },
              "example": {
                "postId": 1,
                "description": "This is an example comment."
              }
            }
          },
          "required": true
        },
        "responses": {
          "201": {
            "description": "Created",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/CommentDto"
                },
                "example": {
                  "id": 1,
                  "postId": 3,
                  "description": "This is an example comment.",
                  "createdOn": "2024-12-05T10:08:34.437053+00:00"
                }
              }
            }
          },
          "422": {
            "description": "Unprocessable Content"
          }
        }
      }
    },
    "/api/forums/{forumId}/posts/{postId}/comments/{commentId}": {
      "get": {
        "tags": [
          "Comments"
        ],
        "summary": "Get comment by ID",
        "description": "Returns a comment based on the provided ID.",
        "operationId": "GetCommentById",
        "parameters": [
          {
            "name": "commentId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/CommentDto"
                },
                "example": {
                  "id": 1,
                  "postId": 3,
                  "description": "This is an example comment.",
                  "createdOn": "2024-12-05T10:08:34.4379673+00:00"
                }
              }
            }
          },
          "404": {
            "description": "Not Found"
          }
        }
      },
      "put": {
        "tags": [
          "Comments"
        ],
        "summary": "Update an existing comment",
        "description": "Updates the description of an existing comment with the given ID.",
        "operationId": "UpdateComment",
        "parameters": [
          {
            "name": "commentId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdatedCommentDto"
              },
              "example": {
                "description": "This is an example comment."
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/CommentDto"
                },
                "example": {
                  "id": 1,
                  "postId": 3,
                  "description": "This is an example comment.",
                  "createdOn": "2024-12-05T10:08:34.4402503+00:00"
                }
              }
            }
          },
          "404": {
            "description": "Not Found"
          },
          "422": {
            "description": "Unprocessable Content"
          }
        }
      },
      "delete": {
        "tags": [
          "Comments"
        ],
        "summary": "Delete a comment",
        "description": "Deletes the comment with the given ID.",
        "operationId": "DeleteComment",
        "parameters": [
          {
            "name": "commentId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "204": {
            "description": "No Content"
          },
          "404": {
            "description": "Not Found"
          }
        }
      }
    },
    "/api/forums": {
      "get": {
        "tags": [
          "Forums"
        ],
        "summary": "Get All Forums",
        "description": "Returns a list of all Forums.",
        "operationId": "GetAllForums",
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/ForumDto"
                  }
                },
                "example": [
                  {
                    "id": 1,
                    "title": "First Example Forum",
                    "description": "This is the content of the first example Forum.",
                    "createdOn": "2024-12-05T10:08:34.4425537+00:00"
                  },
                  {
                    "id": 2,
                    "title": "Second Example Forum",
                    "description": "This is the content of the Second example Forum.",
                    "createdOn": "2024-12-05T10:08:34.4425556+00:00"
                  }
                ]
              }
            }
          }
        }
      },
      "post": {
        "tags": [
          "Forums"
        ],
        "summary": "Create a new forum",
        "description": "Creates a new forum with the given data and returns the created post.",
        "operationId": "CreateForum",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateForumDto"
              },
              "example": {
                "title": "This is an example forum title",
                "description": "This is an example forum description"
              }
            }
          },
          "required": true
        },
        "responses": {
          "201": {
            "description": "Created",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/ForumDto"
                },
                "example": {
                  "id": 5,
                  "title": "This is an example forum",
                  "description": "This is an example description",
                  "createdOn": "2024-12-05T10:08:34.4443095+00:00"
                }
              }
            }
          },
          "422": {
            "description": "Unprocessable Content"
          }
        }
      }
    },
    "/api/forums/{forumId}": {
      "get": {
        "tags": [
          "Forums"
        ],
        "summary": "Get forum by ID",
        "description": "Returns a forum based on the provided ID.",
        "operationId": "GetForumById",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/ForumDto"
                },
                "example": {
                  "id": 5,
                  "title": "This is an example forum",
                  "description": "This is an example description",
                  "createdOn": "2024-12-05T10:08:34.4449835+00:00"
                }
              }
            }
          },
          "404": {
            "description": "Not Found"
          }
        }
      },
      "put": {
        "tags": [
          "Forums"
        ],
        "summary": "Update an existing post",
        "description": "Updates the description of an existing post with the given ID.",
        "operationId": "UpdateForum",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdatedForumDto"
              },
              "example": {
                "description": "This is an example description"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/ForumDto"
                },
                "example": {
                  "id": 5,
                  "title": "This is an example forum",
                  "description": "This is an example description",
                  "createdOn": "2024-12-05T10:08:34.4461921+00:00"
                }
              }
            }
          },
          "404": {
            "description": "Not Found"
          }
        }
      },
      "delete": {
        "tags": [
          "Forums"
        ],
        "summary": "Delete a forum",
        "description": "Deletes the forum with the given ID.",
        "operationId": "DeleteForum",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "204": {
            "description": "No Content"
          },
          "404": {
            "description": "Not Found"
          }
        }
      }
    },
    "/api/forums/{forumId}/posts": {
      "get": {
        "tags": [
          "Posts"
        ],
        "summary": "Get All Posts",
        "description": "Returns a list of all posts.",
        "operationId": "GetAllPosts",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/PostDto"
                  }
                },
                "example": [
                  {
                    "id": 1,
                    "forum": 2,
                    "title": "First Example Post",
                    "description": "This is the content of the first example post.",
                    "createdOn": "2024-12-05T10:08:34.447773+00:00"
                  },
                  {
                    "id": 2,
                    "forum": 5,
                    "title": "Second Example Post",
                    "description": "This is the content of the Second example post.",
                    "createdOn": "2024-12-05T10:08:34.4477737+00:00"
                  }
                ]
              }
            }
          },
          "404": {
            "description": "Not Found"
          }
        }
      },
      "post": {
        "tags": [
          "Posts"
        ],
        "summary": "Create a new post",
        "description": "Creates a new post with the given data and returns the created post.",
        "operationId": "CreatePost",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreatePostDto"
              },
              "example": {
                "title": "This is an example post",
                "forumId": 6,
                "description": "This is an example description"
              }
            }
          },
          "required": true
        },
        "responses": {
          "201": {
            "description": "Created",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PostDto"
                },
                "example": {
                  "id": 9,
                  "forum": 6,
                  "title": "This is an example post",
                  "description": "This is an example description",
                  "createdOn": "2024-12-05T10:08:34.4492297+00:00"
                }
              }
            }
          },
          "422": {
            "description": "Unprocessable Content"
          }
        }
      }
    },
    "/api/forums/{forumId}/posts/{postId}": {
      "get": {
        "tags": [
          "Posts"
        ],
        "summary": "Get Post by ID",
        "description": "Returns a post based on the provided ID.",
        "operationId": "GetPostById",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PostDto"
                },
                "example": {
                  "id": 9,
                  "forum": 6,
                  "title": "This is an example post",
                  "description": "This is an example description",
                  "createdOn": "2024-12-05T10:08:34.4498611+00:00"
                }
              }
            }
          },
          "404": {
            "description": "Not Found"
          }
        }
      },
      "put": {
        "tags": [
          "Posts"
        ],
        "summary": "Update an existing post",
        "description": "Updates the description of an existing post with the given ID.",
        "operationId": "UpdatePost",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdatedPostDto"
              },
              "example": {
                "description": "This is an example description"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PostDto"
                },
                "example": {
                  "id": 9,
                  "forum": 6,
                  "title": "This is an example post",
                  "description": "This is an example description",
                  "createdOn": "2024-12-05T10:08:34.4512336+00:00"
                }
              }
            }
          },
          "404": {
            "description": "Not Found"
          },
          "422": {
            "description": "Unprocessable Content"
          }
        }
      },
      "delete": {
        "tags": [
          "Posts"
        ],
        "summary": "Delete a post",
        "description": "Deletes the post with the given ID.",
        "operationId": "DeletePost",
        "parameters": [
          {
            "name": "forumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "postId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "204": {
            "description": "No Content"
          },
          "404": {
            "description": "Not Found"
          }
        }
      }
    },
    "/api/register": {
      "post": {
        "tags": [
          "STTP_projektas"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/RegisterUserDto"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/login": {
      "post": {
        "tags": [
          "STTP_projektas"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/LoginDto"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/accessToken": {
      "post": {
        "tags": [
          "STTP_projektas"
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/logout": {
      "post": {
        "tags": [
          "STTP_projektas"
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "CommentDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "postId": {
            "type": "integer",
            "format": "int32"
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "createdOn": {
            "type": "string",
            "format": "date-time"
          }
        },
        "additionalProperties": false
      },
      "CreateCommentDto": {
        "type": "object",
        "properties": {
          "postId": {
            "type": "integer",
            "format": "int32"
          },
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "CreateForumDto": {
        "type": "object",
        "properties": {
          "title": {
            "type": "string",
            "nullable": true
          },
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "CreatePostDto": {
        "type": "object",
        "properties": {
          "title": {
            "type": "string",
            "nullable": true
          },
          "forumId": {
            "type": "integer",
            "format": "int32"
          },
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "ForumDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "createdOn": {
            "type": "string",
            "format": "date-time"
          }
        },
        "additionalProperties": false
      },
      "LoginDto": {
        "type": "object",
        "properties": {
          "username": {
            "type": "string",
            "nullable": true
          },
          "password": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "PostDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "forum": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "createdOn": {
            "type": "string",
            "format": "date-time"
          }
        },
        "additionalProperties": false
      },
      "RegisterUserDto": {
        "type": "object",
        "properties": {
          "username": {
            "type": "string",
            "nullable": true
          },
          "email": {
            "type": "string",
            "nullable": true
          },
          "password": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "UpdatedCommentDto": {
        "type": "object",
        "properties": {
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "UpdatedForumDto": {
        "type": "object",
        "properties": {
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "UpdatedPostDto": {
        "type": "object",
        "properties": {
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      }
    }
  }
}
