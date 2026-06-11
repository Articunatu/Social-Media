# Domain-Driven Design Refactoring Guide

## Migrating an Existing Social Media .NET Application to a DDD Architecture

---

# 1. Purpose

This document describes the target Domain-Driven Design (DDD) architecture for a social media platform built with:

* .NET
* ASP.NET Core
* Entity Framework Core
* Microsoft SQL Server

It also describes a safe incremental migration path from an existing database-first or CRUD-oriented architecture where domain models have grown into large entities.

The goal is not to rewrite the entire system.

The goal is to gradually move from:

```
Database
    |
    |
EF Entities
    |
    |
Controllers
```

towards:

```
User Intent

    |
    v

Application Commands

    |
    v

Domain Model

    |
    v

Persistence
```

---

# 2. Current Common Problem

Many social applications start with models like:

```csharp
public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; }


    public ICollection<Post> Posts { get; set; }

    public ICollection<Comment> Comments { get; set; }

    public ICollection<Message> Messages { get; set; }

    public ICollection<Photo> Photos { get; set; }

    public ICollection<Friend> Friends { get; set; }
}
```

This creates a "God Entity".

Problems:

* User becomes responsible for everything.
* EF Core loads huge graphs.
* Business rules become scattered.
* Changing one feature affects unrelated areas.
* Testing becomes difficult.
* The database becomes the architecture.

DDD solves this by creating boundaries.

---

# 3. DDD Principles Used

## 3.1 Domain Model First

The domain model represents business rules.

Example:

Bad:

```csharp
post.Text = value;
db.SaveChanges();
```

Good:

```csharp
post.EditContent(value);
```

The domain decides if the operation is valid.

---

# 3.2 Bounded Contexts

A social platform is not one domain.

It contains multiple subdomains.

Target:

```
SocialMedia

 |
 |
 +-- Identity
 |
 +-- Content
 |
 +-- Messaging
 |
 +-- SocialGraph
 |
 +-- Media
 |
 +-- Notifications
```

---

# 4. Target Architecture

Solution:

```
SocialMedia.sln


src/

SocialMedia.Api

SocialMedia.Application

SocialMedia.Domain

SocialMedia.Infrastructure

SocialMedia.Persistence
```

---

# 5. Domain Boundaries

## Identity

Responsible for:

* User account
* Authentication
* Credentials
* Sessions

Owns:

```
User
RefreshToken
Account
```

Does NOT own:

```
Posts
Messages
Followers
```

---

## Content

Responsible for:

* Posts
* Comments
* Reactions

Owns:

```
Post
Comment
Reaction
```

---

## Messaging

Responsible for:

* Direct messages
* Conversations
* Read state

Owns:

```
Conversation
Message
ConversationMember
```

---

## Social Graph

Responsible for:

* Follow
* Block
* Friend relationships

Owns:

```
Follow
Block
FriendRequest
```

---

## Media

Responsible for:

* Uploaded files
* Images
* Videos

Owns:

```
MediaFile
```

---

# 6. Aggregate Design

An Aggregate is a consistency boundary.

Do not create:

```
User

 |
 + Posts
 + Comments
 + Messages
 + Photos
 + Followers
```

Instead:

```
User Aggregate


Post Aggregate

Post
 |
 + Comment
 + Reaction


Conversation Aggregate

Conversation
 |
 + Message
```

---

# 7. Content Domain Example

## Post

```csharp
public class Post : AggregateRoot
{

private readonly List<Comment> _comments = new();

private readonly List<Reaction> _reactions = new();


public Guid Id { get; private set; }


public Guid AuthorId { get; private set; }


public string Text { get; private set; }



public void AddComment(
    Guid userId,
    string text)
{

if(string.IsNullOrWhiteSpace(text))
    throw new DomainException(
        "Comment cannot be empty");


_comments.Add(
    new Comment(
        userId,
        text));

}

}
```

The Post controls:

* comment creation
* validation
* reactions

---

# 8. Value Objects

Avoid:

```csharp
string Email
```

Use:

```csharp
public class EmailAddress
{

public string Value {get;}


public EmailAddress(string value)
{

if(!value.Contains("@"))
    throw new DomainException();


Value=value;

}

}
```

---

# 9. Domain Events

Domains communicate through events.

Example:

Comment added:

```
Content

Post.AddComment()

        |
        v

CommentAddedEvent

        |
        +---- Notification
        |
        +---- Analytics
```

Example:

```csharp
AddDomainEvent(
 new CommentAddedEvent(
     PostId,
     CommentId));
```

---

# 10. Database Design

SQL Server:

```
SocialMediaDb


Identity schema

Users
RefreshTokens


Content schema

Posts
Comments
Reactions
PostMedia


Messaging schema

Conversations
ConversationMembers
Messages


Social schema

Follows
Blocks


Media schema

Files
```

---

# 11. EF Core Mapping

Example:

```csharp
public class PostConfiguration 
    : IEntityTypeConfiguration<Post>
{

public void Configure(
EntityTypeBuilder<Post> builder)
{

builder.ToTable(
"Posts",
"Content");


builder.HasKey(x=>x.Id);


builder.Property(x=>x.Text)
.HasMaxLength(5000);

}

}
```

---

# 12. Migration Strategy

IMPORTANT:

Do not rewrite.

Use incremental migration.

---

# Phase 0 - Stabilize Existing System

Goals:

* Stop adding features to giant entities.
* Add tests around existing behavior.

Create:

```
Characterization Tests
```

These describe current behavior.

---

# Phase 1 - Introduce Application Layer

Current:

```
Controller

   |
DbContext

   |
Entity
```

Move toward:

```
Controller

 |
Command

 |
Handler

 |
Domain
```

Example:

Before:

```csharp
db.Posts.Add(post);
```

After:

```csharp
CreatePostCommand

CreatePostHandler

Post.Create()
```

---

# Phase 2 - Extract Content Context

Move:

```
User.Posts
User.Comments
User.Reactions
```

into:

```
Content

Post
Comment
Reaction
```

Database:

Before:

```
dbo.Posts
```

After:

```
Content.Posts
```

---

# Phase 3 - Remove Entity Navigation Abuse

Before:

```csharp
Post.Author
```

After:

```csharp
Post.AuthorId
```

Reason:

The Post domain does not own Identity.

---

# Phase 4 - Extract Messaging

Move:

```
User.Messages
```

into:

```
Messaging

Conversation
Message
```

New behavior:

```csharp
conversation.SendMessage(
 userId,
 text);
```

---

# Phase 5 - Extract Social Graph

Move:

```
User.Friends
User.Followers
```

into:

```
SocialGraph
```

Database:

```
Follows

FollowerId
FollowingId
```

---

# Phase 6 - Introduce Domain Events

Replace:

```csharp
post.Save();

notification.Send();
```

with:

```
PostCreatedEvent

        |
        + Notification
        + Search
        + Analytics
```

---

# 13. Refactoring Rules

## Rule 1

Entities protect themselves.

Bad:

```csharp
entity.Property=value;
```

Good:

```csharp
entity.ChangeProperty(value);
```

---

## Rule 2

Do not share entities between contexts.

Bad:

```
Content.Post
references
Identity.User
```

Good:

```
Content.Post

AuthorId
```

---

## Rule 3

Repositories are aggregate based.

Good:

```csharp
IPostRepository
```

Bad:

```csharp
IGenericRepository<TEntity>
```

---

# 14. Recommended Migration Order

```
1. Add tests

2. Add Application layer

3. Extract Content

4. Extract Messaging

5. Extract Social Graph

6. Add Domain Events

7. Split DbContexts

8. Split databases only if needed
```

---

# 15. Final Target

The final architecture:

```
Identity

User
Account


Content

Post
Comment
Reaction


Messaging

Conversation
Message


Social

Follow
Block


Media

File


Notifications

Notification
```

Each area:

* owns its rules
* owns its data
* communicates through contracts/events

---

# Final Principle

Do not organize the application around tables.

Do not organize the application around EF entities.

Organize around business capabilities.

The database is a storage mechanism.

The domain model is where the business lives.
